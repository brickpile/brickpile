using System.Security.Claims;
using System.Web.Mvc;
using BrickPile.Core.Security.Claims;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Represents the properties and methods that are needed in order to render a view that uses ASP.NET Razor syntax in
    ///     BrickPile UI.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        ///     Gets the current user.
        /// </summary>
        /// <value>
        ///     The current user.
        /// </value>
        protected AppUserPrincipal CurrentUser
        {
            get { return new AppUserPrincipal(User as ClaimsPrincipal); }
        }
    }

    /// <summary>
    ///     Represents the properties and methods that are needed in order to render a view that uses ASP.NET Razor syntax in
    ///     BrickPile UI.
    /// </summary>
    public abstract class AppViewPage : AppViewPage<dynamic> {}
}