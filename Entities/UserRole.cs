using Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class UserRole : IdentityUserRole<long>, IEntity
    {
        public virtual Role.Role Role { get; set; }

        public virtual User.User User { get; set; }
    }
}