using System.Collections.Generic;
using ViewModels.Token;
using ViewModels.User;

namespace ViewModels.Account
{
    public class SignInResponseDto
    {
        public UserViewModel User { get; set; }
        public UserToken Token { get; set; }
        public IEnumerable<string> Providers { get; set; }
        public bool RememberMe { get; set; }
        public bool Is2StepVerificationRequired { get; set; }

    }
}