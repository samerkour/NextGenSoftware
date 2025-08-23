using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users.Features.GettingUerByEmail;

public record GetUserByEmailResponse(IdentityUserDto? UserIdentity);
