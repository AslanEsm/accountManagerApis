namespace ViewModels.Account
{
    public class RegisterWithEmailConfirmCodeDto
    {
        public string CallBackUrl { get; set; }
        public Entities.User.User User { get; set; }
    }
}