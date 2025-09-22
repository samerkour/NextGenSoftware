using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Inventories.Products.Features.CreatingProduct.Events.Integration;

public record ProductCreated(long Id, string Name, long CategoryId, string CategoryName, int Stock) :
    IntegrationEvent;
