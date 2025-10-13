using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Identity.Claims.Dtos;

namespace NextGen.Modules.Identity.Claims.Features.GetClaimById;

public record GetClaimByIdResponse(ClaimDto Claim);
