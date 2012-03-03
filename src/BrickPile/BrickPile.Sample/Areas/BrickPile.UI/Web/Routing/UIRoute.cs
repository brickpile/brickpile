using System.Web.Mvc;
using System.Web.Routing;

namespace BrickPile.UI.Web.Routing {
    public class UIRoute : Route, IRouteWithArea {
        /// <summary>
        /// Gets the name of the area to associate the route with.
        /// </summary>
        /// <returns>The name of the area to associate the route with.</returns>
        public string Area {
            get { return "UI"; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler) {}
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler) {}
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler) {}
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. These values are passed to the route handler, where they can be used for processing the request.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public UIRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler) {}

    }
}