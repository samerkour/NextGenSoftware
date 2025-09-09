using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Identity.Features.GenerateCaptcha
{
    public record GenerateCaptchaResponse
    {
        public GenerateCaptchaResponse(Guid captchaId, string captchaImage)
        {
            Id = captchaId;
            ImageBase64 = captchaImage;
        }

        public Guid Id { get; }
        public string ImageBase64 { get; }
    }
}
