using BuildingBlocks.Abstractions.PagedList;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.PagedList
{
    /// <summary>
    /// Provides async/sync extension methods for IQueryable to support EF Core paging.
    /// </summary>
    public static class IQueryablePagedListExtensions
    {
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(
            this IQueryable<T> source,
            int page = 1,
            int pageSize = 10,
            int indexFrom = 1,
            CancellationToken cancellationToken = default)
        {
            if (page < indexFrom)
                page = indexFrom;
            if (pageSize <= 0)
                pageSize = 10;

            var isEmpty = await source.AnyAsync(cancellationToken) == false;
            if (isEmpty)
                return PagedList.Empty<T>();

            var totalItems = await source.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = await source.Limit(page, pageSize).ToListAsync(cancellationToken);

            return new PagedList<T>
            {
                PageIndex = page,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        public static IPagedList<T> ToPagedList<T>(
            this IQueryable<T> source,
            int page = 1,
            int pageSize = 10,
            int indexFrom = 1)
        {
            if (page < indexFrom)
                page = indexFrom;
            if (pageSize <= 0)
                pageSize = 10;

            var totalItems = source.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = source.Limit(page, pageSize).ToList();

            return new PagedList<T>
            {
                PageIndex = page,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        /// <summary>
        /// Internal helper to apply Skip/Take.
        /// </summary>
        public static IQueryable<T> Limit<T>(
            this IQueryable<T> query,
            int page,
            int pageSize)
        {
            if (page <= 0)
                page = 1;
            if (pageSize <= 0)
                pageSize = 10;

            var skip = (page - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }
    }

}
