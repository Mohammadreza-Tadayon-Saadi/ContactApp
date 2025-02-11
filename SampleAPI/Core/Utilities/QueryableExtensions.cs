using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Core.Utilities;

public static class QueryableExtensions
{
	public static QueryableExtensionsResult<IQueryable<T>> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool descending)
	{
		if (propertyName.HasValue())
		{
			try
			{
				var parameter = Expression.Parameter(typeof(T), "x");
				var property = GetPropertyExpression(parameter, propertyName);
				var selector = Expression.Lambda(property, parameter);

				var orderByMethod = descending ? "OrderByDescending" : "OrderBy";
				var orderByExpression = Expression.Call(typeof(Queryable), orderByMethod, [typeof(T), property.Type],
					source.Expression, Expression.Quote(selector));

				return QueryableExtensionsResult<IQueryable<T>>.Success(source.Provider.CreateQuery<T>(orderByExpression));
			}
			catch (Exception)
			{
				return QueryableExtensionsResult<IQueryable<T>>.Failure();
			}
		}

		return QueryableExtensionsResult<IQueryable<T>>.Failure();
	}

	public static QueryableExtensionsResult<IEnumerable<T>> OrderByProperty<T>(this IEnumerable<T> source, string propertyName, bool descending)
	{
		if (propertyName.HasValue())
		{
			try
			{
				var propertyNames = propertyName.Split('.'); // Handle nested properties

				var propertyExpression = GetPropertyExpression<T>(propertyNames);

				var orderedQuery = descending ?
					source.OrderByDescending(propertyExpression) :
					source.OrderBy(propertyExpression);

				return QueryableExtensionsResult<IEnumerable<T>>.Success(orderedQuery);
			}
			catch (Exception)
			{
				return QueryableExtensionsResult<IEnumerable<T>>.Failure();
			}
		}

		return QueryableExtensionsResult<IEnumerable<T>>.Failure();
	}

	private static Func<T, object> GetPropertyExpression<T>(string[] propertyNames)
	{
		var parameter = Expression.Parameter(typeof(T), "x");
		Expression propertyExpression = parameter;

		foreach (var name in propertyNames)
		{
			var property = typeof(T).GetProperty(name);
			propertyExpression = Expression.Property(propertyExpression, property);
		}

		var conversion = Expression.Convert(propertyExpression, typeof(object));
		return Expression.Lambda<Func<T, object>>(conversion, parameter).Compile();
	}

	public static IQueryable<T> ToPageData<T>(this IQueryable<T> source, int skip, int take)
		=> source.Skip(skip).Take(take);

	public static async Task<IEnumerable<T>> ToPageDataAsync<T>(this IQueryable<T> source, int skip, int take, CancellationToken cancellationToken = default)
		=> await source.Skip(skip).Take(take).ToListAsync(cancellationToken);

	public static IQueryable<T> WhereProperty<T>(this IQueryable<T> source, string propertyName, string propertyValue, bool withContains = false)
	{
		var type = typeof(T);
		var parameter = Expression.Parameter(type, type.Name);
		var property = GetPropertyExpression(parameter, propertyName);

		if (property is not null)
		{
			// Convert the property value to the property type
			var convertedValue = property.Type.IsEnum ?
                Enum.Parse(property.Type, propertyValue) : Convert.ChangeType(propertyValue, property.Type);

			var value = Expression.Constant(convertedValue);

			if (withContains && property.Type == typeof(string))
			{
				var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
				var containsExpression = Expression.Call(property, containsMethod, value);

				var lambdaExpression = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
				return source.Where(lambdaExpression);
			}
			else
			{
				var equalsExpression = Expression.Equal(property, value);
				var lambdaExpression = Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);

				return source.Where(lambdaExpression);
			}
		}
		else
		{
			// Search in the navigation properties recursively
			var navigationProperties = typeof(T).GetProperties()
				.Where(p => (p.PropertyType.IsClass && p.PropertyType != typeof(string)) || p.PropertyType.IsInterface);

			foreach (var navigationProperty in navigationProperties)
			{
				var subQuery = source.WhereProperty($"{navigationProperty.Name}.{propertyName}", propertyValue, withContains);
				if (subQuery.Any())
					return subQuery;
			}

			return source;
		}
	}

	private static MemberExpression GetPropertyExpression(Expression parameter, string propertyName)
	{
		try
		{
			var propertyNames = propertyName.Split('.');
			Expression currentExpression = parameter;

			foreach (var name in propertyNames)
			{
				var currentType = currentExpression.Type;
				var currentProperty = currentType.GetProperty(name);

				if (currentProperty is null)
				{
					var navigationProperty = currentType.GetProperties()
						.FirstOrDefault(p => p.Name == name);

					if (navigationProperty is null)
						return null;

					currentExpression = Expression.Property(currentExpression, navigationProperty);
				}
				else
					currentExpression = Expression.Property(currentExpression, currentProperty);
			}

			if (currentExpression is MemberExpression memberExpression)
				return memberExpression;

			return null;
		}
		catch (Exception)
		{
			return null;
		}
	}
}

public record QueryableExtensionsResult<TValue>
{
	private QueryableExtensionsResult(TValue value, bool isSuccess)
	{
		if (isSuccess && value is null)
			throw new InvalidOperationException();

		if (!isSuccess && value is not null)
			throw new InvalidOperationException();

		Value = value;
		IsSuccess = isSuccess;
	}

	public TValue Value { get; }
	public bool IsSuccess { get; }
	public bool IsFailure { get => !IsSuccess; }

	public static QueryableExtensionsResult<TValue> Success(TValue value)
		=> new(value, true);

	public static QueryableExtensionsResult<TValue> Failure()
		=> new(default!, false);

	public static implicit operator QueryableExtensionsResult<TValue>(TValue value)
		=> Success(value);
}