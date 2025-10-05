using BuildingBlocks.Abstractions.PagedList;

namespace BuildingBlocks.Core.PagedList
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPagedList{T}"/> interface.
    /// </summary>
    public class PagedList<T> : IPagedList<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int IndexFrom { get; set; }
        public IList<T> Items { get; set; } = new List<T>();

        public bool HasPreviousPage => PageIndex - IndexFrom > 0;
        public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;
    }

    /// <summary>
    /// Provides some helper methods for <see cref="IPagedList{T}"/>.
    /// </summary>
    public static class PagedList
    {
        /// <summary>
        /// Creates an empty paged list.
        /// </summary>
        public static IPagedList<T> Empty<T>() => new PagedList<T>
        {
            PageIndex = 1,
            PageSize = 0,
            IndexFrom = 1,
            TotalCount = 0,
            TotalPages = 0,
            Items = new List<T>()
        };
    }

}
