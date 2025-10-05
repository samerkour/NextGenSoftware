using BuildingBlocks.Core.CQRS.Query;

namespace NextGen.Modules.Parties.Parties.Features.GettingParties;

// https://blog.codingmilitia.com/2022/01/03/getting-complex-type-as-simple-type-query-string-aspnet-core-api-controller/
// https://benfoster.io/blog/minimal-apis-custom-model-binding-aspnet-6/
public record GetPartiesRequest : PageRequest
{
    // // For handling in minimal api
    // public static ValueTask<GetPartiesRequest?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    // {
    //     var page = httpContext.Request.Query.Get<int>("Page", 1);
    //     var pageSize = httpContext.Request.Query.Get<int>("PageSize", 20);
    //     var partyState = httpContext.Request.Query.Get<PartyState>("PartyState", PartyState.None);
    //     var sorts = httpContext.Request.Query.GetCollection<List<string>>("Sorts");
    //     var filters = httpContext.Request.Query.GetCollection<List<FilterModel>>("Filters");
    //     var includes = httpContext.Request.Query.GetCollection<List<string>>("Includes");
    //
    //     var request = new GetPartiesRequest()
    //     {
    //         Page = page,
    //         PageSize = pageSize,
    //         PartyState = partyState,
    //         Sorts = sorts,
    //         Filters = filters,
    //         Includes = includes
    //     };
    //
    //     return ValueTask.FromResult<GetPartiesRequest?>(request);
    // }
}
