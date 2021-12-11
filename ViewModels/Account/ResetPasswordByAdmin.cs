using System.ComponentModel.DataAnnotations;

namespace ViewModels.Account
{
    public class ResetPasswordByAdmin
    {
        public string Id { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string NewPassword { get; set; }
    }
}