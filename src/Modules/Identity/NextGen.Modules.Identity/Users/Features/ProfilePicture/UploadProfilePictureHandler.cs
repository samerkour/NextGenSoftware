using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.ProfilePicture;

internal class UploadProfilePictureHandler : ICommandHandler<UploadProfilePictureCommand, UploadProfilePictureResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public UploadProfilePictureHandler(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _env = Guard.Against.Null(env, nameof(env));
    }

    public async Task<UploadProfilePictureResponse> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.File, nameof(request.File));

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        // Generate a unique file name
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
        var uploadsDir = Path.Combine(_env.ContentRootPath, "uploads");

        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        var filePath = Path.Combine(uploadsDir, fileName);

        // Save the file
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        // Update user's ImagePath
        user.ProfileImagePath = $"/uploads/{fileName}";
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to update user's image.");

        return new UploadProfilePictureResponse(user.ProfileImagePath);
    }
}
