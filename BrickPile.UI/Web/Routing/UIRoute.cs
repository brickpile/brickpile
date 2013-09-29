using System;
using System.Linq;
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
    [Obsolete("Removed",true)]
    public class UIRoute : Route, IRouteWithArea {
        private readonly string _url;
        private readonly RouteValueDictionary _defaults;
        private readonly RouteValueDictionary _constraints;
        private readonly RouteValueDictionary _dataTokens;
        private readonly IRouteHandler _routeHandler;
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
            get { return "pages"; }
        }
        /// <summary>
        /// Gets the name of the area to associate the route with.
        /// </summary>
        /// <returns>The name of the area to associate the route with.</returns>
        public string Area {
            get { return "ui"; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler) {
            _url = url;
            _routeHandler = routeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler) {
            _url = url;
            _defaults = defaults;
            _routeHandler = routeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaults">The defaults.</param>
        /// <param name="constraints">The constraints.</param>
        /// <param name="routeHandler">The route handler.</param>
        public UIRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler) {
            _url = url;
            _defaults = defaults;
            _constraints = constraints;
            _routeHandler = routeHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIRoute"/> class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. These values are passed to the route handler, where they can be used for processing the request.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public UIRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler) {
            _url = url;
            _defaults = defaults;
            _constraints = constraints;
            _dataTokens = dataTokens;
            _routeHandler = routeHandler;
        }

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

            var routeData = new RouteData(this, _routeHandler);
            
            foreach (var defaultPair in this._defaults)
                routeData.Values[defaultPair.Key] = defaultPair.Value;
            
            // try to resolve the current item
            var pathData = PathResolver.ResolvePath(routeData, virtualPath.Replace("/ui/pages", string.Empty));

            // Abort and proceed to other routes in the route table
            if (pathData == null) {
                return null;
            }

            routeData.ApplyCurrentPage(DefaultControllerName, pathData.Action, pathData.CurrentPage);
            //routeData.ApplyCurrentContent(pathData.CurrentContent);
            routeData.ApplyCurrentStructureInfo(new StructureInfo
            {
                NavigationContext = pathData.NavigationContext.OrderBy(x => x.Metadata.SortOrder),
                CurrentPage = pathData.CurrentPage,
                //CurrentContent = pathData.CurrentContent,
                StartPage = pathData.NavigationContext.Single(x => x.Parent == null),
                ParentPage = pathData.CurrentPage.Parent != null ? pathData.NavigationContext.SingleOrDefault(x => x.Id == pathData.CurrentPage.Parent.Id) : null
            });
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

            var model = values[ModelKey] as IPage;

            if (model == null) {
                VirtualPathData path = base.GetVirtualPath(requestContext, values);

                if (path != null && path.VirtualPath != "")
                    path.VirtualPath = path.VirtualPath + "/";
                return path;                
            }

            var vpd = new VirtualPathData(this, this.VirtualPathResolver.ResolveVirtualPath(model, values));

            vpd.Route = this;

            vpd.VirtualPath = string.Format("/ui/pages".TrimStart(new[] { '/' }) + "/{0}", VirtualPathResolver.ResolveVirtualPath(model, values));

            var queryParams = String.Empty;
            // add query string parameters
            foreach (var kvp in values) {
                if (kvp.Key.Equals(ModelKey) || kvp.Key.Equals(ControllerKey) || kvp.Key.Equals(ActionKey)) {
                    continue;
                }
                queryParams = queryParams.AddQueryParam(kvp.Key, kvp.Value.ToString());
            }
            vpd.VirtualPath += queryParams;

            return vpd;
        }
    }
}