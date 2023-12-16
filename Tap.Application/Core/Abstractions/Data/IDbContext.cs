using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Core.Abstractions.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity;

        Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(int id, CancellationToken cancellationToken)
            where TEntity : Entity;

        Task<Maybe<TEntity>> GetByAsync<TEntity>(Expression<Func<TEntity, bool>> predicate
            , CancellationToken cancellationToken)
            where TEntity : Entity;

        void Insert<TEntity>(TEntity entity)
            where TEntity : Entity;

        void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
            where TEntity : Entity;

        void Remove<TEntity>(TEntity entity)
            where TEntity : Entity;

        Task<int> ExecuteSqlAsync(
            string sql,
            IEnumerable<SqlParameter> parameters,
            CancellationToken cancellationToken = default
        );
    }
}