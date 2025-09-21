using System.Linq.Expressions;
using AutoMapper;
using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Abstractions.PagedList;
using Microsoft.EntityFrameworkCore.Query;

namespace BuildingBlocks.Abstractions.Persistence.EfCore;

public interface IEfRepository<TEntity, TKey> where TEntity : class, IHaveIdentity<TKey> 
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void AddRange(IEnumerable<TEntity> entities);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    int Count(Expression<Func<TEntity, bool>>? filter = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);
    void Delete(TEntity entity);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default);
    void Dispose();
    TEntity? Find(params object[] keyValues);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(params object[] keyValues);
    Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    IEnumerable<TEntity> GetInclude(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool disableTracking = true);
    Task<IEnumerable<TEntity>> GetIncludeAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool disableTracking = true);
    IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? saleBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 1, int pageSize = 20, bool disableTracking = true);
    Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? saleBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 1, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? saleBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 1, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default) where TResult : class;
    void Update(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}

