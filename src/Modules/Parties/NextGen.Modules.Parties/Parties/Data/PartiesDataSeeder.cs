using BuildingBlocks.Abstractions.Persistence;

namespace NextGen.Modules.Parties.Parties.Data;

public class PartiesDataSeeder : IDataSeeder
{
    public Task SeedAllAsync()
    {
        return Task.CompletedTask;
    }
}
