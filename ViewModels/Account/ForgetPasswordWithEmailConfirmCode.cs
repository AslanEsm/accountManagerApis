namespace ViewModels.Account
{
    public class ForgetPasswordWithEmailConfirmCode
    {
        public string CallBackUrl { get; set; }
        public Entities.User.User User { get; set; }
    }
}