using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Domain.ValueObjects;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Core.IdsGenerator;
using NextGen.Modules.Parties.Parties.Exceptions.Application;
using NextGen.Modules.Parties.Parties.Models;
using NextGen.Modules.Parties.Shared.Clients.Identity;
using NextGen.Modules.Parties.Shared.Data;
using FluentValidation;

namespace NextGen.Modules.Parties.Parties.Features.CreatingParty;

public record CreateParty(string Email) : ITxCreateCommand<CreatePartyResponse>
{
    public long Id { get; init; } = SnowFlakIdGenerator.NewId();
}

internal class CreatePartyValidator : AbstractValidator<CreateParty>
{
    public CreatePartyValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email address is invalid.");
    }
}

public class CreatePartyHandler : ICommandHandler<CreateParty, CreatePartyResponse>
{
    private readonly IIdentityApiClient _identityApiClient;
    private readonly PartiesDbContext _partiesDbContext;
    private readonly ILogger<CreatePartyHandler> _logger;

    public CreatePartyHandler(
        IIdentityApiClient identityApiClient,
        PartiesDbContext partiesDbContext,
        ILogger<CreatePartyHandler> logger)
    {
        _identityApiClient = identityApiClient;
        _partiesDbContext = partiesDbContext;
        _logger = logger;
    }

    public async Task<CreatePartyResponse> Handle(CreateParty command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        if (_partiesDbContext.Parties.Any(x => x.Email == command.Email))
            throw new PartyAlreadyExistsException($"Party with email '{command.Email}' already exists.");

        var identityUser = (await _identityApiClient.GetUserByEmailAsync(command.Email, cancellationToken))
            ?.UserIdentity;

        Guard.Against.NotFound(
            identityUser,
            new PartyNotFoundException($"Identity user with email '{command.Email}' not found."));

        var party = Party.Create(
            command.Id,
            Email.Create(identityUser!.Email),
            PartyName.Create(identityUser.FirstName, identityUser.LastName),
            identityUser.Id);

        await _partiesDbContext.AddAsync(party, cancellationToken);

        await _partiesDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created a party with ID: '{@PartyId}'", party.Id);

        return new CreatePartyResponse(
            party.Id,
            party.Email!,
            party.Name.FirstName,
            party.Name.LastName,
            party.IdentityId);
    }
}
