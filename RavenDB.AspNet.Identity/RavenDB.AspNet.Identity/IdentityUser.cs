using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace RavenDB.AspNet.Identity
{
    public class IdentityUser : IUser
    {
        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool IsEmailConfirmed { get; set; }
        public virtual bool IsPhoneNumberConfirmed { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTimeOffset LockoutEndDate { get; set; }
        public virtual bool TwoFactorAuthEnabled { get; set; }
        public virtual List<string> Roles { get; private set; }
        public virtual List<IdentityUserClaim> Claims { get; private set; }
        public virtual List<UserLoginInfo> Logins { get; private set; }

        public IdentityUser()
        {
            this.Claims = new List<IdentityUserClaim>();
            this.Roles = new List<string>();
            this.Logins = new List<UserLoginInfo>();
        }

        public IdentityUser(string userId, string userName)
            : this()
        {
            this.Id = userId;
            this.UserName = userName;
        }
    }

    public sealed class IdentityUserLogin
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Provider { get; set; }
        public string ProviderKey { get; set; }
    }

    public class IdentityUserClaim
    {
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }

    public sealed class IdentityUserByUserName
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public IdentityUserByUserName(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
