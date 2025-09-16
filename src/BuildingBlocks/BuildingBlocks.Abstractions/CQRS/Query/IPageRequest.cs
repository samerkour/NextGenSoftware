namespace BuildingBlocks.Abstractions.CQRS.Query;

public interface IPageRequest
{
    int Page { get; init; }
    int PageSize { get; init; }
    IList<string>? Includes { get; init; }
    IList<FilterModel>? Filters { get; init; }
    IList<string>? Sorts { get; init; }
}
