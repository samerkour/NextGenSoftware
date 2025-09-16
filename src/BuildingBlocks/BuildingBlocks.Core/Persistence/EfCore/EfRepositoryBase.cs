using System.Linq.Expressions;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Event;
using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Abstractions.PagedList;
using BuildingBlocks.Abstractions.Persistence.EfCore;
using BuildingBlocks.Core.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BuildingBlocks.Core.Persistence.EfCore;

public abstract class EfRepositoryBase<TDbContext, TEntity, TKey> :
       IEfRepository<TEntity, TKey>,
       IPageRepository<TEntity, TKey>
       where TEntity : class, IHaveIdentity<TKey>
       where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;
    private readonly IAggregatesDomainEventsRequestStore _domainEventsStore;

    protected EfRepositoryBase(TDbContext dbContext, IAggregatesDomainEventsRequestStore domainEventsStore)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<TEntity>();
        _domainEventsStore = domainEventsStore;
    }

    #region Query Helpers

    protected virtual IQueryable<TEntity> BuildQuery(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = DbSet;

        if (disableTracking)
            query = query.AsNoTracking();
        if (include != null)
            query = include(query);
        if (filter != null)
            query = query.Where(filter);

        return orderBy != null ? orderBy(query) : query;
    }

    #endregion

    #region Get / Find

    public virtual TEntity? Find(params object[] keyValues) => DbSet.Find(keyValues);

    public virtual async Task<TEntity?> FindAsync(params object[] keyValues) =>
        await DbSet.FindAsync(keyValues);

    public virtual async Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default) =>
        await DbSet.SingleOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);

    public virtual async Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await DbSet.SingleOrDefaultAsync(predicate, cancellationToken);

    public virtual async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await DbSet.Where(predicate).ToListAsync(cancellationToken);

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet.ToListAsync(cancellationToken);

    #endregion

    #region Includes

    public virtual IEnumerable<TEntity> GetInclude(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        return BuildQuery(predicate, include: include, disableTracking: disableTracking).AsEnumerable();
    }

    public virtual async Task<IEnumerable<TEntity>> GetIncludeAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true)
    {
        return await BuildQuery(predicate, include: include, disableTracking: disableTracking).ToListAsync();
    }

    #endregion

    #region Paging

    public virtual IPagedList<TEntity> GetPagedList(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 1,
        int pageSize = 20,
        bool disableTracking = true)
    {
        return BuildQuery(filter, orderBy, include, disableTracking)
            .ToPagedList(pageIndex, pageSize);
    }

    public virtual async Task<IPagedList<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 1,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(filter, orderBy, include, disableTracking)
            .ToPagedListAsync(pageIndex, pageSize, cancellationToken: cancellationToken);
    }

    public virtual async Task<IPagedList<TResult>> GetPagedListAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 1,
        int pageSize = 20,
        bool disableTracking = true,
        CancellationToken cancellationToken = default) where TResult : class
    {
        return await BuildQuery(filter, orderBy, include, disableTracking)
            .Select(selector)
            .ToPagedListAsync(pageIndex, pageSize, cancellationToken: cancellationToken);
    }

    #endregion

    #region CRUD

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));
        await DbSet.AddAsync(entity, cancellationToken);
        // TODO: register domain events with _domainEventsStore
        return entity;
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));
        DbContext.Entry(entity).State = EntityState.Modified;
        return Task.FromResult(entity);
    }

    public virtual void Update(TEntity entity) => DbSet.Update(entity);

    public virtual void Delete(TEntity entity) => DbSet.Remove(entity);

    public virtual async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);
        Guard.Against.NotFound(id?.ToString() ?? "", nameof(TEntity), nameof(id));
        DbSet.Remove(entity!);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));
        DbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(entities, nameof(entities));
        DbSet.RemoveRange(entities);
        await Task.CompletedTask;
    }

    #endregion

    #region Counts

    public virtual int Count(Expression<Func<TEntity, bool>>? filter = null) =>
        filter == null ? DbSet.Count() : DbSet.Count(filter);

    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default) =>
        filter == null ? DbSet.CountAsync(cancellationToken) : DbSet.CountAsync(filter, cancellationToken);

    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}
