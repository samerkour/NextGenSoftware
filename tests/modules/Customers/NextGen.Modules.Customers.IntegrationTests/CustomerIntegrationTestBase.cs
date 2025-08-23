using NextGen.Api;
using NextGen.Modules.Customers.Shared.Data;
using Tests.Shared.Fixtures;
using Xunit.Abstractions;

namespace NextGen.Modules.Customers.IntegrationTests;

public class CustomerModuleTestIntegrationTestBase : ModuleTestBase<Program, CustomersModuleConfiguration, CustomersDbContext, CustomersReadDbContext>
{
    public CustomerModuleTestIntegrationTestBase(IntegrationTestFixture<Program> integrationTestFixture,
        ITestOutputHelper outputHelper) : base(integrationTestFixture, outputHelper)
    {
    }
}
