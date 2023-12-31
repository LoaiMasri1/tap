using System.Linq.Expressions;
using System.Reflection;

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
        if (propertyInfo == null)
        {
            throw new ArgumentException(
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
}
