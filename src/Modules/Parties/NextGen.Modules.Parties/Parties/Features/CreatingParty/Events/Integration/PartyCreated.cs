using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Parties.Parties.Features.CreatingParty.Events.Integration;

public record PartyCreated(long PartyId) : IntegrationEvent;
