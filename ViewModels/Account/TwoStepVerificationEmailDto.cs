using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Account
{
    public class TwoStepVerificationEmailDto
    {
        public Entities.User.User User { get; set; }
        public string Token { get; set; }
    }
}
