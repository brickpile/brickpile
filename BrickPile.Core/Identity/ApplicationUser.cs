using RavenDB.AspNet.Identity;

namespace BrickPile.Core.Identity
{
    /// <summary>
    ///     Represents a common BrickPile user
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public string Email { get; set; }
    }
}