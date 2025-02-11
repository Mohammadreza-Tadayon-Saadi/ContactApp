using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace Infrastructure.Context;

public class DataBaseContext(DbContextOptions<DataBaseContext> options)
	: DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var assembly = typeof(DataBaseContext).Assembly;

		modelBuilder.RegisterAllConfiguration<Entity>(assembly, assembly);

        base.OnModelCreating(modelBuilder);
	}

    #region Override SaveChangeMehod

    public override int SaveChanges()
	{
		CleanString();
		return base.SaveChanges();
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		CleanString();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		CleanString();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		CleanString();
		return base.SaveChangesAsync(cancellationToken);
	}

	protected void CleanString()
	{
		var changedEntities = ChangeTracker.Entries()
								.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

		foreach (var item in changedEntities)
		{
			if (item.Entity is null)
				continue;

			var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
								.Where(p => p.CanWrite && p.CanRead && p.PropertyType == typeof(string));

			foreach (var property in properties)
			{
				var propName = property.Name;
				var value = (string)property.GetValue(item.Entity, null);

				if (value.HasValue())
				{
					var newValue = "";
					try
					{
						newValue = value.Trim().Fa2En().FixPersianChars();
					}
					catch
					{
						newValue = value.Fa2En().FixPersianChars();
					}
					if (newValue == value)
						continue;

					property.SetValue(item.Entity, newValue, null);
				}
			}
		}
	}

	#endregion Override SaveChangeMehod
}