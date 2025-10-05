using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Parties.Parties.Dtos;

namespace NextGen.Modules.Parties.Parties.Features.GettingParties;

public record GetPartiesResponse(ListResultModel<PartyReadDto> Parties);
