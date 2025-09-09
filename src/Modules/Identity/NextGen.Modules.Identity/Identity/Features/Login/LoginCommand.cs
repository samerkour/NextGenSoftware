using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Persistence;
using FirebirdSql.Data.Services;
using NextGen.Modules.Identity.Identity.Exceptions;
using NextGen.Modules.Identity.Shared.Exceptions;
using SendGrid.Helpers.Mail;
using Spectre.Console;
using static IdentityModel.OidcConstants;

namespace NextGen.Modules.Identity.Identity.Features.Login;

public record LoginCommand(string Captcha, string UserNameOrEmail, string Password, bool Remember) :
    ICommand<LoginResponse>, ITxRequest;
