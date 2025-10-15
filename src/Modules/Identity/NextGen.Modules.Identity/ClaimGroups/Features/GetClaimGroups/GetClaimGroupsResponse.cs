using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Identity.ClaimGroups.Dtos;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;


public record GetClaimGroupsResponse(ListResultModel<ClaimGroupDto> ClaimGroups);
