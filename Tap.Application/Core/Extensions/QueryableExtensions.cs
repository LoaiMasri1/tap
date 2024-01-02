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

    public static IQueryable<T> Where<T>(
        this IQueryable<T> source,
        string propertyName,
        string filterQuery
    )
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (propertyName == null)
            throw new ArgumentNullException(nameof(propertyName));
        if (filterQuery == null)
            throw new ArgumentNullException(nameof(filterQuery));

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

        if (propertyInfo.PropertyType != typeof(string))
        {
            throw new PropertyNotFoundException(
                $"Property '{propertyName}' is not of type string."
            );
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
        var constant = Expression.Constant(filterQuery);
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
        var containsExpression = Expression.Call(propertyAccess, containsMethod, constant);
        var whereExpression = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

        return source.Where(whereExpression);
    }
}
