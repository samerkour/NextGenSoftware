using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Parties.RestockSubscriptions.Exceptions.Application;
using NextGen.Modules.Parties.Shared.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.DeletingRestockSubscription;

public record DeleteRestockSubscription(long Id) : ITxCommand;

internal class DeleteRestockSubscriptionValidator : AbstractValidator<DeleteRestockSubscription>
{
    public DeleteRestockSubscriptionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal class DeleteRestockSubscriptionHandler : ICommandHandler<DeleteRestockSubscription>
{
    private readonly PartiesDbContext _partiesDbContext;
    private readonly ILogger<DeleteRestockSubscriptionHandler> _logger;

    public DeleteRestockSubscriptionHandler(
        PartiesDbContext partiesDbContext,
        ILogger<DeleteRestockSubscriptionHandler> logger)
    {
        _partiesDbContext = partiesDbContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteRestockSubscription command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var exists = await _partiesDbContext.RestockSubscriptions.IgnoreAutoIncludes()
            .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        Guard.Against.NotFound(exists, new RestockSubscriptionNotFoundException(command.Id));

        // for raising a deleted domain event
        exists!.Delete();

        _partiesDbContext.Entry(exists).State = EntityState.Deleted;
        _partiesDbContext.Entry(exists.ProductInformation).State = EntityState.Unchanged;

        await _partiesDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("RestockSubscription with id '{Id} removed.'", command.Id);

        return Unit.Value;
    }
}
