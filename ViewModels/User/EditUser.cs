using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ViewModels.User
{
    public class EditUser
    {
        public long Id { get; set; }

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

        [Display(Name = "تاریخ ویرایش")]
        public string LastUpdate { get; set; }

        [Display(Name = "آخرین بازدید")]
        public string LastVisit { get; set; }

        [Display(Name = "فعال/غیر فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "سطح دسترسی")]
        public IEnumerable<string> Roles { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string LockoutEnd { get; set; }
    }
}
