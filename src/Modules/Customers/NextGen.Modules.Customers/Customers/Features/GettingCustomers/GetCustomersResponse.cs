using BuildingBlocks.Core.CQRS.Query;
using NextGen.Modules.Customers.Customers.Dtos;

namespace NextGen.Modules.Customers.Customers.Features.GettingCustomers;

public record GetCustomersResponse(ListResultModel<CustomerReadDto> Customers);
