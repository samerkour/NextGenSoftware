using BuildingBlocks.Core.Messaging;

namespace NextGen.Modules.Inventorys.Products.Features.DebitingProductStock.Events.Integration;

public record ProductStockDebited(long ProductId, int NewStock, int DebitedQuantity) : IntegrationEvent;
