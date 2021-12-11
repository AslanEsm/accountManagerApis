using System.ComponentModel.DataAnnotations;

namespace ViewModels.Account
{
    public class SignInViewModel : SignInBaseViewModel
    {
        [Display(Name = "مرا به خاطر بسپار؟")]
        public bool RememberMe { get; set; }

    }
}