using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Utilities;

public static class ModelBuilderExtensions
{
    public static void RegisterAllConfiguration<BaseType>(this ModelBuilder modelBuilder, 
        Assembly entitiesAssembly, Assembly fluentApiAssembly)
    {
		modelBuilder.RegisterAllEntities<BaseType>(entitiesAssembly);
		modelBuilder.SetVersionToTimeStampForEntities<BaseType>();
		modelBuilder.ApplyConfigurationsFromAssembly(fluentApiAssembly);
		modelBuilder.AddDeleteBehaviorConvention<BaseType>();
        //modelBuilder.SetIsDeleteQueryFilterForEntities<BaseType>();
        //modelBuilder.SetIsActiveQueryFilterForEntities<BaseType>();
    }

	/// <summary>
	/// Set DeleteBehavior for relations
	/// </summary>
	/// <param name="modelBuilder"></param>
	private static void AddDeleteBehaviorConvention<BaseType>(this ModelBuilder modelBuilder)
    {
        var entities = modelBuilder.Model.GetEntityTypes().Where(t => typeof(BaseType).IsAssignableFrom(t.ClrType));

		foreach (var entity in entities)
        {
			var attribute = entity.ClrType.GetCustomAttribute<CasCadeDeleteBehaviorAttribute>();

            // Set NoAction for default behavior
			var behavior = attribute?.DeleteBehavior ?? DeleteBehavior.NoAction;
			var cascadeFKs = entity.GetForeignKeys().Where(fk => !fk.IsOwnership);

			foreach (var fk in cascadeFKs)
				fk.DeleteBehavior = behavior;
		}
    }

	/// <summary>
	/// Dynamicaly Set !IsDelete queryFilter for all Entities that have IsDelete property
	/// </summary>
	/// <param name="modelBuilder"></param>
	private static void SetIsDeleteQueryFilterForEntities<BaseType>(this ModelBuilder modelBuilder)
	{
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(t => typeof(BaseType).IsAssignableFrom(t.ClrType)))
        {
            var isDeletedProperty = entityType.ClrType.GetProperty("IsDeleted");
            if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(Expression.Property(parameter, "IsDeleted"), Expression.Constant(false));

                var lambda = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }

            var isDeleteProperty = entityType.ClrType.GetProperty("IsDelete");
            if (isDeleteProperty != null && isDeleteProperty.PropertyType == typeof(bool))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(Expression.Property(parameter, "IsDelete"), Expression.Constant(false));

                var lambda = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    /// <summary>
    /// Dynamicaly Set IsActive queryFilter for all Entities that have IsDelete property
    /// </summary>
    /// <param name="modelBuilder"></param>
    private static void SetIsActiveQueryFilterForEntities<BaseType>(this ModelBuilder modelBuilder)
	{
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(t => typeof(BaseType).IsAssignableFrom(t.ClrType)))
        {
            var isActiveProperty = entityType.ClrType.GetProperty("IsActive");
            if (isActiveProperty != null && isActiveProperty.PropertyType == typeof(bool))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(Expression.Property(parameter, "IsActive"), Expression.Constant(true));

                var lambda = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    /// <summary>
    /// Dynamicaly register all Entities that inherit from specific BaseType
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="baseType">Base type that Entities inherit from this</param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterAllEntities<BaseType>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseType).IsAssignableFrom(c));

        foreach (Type type in types)
            modelBuilder.Entity(type);
	}

	/// <summary>
	/// Dynamicaly Set Version Property To Be TimeStamp For Concurrency Checking
	/// </summary>
	/// <param name="modelBuilder"></param>
	private static void SetVersionToTimeStampForEntities<BaseType>(this ModelBuilder modelBuilder)
	{
		foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(t => typeof(BaseType).IsAssignableFrom(t.ClrType)))
		{
			var versionProperty = entityType.ClrType.GetProperty("Version");
            if (versionProperty is not null && versionProperty.PropertyType == typeof(byte[]))
                modelBuilder.Entity(entityType.ClrType)
                    //.Ignore(versionProperty.Name);
                    .Property(versionProperty.Name)
                    .IsRowVersion();
		}
	}
}