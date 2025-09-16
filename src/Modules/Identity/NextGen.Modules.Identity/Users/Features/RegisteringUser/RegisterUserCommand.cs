using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Users.Features.RegisteringUser;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword,
    List<string>? Roles = null) : ITxCreateCommand<RegisterUserResponse>
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public Guid Id { get; init; } = Guid.NewGuid();
}
