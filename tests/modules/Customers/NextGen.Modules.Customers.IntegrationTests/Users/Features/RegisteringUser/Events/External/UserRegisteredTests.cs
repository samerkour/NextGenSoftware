using Bogus;
using BuildingBlocks.Abstractions.Web.Module;
using NextGen.Modules.Parties.Parties.Features;
using NextGen.Modules.Parties.Parties.Features.CreatingParty.Events.Integration;
using NextGen.Modules.Parties.Shared.Clients.Identity;
using NextGen.Modules.Parties.Shared.Clients.Identity.Dtos;
using NextGen.Modules.Parties.Shared.Data;
using NextGen.Modules.Parties.Users.Features.RegisteringUser.Events.External;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Tests.Shared.Fixtures;
using Xunit.Abstractions;
using Program = NextGen.Api.Program;

namespace NextGen.Modules.Parties.IntegrationTests.Users.Features.RegisteringUser.Events.External;

public class
    UserRegisteredTests : ModuleTestBase<Program, PartiesModuleConfiguration, PartiesDbContext, PartiesReadDbContext>
{
    private static UserRegistered _userRegistered;

    public UserRegisteredTests(IntegrationTestFixture<Program> integrationTestFixture, ITestOutputHelper outputHelper) :
        base(integrationTestFixture, outputHelper)
    {
        _userRegistered = new Faker<UserRegistered>().CustomInstantiator(faker =>
                new UserRegistered(
                    Guid.NewGuid(),
                    faker.Person.Email,
                    faker.Person.UserName,
                    faker.Person.FirstName,
                    faker.Person.LastName, new List<string> {"user"}))
            .Generate();
    }

    protected override void RegisterModulesTestsServices(IServiceCollection services, IModuleDefinition module)
    {
        base.RegisterModulesTestsServices(services, module);

        if (module.GetType() == typeof(PartiesModuleConfiguration))
        {
            services.Replace(ServiceDescriptor.Transient<IIdentityApiClient>(x =>
            {
                var f = Substitute.For<IIdentityApiClient>();
                f.GetUserByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())!
                    .Returns(args =>
                    {
                        var email = args.Arg<string>();

                        return Task.FromResult(new GetUserByEmailResponse(new UserIdentityDto()
                        {
                            Email = _userRegistered.Email,
                            Id = _userRegistered.IdentityId,
                            FirstName = _userRegistered.FirstName,
                            LastName = _userRegistered.LastName,
                            UserName = _userRegistered.UserName
                        }));
                    });

                return f;
            }));
        }
    }

    [Fact]
    public async Task user_registered_message_should_consume_existing_consumer_by_broker()
    {
        // Arrange
        var shouldConsume = await ModuleFixture.ShouldConsume<UserRegistered>();

        // Act
        await ModuleFixture.PublishMessageAsync(_userRegistered, null, CancellationToken);

        // Assert
        await shouldConsume.Validate(60.Seconds());
    }

    [Fact]
    public async Task user_registered_message_should_consume_new_consumers_by_broker()
    {
        // Arrange
        var shouldConsume = await ModuleFixture.ShouldConsumeWithNewConsumer<UserRegistered>();

        // Act
        await ModuleFixture.PublishMessageAsync(_userRegistered, cancellationToken: CancellationToken);

        // Assert
        await shouldConsume.Validate(60.Seconds());
    }

    [Fact]
    public async Task user_registered_message_should_consume_by_user_registered_consumer()
    {
        var shouldConsume = await ModuleFixture.ShouldConsume<UserRegistered, UserRegisteredConsumer>();

        // Act
        await ModuleFixture.PublishMessageAsync(_userRegistered, cancellationToken: CancellationToken);

        // Assert
        await shouldConsume.Validate(60.Seconds());
    }

    [Fact]
    public async Task user_registered_message_should_create_new_party_in_sqlserver_write_db()
    {
        _userRegistered = new Faker<UserRegistered>().CustomInstantiator(faker =>
                new UserRegistered(
                    Guid.NewGuid(),
                    faker.Person.Email,
                    faker.Person.UserName,
                    faker.Person.FirstName,
                    faker.Person.LastName, new List<string> {"user"}))
            .Generate();

        // Act
        await ModuleFixture.PublishMessageAsync(_userRegistered, cancellationToken: CancellationToken);

        // Assert
        var shouldConsume = await ModuleFixture.ShouldConsume<UserRegistered, UserRegisteredConsumer>(x =>
            x.Email.ToLower() == _userRegistered.Email.ToLower());

        await shouldConsume.Validate(60.Seconds());

        var party = await ModuleFixture.ExecuteContextAsync(async ctx =>
        {
            var res = await ctx.Parties.AnyAsync(x => x.Email == _userRegistered.Email.ToLower());

            return res;
        });

        party.Should().BeTrue();
    }


    [Fact]
    public async Task user_registered_message_should_create_new_party_in_internal_persistence_message_and_mongo()
    {
        // Act
        await ModuleFixture.PublishMessageAsync(_userRegistered, cancellationToken: CancellationToken);

        // Assert
        await ModuleFixture.ShouldProcessedPersistInternalCommand<CreateMongoPartyReadModels>();

        var existsParty = await ModuleFixture.ExecuteReadContextAsync(async ctx =>
        {
            var res = await ctx.Parties.AsQueryable().AnyAsync(x => x.Email == _userRegistered.Email.ToLower());

            return res;
        });

        existsParty.Should().BeTrue();
    }

    [Fact]
    public async Task user_registered_message_should_create_party_created_in_the_outbox()
    {
        // Act
        await ModuleFixture.PublishMessageAsync(_userRegistered, cancellationToken: CancellationToken);

        await ModuleFixture.ShouldProcessedOutboxPersistMessage<PartyCreated>();
    }

    [Fact]
    public async Task user_registered_should_should_publish_party_created()
    {
        // Arrange
        var shouldPublish = await ModuleFixture.ShouldPublish<PartyCreated>();

        // Act
        await ModuleFixture.PublishMessageAsync(_userRegistered, cancellationToken: CancellationToken);

        // Assert
        await shouldPublish.Validate(60.Seconds());
    }
}
