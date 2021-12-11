using common.Utilities.Paging;
using System;

namespace ViewModels.User
{
    public class FilterUser : PaginationDto
    {
        public string Search { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        //=================================//
        public string FromBirthDate { get; set; }

        public string ToBirthDate { get; set; }
        public string FromRegisterDate { get; set; }
        public string ToRegisterDate { get; set; }
        public string FromUpdateTime { get; set; }
        public string ToUpdateTime { get; set; }
        public string FromLastVisitDate { get; set; }
        public string ToLastVisitDate { get; set; }

        //=================================//

        public bool IsActive { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool IsLockoutEnabled { get; set; }

        //===================SORTING=======================/
        public string SortBy { get; set; }
        public bool Reverse { get; set; }

        public FilterUser()
        {
            SortBy = "Id";
            Reverse = false;
        }
    }
}