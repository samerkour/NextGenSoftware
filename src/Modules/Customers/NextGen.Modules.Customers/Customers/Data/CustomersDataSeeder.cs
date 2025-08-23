using BuildingBlocks.Abstractions.Persistence;

namespace NextGen.Modules.Customers.Customers.Data;

public class CustomersDataSeeder : IDataSeeder
{
    public Task SeedAllAsync()
    {
        return Task.CompletedTask;
    }
}
