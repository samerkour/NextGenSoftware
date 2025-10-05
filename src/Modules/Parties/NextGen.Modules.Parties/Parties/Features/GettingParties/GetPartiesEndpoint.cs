using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Query;
using Swashbuckle.AspNetCore.Annotations;


namespace NextGen.Modules.Parties.Parties.Features.GettingParties;

// https://www.youtube.com/watch?v=SDu0MA6TmuM
// https://github.com/ardalis/ApiEndpoints
public class GetPartiesEndpoint : EndpointBaseAsync
    .WithRequest<GetPartiesRequest?>
    .WithActionResult<GetPartiesResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetPartiesEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(PartiesConfigs.PartiesPrefixUri, Name = "GetParties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get all parties",
        Description = "Get all parties",
        OperationId = "GetParties",
        Tags = new[] { PartiesConfigs.Tag })]
    public override async Task<ActionResult<GetPartiesResponse>> HandleAsync(
        [FromQuery] GetPartiesRequest? request,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(request, nameof(request));

        var result = await _queryProcessor.SendAsync(
            new GetParties
            {
                Filters = request.Filters,
                Includes = request.Includes,
                Page = request.Page,
                Sorts = request.Sorts,
                PageSize = request.PageSize
            },
            cancellationToken);

        return Ok(result);
    }
}
