using NextGen.Api;
using NextGen.Modules.Parties.Shared.Data;
using Tests.Shared.Fixtures;
using Xunit.Abstractions;

namespace NextGen.Modules.Parties.IntegrationTests;

public class PartyModuleTestIntegrationTestBase : ModuleTestBase<Program, PartiesModuleConfiguration, PartiesDbContext, PartiesReadDbContext>
{
    public PartyModuleTestIntegrationTestBase(IntegrationTestFixture<Program> integrationTestFixture,
        ITestOutputHelper outputHelper) : base(integrationTestFixture, outputHelper)
    {
    }
}
