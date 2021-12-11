using System.Collections;
using System.Collections.Generic;
using Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace Entities.Role
{
    public class Role : IdentityRole<long>, IEntity
    {
        public string Description { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}