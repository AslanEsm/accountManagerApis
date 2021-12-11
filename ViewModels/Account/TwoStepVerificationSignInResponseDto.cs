using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Token;
using ViewModels.User;

namespace ViewModels.Account
{
    public class TwoStepVerificationSignInResponseDto
    {
        public UserViewModel User { get; set; }
        public UserToken Token { get; set; }
    }
}
