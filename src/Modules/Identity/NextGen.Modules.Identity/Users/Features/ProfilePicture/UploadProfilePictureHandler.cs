using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Users.Features.ProfilePicture;

internal class UploadProfilePictureHandler : ICommandHandler<UploadProfilePictureCommand, UploadProfilePictureResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public UploadProfilePictureHandler(
        UserManager<ApplicationUser> userManager,
        IAmazonS3 s3Client,
        IConfiguration configuration)
    {
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
        _s3Client = Guard.Against.Null(s3Client, nameof(s3Client));
        _configuration = Guard.Against.Null(configuration, nameof(configuration));
    }

    public async Task<UploadProfilePictureResponse> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.File, nameof(request.File));

        // Generate unique file name
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";

        // Get bucket name from configuration
        string bucketName = _configuration["S3:BucketName"] ?? "public-files";

        // Ensure bucket exists (create if missing)
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName))
        {
            await _s3Client.PutBucketAsync(
                new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                },
                cancellationToken);
        }

        // Upload file stream to MinIO / S3
        await using var stream = request.File.OpenReadStream();
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = stream,
            ContentType = request.File.ContentType,
            AutoCloseStream = true
        };

        await _s3Client.PutObjectAsync(putRequest, cancellationToken);

        // Use ServiceURL from _s3Client instead of configuration
        var s3ClientImpl = _s3Client as AmazonS3Client
                           ?? throw new InvalidOperationException("Invalid S3 client type.");
        string baseUrl = s3ClientImpl.Config.ServiceURL ?? "http://localhost:9000/";

        // Save the S3 URL in user profile
        var fileUrl = $"{baseUrl}{bucketName}/{fileName}";

        return new UploadProfilePictureResponse(fileUrl);
    }
}
