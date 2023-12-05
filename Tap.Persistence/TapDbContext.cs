using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Persistence;

public class TapDbContext : DbContext, IDbContext, IUnitOfWork
{
    private readonly IDateTime _dateTime;

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity => base.Set<TEntity>();

    public TapDbContext(DbContextOptions<TapDbContext> options, IDateTime dateTime)
        : base(options)
    {
        _dateTime = dateTime;
    }

    public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(
        int id,
        CancellationToken cancellationToken
    )
        where TEntity : Entity =>
        id == default
            ? Maybe<TEntity>.None
            : Maybe<TEntity>.From(
                (await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken))!
            );

    public void Insert<TEntity>(TEntity entity)
        where TEntity : Entity => Set<TEntity>().Add(entity);

    public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : Entity => Set<TEntity>().AddRange(entities);

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : Entity => Set<TEntity>().Remove(entity);

    public Task<int> ExecuteSqlAsync(
        string sql,
        IEnumerable<SqlParameter> parameters,
        CancellationToken cancellationToken = default
    ) => Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DateTime utcNow = _dateTime.UtcNow;

        UpdateAuditableEntities(utcNow);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    ) => Database.BeginTransactionAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    private void UpdateAuditableEntities(DateTime utcNow)
    {
        foreach (
            EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>()
        )
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedAtUtc)).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.UpdatedAtUtc)).CurrentValue = utcNow;
            }
        }
    }
}
