using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.User
{
    public class CreateUser
    {
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "تاریخ تولد")]
        public string BirthDate { get; set; }

        [Display(Name = "تصویر پروفایل")]
        public string Image { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public string CreateDate { get; set; }
        [Display(Name = "تاریخ ویرایش")]
        public string LastUpdate { get; set; }

        [Display(Name = "آخرین بازدید")]
        public string LastVisit { get; set; }

        [Display(Name = "فعال/غیر فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "سطح دسترسی")]
        public IEnumerable<string> Roles { get; set; }
    }
}
