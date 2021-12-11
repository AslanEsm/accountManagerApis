using System.ComponentModel.DataAnnotations;

namespace ViewModels.Role
{
    public class RoleViewModel : EditRole
    {
        [Display(Name = "کاربران")]
        public int UsersCount { get; set; }
    }
}