using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.AspNetCore.Identity;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.ProfilePicture;

internal class UploadProfilePictureHandler : ICommandHandler<UploadProfilePictureCommand, UploadProfilePictureResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName = "profile-pictures";

    public UploadProfilePictureHandler(UserManager<ApplicationUser> userManager, IAmazonS3 s3Client)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _s3Client = Guard.Against.Null(s3Client, nameof(s3Client));
    }

    public async Task<UploadProfilePictureResponse> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.File, nameof(request.File));

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with Id {request.UserId} not found.");

        // Generate unique file name
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";

        // Ensure bucket exists (create if missing)
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName))
        {
            await _s3Client.PutBucketAsync(
                new PutBucketRequest
            {
                BucketName = _bucketName,
                UseClientRegion = true
            },
                cancellationToken);
        }

        // Upload file stream to MinIO
        await using var stream = request.File.OpenReadStream();
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = stream,
            ContentType = request.File.ContentType,
            AutoCloseStream = true
        };

        await _s3Client.PutObjectAsync(putRequest, cancellationToken);

        // Save the S3 URL in user profile
        var fileUrl = $"http://localhost:9000/{_bucketName}/{fileName}";
        user.ProfileImagePath = fileUrl;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to update user's profile image.");

        return new UploadProfilePictureResponse(user.ProfileImagePath);
    }
}
