using System.Linq.Expressions;
using System.Reflection;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Persistence;

public class TapDbContext : DbContext, IDbContext, IUnitOfWork
{
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    public TapDbContext(
        DbContextOptions<TapDbContext> options,
        IDateTime dateTime,
        IMediator mediator
    )
        : base(options)
    {
        _dateTime = dateTime;
        _mediator = mediator;
    }

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity
    {
        return base.Set<TEntity>();
    }

    public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(
        int id,
        CancellationToken cancellationToken
    )
        where TEntity : Entity
    {
        return id == default
            ? Maybe<TEntity>.None
            : Maybe<TEntity>.From(
                (await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken))!
            );
    }

    public async Task<Maybe<TEntity>> GetByAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    )
        where TEntity : Entity
    {
        return Maybe<TEntity>.From(
            (await Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken))!
        );
    }

    public void Insert<TEntity>(TEntity entity)
        where TEntity : Entity
    {
        Set<TEntity>().Add(entity);
    }

    public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : Entity
    {
        Set<TEntity>().AddRange(entities);
    }

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : Entity
    {
        Set<TEntity>().Remove(entity);
    }

    public Task<int> ExecuteSqlAsync(
        string sql,
        IEnumerable<SqlParameter> parameters,
        CancellationToken cancellationToken = default
    )
    {
        return Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = _dateTime.UtcNow;

        UpdateAuditableEntities(utcNow);

        await PublishDomainEvents(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    )
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    private void UpdateAuditableEntities(DateTime utcNow)
    {
        foreach (var entityEntry in ChangeTracker.Entries<IAuditableEntity>())
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

    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
            .ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        var tasks = domainEvents.Select(
            domainEvent => _mediator.Publish(domainEvent, cancellationToken)
        );

        await Task.WhenAll(tasks);
    }
}
