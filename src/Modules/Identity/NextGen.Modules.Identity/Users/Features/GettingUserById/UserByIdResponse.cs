using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users.Features.GettingUserById;

// Flatten the response
internal record UserByIdResponse : IdentityUserDto
{
    public UserByIdResponse(IdentityUserDto identityUserDto) : base(identityUserDto) { }
}
