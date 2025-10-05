namespace NextGen.Modules.Identity.Claims.Features.CreateClaim
{
        public class CreateDto
        {
            /// <summary>
            /// نوع Claim (مثلاً role یا permission)
            /// </summary>
            public string Type { get; set; } = default!;

            /// <summary>
            /// مقدار Claim (مثلاً "admin")
            /// </summary>
            public string Value { get; set; } = default!;

            /// <summary>
            /// شناسه گروه Claim
            /// </summary>
            public Guid ClaimGroupId { get; set; }
        }
    }
