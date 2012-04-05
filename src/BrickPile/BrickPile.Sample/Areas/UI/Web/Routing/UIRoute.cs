using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using StructureMap;

namespace BrickPile.UI.Web.Routing {
    /// <summary>
    /// 
    /// </summary>
    public class UIRoute : Route, IRouteWithArea {
        public const string ControllerKey = "controller";
        /// <summary>
        /// Gets the path resolver.
        /// </summary>
        protected IPathResolver PathResolver {
            get { return _pathResolver ?? (_pathResolver = ObjectFactory.GetInstance<IPathResolver>()); }
        }
        private IPathResolver _pathResolver;
        /// <summary>
        /// Gets the virtual path resolver.
        /// </summary>
        protected IVirtualPathResolver VirtualPathResolver {
            get { return _virtualPathResolver ?? (_virtualPathResolver = ObjectFactory.GetInstance<IVirtualPathResolver>()); }
        }
        private IVirtualPathResolver _virtualPathResolver;
        /// <summary>
        /// Gets the model key.
        /// </summary>
        public static string ModelKey {
            get { return "model"; }
        }
        /// <summary>
        /// Gets the action key.
        /// </summary>
        public static string ActionKey {
            get { return "action"; }
        }
        /// <summary>
        /// Gets the default action.
        /// </summary>
        public static string DefaultAction {
            get { return "index"; }
        }
        /// <summary>
        /// Gets the default controller.
        /// </summary>
        /// <value>
        /// The default name of the controller.
        /// </value>
        public static string DefaultControllerName {
            get { return "Pages"; }
        }
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
        public UIRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. These values are passed to the route handler, where they can be used for processing the request.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public UIRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler) { }
        /// <summary>
        /// Returns information about the requested route.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <returns>
        /// An object that contains the values from the route definition.
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext) {

            // get the virtual path of the request
            var virtualPath = httpContext.Request.CurrentExecutionFilePath;

            // exit with the base functionality
            if (!virtualPath.StartsWith("/pages", StringComparison.OrdinalIgnoreCase)) {
                return base.GetRouteData(httpContext);
            }

            var routeData = base.GetRouteData(httpContext);

            // try to resolve the current item
            var pathData = this.PathResolver.ResolvePath(routeData, virtualPath.Replace("/pages", string.Empty));

            //var routeData = new RouteData(this, this.RouteHandler);

            // Abort and proceed to other routes in the route table
            if (pathData == null) {
                return base.GetRouteData(httpContext);
            }

            routeData.ApplyCurrentModel(DefaultControllerName, pathData.Action, pathData.CurrentPageModel);

            return routeData;
        }
        /// <summary>
        /// Returns information about the URL that is associated with the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        /// An object that contains information about the URL that is associated with the route.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {

            var model = values[ModelKey] as IPageModel;

            if (model == null) {
                //return base.GetVirtualPath(requestContext, values);
                return null;
            }

            var vpd = new VirtualPathData(this, this.VirtualPathResolver.ResolveVirtualPath(model, values));

            //var vpd = base.GetVirtualPath(requestContext, values);

            //if (vpd == null)
            //    return null;

            vpd.Route = this;

            vpd.VirtualPath = string.Format("pages/{0}", VirtualPathResolver.ResolveVirtualPath(model, values));

            return vpd;
        }
    }
}