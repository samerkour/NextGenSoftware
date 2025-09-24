using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Identity.Features.SendTOTP.Events.Integration;
using NextGen.Modules.Identity.Shared.Enum;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP;
internal class SendTotpHandler : ICommandHandler<SendTotpCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBus _bus;

    public SendTotpHandler(UserManager<ApplicationUser> userManager, IBus bus)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _bus = bus;
    }

    public async Task<Unit> Handle(SendTotpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new UserNotFoundException(request.UserId);

        // Generate TOTP (could be via authenticator or a custom token provider)
        var provider = request.DeliveryChannel == DeliveryChannel.Email
             ? TokenOptions.DefaultEmailProvider
             : TokenOptions.DefaultPhoneProvider;

        var totpToken = await _userManager.GenerateTwoFactorTokenAsync(user, provider);

        if (string.IsNullOrWhiteSpace(totpToken))
            throw new SendTotpException("Failed to generate TOTP token.");

        // Publish event to actually deliver via SMS/Email service
        await _bus.PublishAsync(
            new UserTotpSent(user.Id, user.Email, user.PhoneNumber, request.DeliveryChannel.ToString(), totpToken),
            null,
            cancellationToken);

        return Unit.Value;
    }
}
