using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Amazon.SecurityToken.SAML;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Persistence;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using NextGen.Modules.Identity.Shared.Models;
using static System.Net.Mime.MediaTypeNames;

namespace NextGen.Modules.Identity.Identity.Features.GenerateCaptcha;

internal class GenerateCaptchaHandler : ICommandHandler<GenerateCaptchaCommand, GenerateCaptchaResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    private readonly ILogger<GenerateCaptchaHandler> _logger;
    private readonly IQueryProcessor _queryProcessor;
    private readonly IMemoryCache _cache;

    public GenerateCaptchaHandler(
            UserManager<ApplicationUser> userManager,
            ICommandProcessor commandProcessor,
            IQueryProcessor queryProcessor,
            IMemoryCache cache,
            ILogger<GenerateCaptchaHandler> logger)
    {
        _commandProcessor = commandProcessor;
        _queryProcessor = queryProcessor;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GenerateCaptchaResponse> Handle(GenerateCaptchaCommand reqest, CancellationToken cancellationToken)
    {
        // Generate random CAPTCHA code
        Guid captchaId = Guid.NewGuid();
        var captchaCode = GenerateRandomCaptchaCode(4);

        // Store the CAPTCHA code in cache with size specified
        _cache.Set(captchaCode.ToLower(), captchaCode.ToLower(), new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            Size = 1 // Specify cache entry size
        });

        // Generate CAPTCHA image
        using (var bitmap = new Bitmap(200, 80))
        using (var graphics = Graphics.FromImage(bitmap))
        {
            var random = new Random();
            graphics.Clear(Color.White);
            var font = new System.Drawing.Font("Arial", 40, FontStyle.Bold);
            var brush = new SolidBrush(Color.Black);

            graphics.DrawString(captchaCode, font, brush, 10, 10);

            // Optionally, add noise to the image (lines, dots)
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(bitmap.Width);
                int y1 = random.Next(bitmap.Height);
                int x2 = random.Next(bitmap.Width);
                int y2 = random.Next(bitmap.Height);
                graphics.DrawLine(Pens.Black, x1, y1, x2, y2);
            }

            // Convert image to base64 string
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                var captchaBase64 = Convert.ToBase64String(memoryStream.ToArray());
                return new GenerateCaptchaResponse(captchaId, captchaBase64);
            }
        }
    }

    private string GenerateRandomCaptchaCode(int length = 6)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
