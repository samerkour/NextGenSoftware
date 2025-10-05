using BuildingBlocks.Abstractions.CQRS.Query;

namespace NextGen.Modules.Identity.Claims.Features.GetClaimById
{
    public record GetClaimByIdQuery(Guid Id) : IQuery<GetClaimByIdResponse>;
}
