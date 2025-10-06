using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Identity.Claims.Dtos;
using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Claims.Features.GetClaims;

public record GetClaimResponse (ListResultModel<ClaimDto> Claims);
