using BuildingBlocks.Abstractions.PagedList;

namespace BuildingBlocks.Core.PagedList
{
    /// <summary>
    /// Provides extension methods for IEnumerable to support paging.
    /// </summary>
    public static class IEnumerablePagedListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(
            this IEnumerable<T> source,
            int pageIndex,
            int pageSize,
            int indexFrom = 1)
        {
            if (pageIndex < indexFrom)
                pageIndex = indexFrom;
            if (pageSize <= 0)
                pageSize = 10;

            var totalItems = source.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                IndexFrom = indexFrom,
                TotalCount = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }
    }
}
