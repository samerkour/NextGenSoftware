using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Parties.RestockSubscriptions.Exceptions.Domain;
using NextGen.Modules.Parties.RestockSubscriptions.Features.SendingRestockNotification;
using NextGen.Modules.Parties.Shared.Data;
using FluentValidation;

namespace NextGen.Modules.Parties.RestockSubscriptions.Features.ProcessingRestockNotification;

public record ProcessRestockNotification(long ProductId, int CurrentStock) : ITxCommand;

internal class ProcessRestockNotificationValidator : AbstractValidator<ProcessRestockNotification>
{
    public ProcessRestockNotificationValidator()
    {
        RuleFor(x => x.CurrentStock)
            .NotEmpty();

        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}

internal class ProcessRestockNotificationHandler : ICommandHandler<ProcessRestockNotification>
{
    private readonly PartiesDbContext _partiesDbContext;
    private readonly ICommandProcessor _commandProcessor;
    private readonly ILogger<ProcessRestockNotificationHandler> _logger;

    public ProcessRestockNotificationHandler(
        PartiesDbContext partiesDbContext,
        ICommandProcessor commandProcessor,
        ILogger<ProcessRestockNotificationHandler> logger)
    {
        _partiesDbContext = partiesDbContext;
        _commandProcessor = commandProcessor;
        _logger = logger;
    }

    public async Task<Unit> Handle(ProcessRestockNotification command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, new RestockSubscriptionDomainException("Command cannot be null"));

        var subscribedParties =
            _partiesDbContext.RestockSubscriptions.Where(x =>
                x.ProductInformation.Id == command.ProductId && !x.Processed);

        if (!subscribedParties.Any())
            return Unit.Value;

        foreach (var restockSubscription in subscribedParties)
        {
            restockSubscription!.MarkAsProcessed(DateTime.Now);

            // https://github.com/kgrzybek/modular-monolith-with-ddd#38-internal-processing
            // schedule `SendRestockNotification` for running as a internal command after commenting transaction
            await _commandProcessor.ScheduleAsync(
                new SendRestockNotification(restockSubscription.Id, command.CurrentStock),
                cancellationToken);
        }

        await _partiesDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Marked restock subscriptions as processed");

        return Unit.Value;
    }
}
