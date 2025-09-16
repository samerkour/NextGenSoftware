using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.ProfilePicture;
public class UploadProfilePictureCommandValidator : AbstractValidator<UploadProfilePictureCommand>
{
    private readonly long _maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
    private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png" };

    public UploadProfilePictureCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .Must(f => f!.Length > 0)
            .WithMessage("File is empty.")
            .Must(f => f!.Length <= _maxFileSizeInBytes)
            .WithMessage($"File size must be {_maxFileSizeInBytes / (1024 * 1024)} MB or less.")
            .Must(f => _permittedExtensions.Contains(Path.GetExtension(f!.FileName).ToLower()))
            .WithMessage($"Only {string.Join(", ", _permittedExtensions)} files are allowed.");
    }
}
