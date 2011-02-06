using System.Web.Routing;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Extensions {
    /// <summary>
    /// Extension methods for RouteData objects
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class RouteDataExtensions {
        /// <summary>
        /// Used for adding a page model to the RouteData object's DataTokens
        /// </summary>
        /// <param name="data"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static RouteData ApplyCurrentPageModel(this RouteData data, string controllerName, string actionName, dynamic model)
        {
            data.Values[PageRoute.ControllerKey] = controllerName;
            data.Values[PageRoute.ActionKey] = actionName;
            data.Values[PageRoute.DocumentKey] = model;
            return data;
        }
    }
}