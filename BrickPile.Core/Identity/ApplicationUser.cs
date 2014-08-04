using RavenDB.AspNet.Identity;

namespace BrickPile.Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Email { get; set; }
    }
}