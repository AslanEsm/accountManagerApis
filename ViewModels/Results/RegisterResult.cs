using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Results
{
    public class RegisterResult
    {
        public Result Result { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<IdentityError> ErrorList { get; set; }

        public RegisterResult(Result result, IEnumerable<IdentityError> errorList = null)
        {
            Result = result;
            ErrorList = errorList;
        }
    }

    public enum Result
    {
        [Display(Name = "نام کاربری تکراری است")]
        DuplicateUserName,

        [Display(Name = "ایمیل تکراری است")]
        DuplicateEmail,

        [Display(Name = "ثبت نام با موفقیت انجام شد")]
        Success,

        [Display(Name = "خطایی اتفاق افتاد")]
        ServerError,

        [Display(Name = "خطای هنگام افزودن کاربر به نقش ها رخ داده است")]
        AddToRoleOperation
    }
}