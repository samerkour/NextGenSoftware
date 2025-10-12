using BuildingBlocks.Abstractions.CQRS.Query;
using System;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroupById
{
    public record GetClaimGroupByIdQuery(Guid Id) : IQuery<GetClaimGroupByIdResponse>;
}
