using System.Linq.Expressions;
using System.Reflection;
using Tap.Domain.Core.Exceptions;

namespace Tap.Application.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(
        this IQueryable<T> source,
        string propertyName,
        string sortOrder = "asc"
    )
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (propertyName == null)
            throw new ArgumentNullException(nameof(propertyName));

        var propertyInfo = typeof(T).GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
        );
        if (propertyInfo is null)
        {
            throw new PropertyNotFoundException(
                $"No property '{propertyName}' found on type '{typeof(T).Name}'"
            );
        }

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);

        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? "OrderByDescending"
            : "OrderBy";

        var genericMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);

        var result =
            (IQueryable<T>)genericMethod.Invoke(null, new object[] { source, orderByExpression })!;

        return result;
    }

    public static IQueryable<T> FilterBy<T>(
        this IQueryable<T> source,
        string? propertyName,
        string? filterQuery
    )
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filterQuery))
        {
            return source;
        }

        var propertyInfo = typeof(T).GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
        );
        if (propertyInfo is null)
        {
            throw new PropertyNotFoundException(
                $"No property '{propertyName}' found on type '{typeof(T).Name}'"
            );
        }
        if (propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType != typeof(int))
        {
            throw new PropertyNotFoundException(
                $"Property '{propertyName}' is not of type string or int."
            );
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);

        Expression filterExpression;
        if (propertyInfo.PropertyType == typeof(string))
        {
            var constant = Expression.Constant(filterQuery);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            filterExpression = Expression.Call(propertyAccess, containsMethod, constant);
        }
        else
        {
            var constant = Expression.Constant(int.Parse(filterQuery));
            filterExpression = Expression.Equal(propertyAccess, constant);
        }

        var whereExpression = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);

        return source.Where(whereExpression);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}
