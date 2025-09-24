using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Enum;
using NextGen.Modules.Identity.Shared.Exceptions;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Features.VerifyTotp;
internal class VerifyTotpHandler : ICommandHandler<VerifyTotpCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public VerifyTotpHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<Unit> Handle(VerifyTotpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        Guard.Against.Null(user, new UserNotFoundException(request.UserId));

        var provider = request.DeliveryChannel == DeliveryChannel.Email
            ? TokenOptions.DefaultEmailProvider
            : TokenOptions.DefaultPhoneProvider;

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, provider, request.Code);
        if (!isValid)
        {
            throw new InvalidTotpException(request.UserId, request.DeliveryChannel);
        }

        return Unit.Value;
    }
}
