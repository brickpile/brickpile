using System.Security.Claims;

namespace BrickPile.Core.Security.Claims
{
    /// <summary>
    ///     An <see cref="T:System.Security.Principal.IPrincipal" /> implementation that supports multiple claims-based
    ///     identities.
    /// </summary>
    public class AppUserPrincipal : ClaimsPrincipal
    {
        /// <summary>
        ///     Gets the name of a BrickPile user.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get { return FindFirst(ClaimTypes.Name).Value; }
        }

        /// <summary>
        ///     Gets the email of a BrickPile user.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public string Email
        {
            get { return FindFirst(ClaimTypes.Email).Value; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppUserPrincipal" /> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public AppUserPrincipal(ClaimsPrincipal principal)
            : base(principal) {}
    }
}