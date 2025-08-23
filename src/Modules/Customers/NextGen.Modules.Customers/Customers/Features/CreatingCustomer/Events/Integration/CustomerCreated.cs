using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Customers.Customers.Features.CreatingCustomer.Events.Integration;

public record CustomerCreated(long CustomerId) : IntegrationEvent;
