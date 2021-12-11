using ViewModels.User;

namespace ViewModels.Account
{
    public class SendVerifyCodeDto
    {
        public string SelectedProvider { get; set; }
        public bool RememberMe { get; set; }
        public string UserId { get; set; }

    }
}