using System;
using System.Collections.Generic;
using Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace Entities.User
{
    public class User : IdentityUser<long>, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public DateTime? LastVisit { get; set; }
        public bool IsActive { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public User()
        {
            CreateDate = DateTime.Now;
            LastUpdate = DateTime.Now;
        }
    }
}