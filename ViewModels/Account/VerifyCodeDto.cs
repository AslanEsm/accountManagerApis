using System.ComponentModel.DataAnnotations;

namespace ViewModels.Account
{
    public class VerifyCodeDto
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Provider { get; set; }
        public string UserId { get; set; }
        [Display(Name = "کد اعتبار سنجی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Code { get; set; }
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberBrowser { get; set; }
    }
}