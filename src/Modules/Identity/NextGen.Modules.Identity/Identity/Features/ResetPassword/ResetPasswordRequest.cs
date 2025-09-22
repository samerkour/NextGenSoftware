using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Identity.Features.ResetPassword;

public record ResetPasswordRequest(
    string ResetToken,
    string NewPassword,
    string ConfirmNewPassword
);
