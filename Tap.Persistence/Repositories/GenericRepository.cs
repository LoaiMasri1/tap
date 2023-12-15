using Microsoft.EntityFrameworkCore;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives;
using System.Linq.Expressions;
using Tap.Application.Core.Abstractions.Data;

namespace Tap.Persistence.Repositories;

internal abstract class GenericRepository<TEntity>
    where TEntity : Entity
{
    protected GenericRepository(IDbContext dbContext) => DbContext = dbContext;

    protected IDbContext DbContext { get; }

    public async Task<Maybe<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await DbContext.GetBydIdAsync<TEntity>(id, cancellationToken);

    public async Task<Maybe<TEntity>> GetByAsync(Expression<Func<TEntity,bool>> predicate,
        CancellationToken cancellationToken) =>
        await DbContext.GetByAsync(predicate, cancellationToken);

    public void Insert(TEntity entity) => DbContext.Insert(entity);

    public void InsertRange(IReadOnlyCollection<TEntity> entities) =>
        DbContext.InsertRange(entities);

    public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

    public void Remove(TEntity entity) => DbContext.Remove(entity);

    protected async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) => await DbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);

    protected async Task<Maybe<TEntity>> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) => (await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken))!;
}
