using System.Web.Mvc;
using System.Web.Routing;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Web;

namespace Dashboard.Web.Routing {

    public class PagesRoute : RouteBase, IRouteWithArea {

        private readonly IPathResolver _pathResolver;
        private readonly IVirtualPathResolver _virtualPathResolver;
        private readonly IRouteHandler _routeHandler;

        private readonly Route _innerRoute;
        public const string ControllerKey = "controller";

        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        public string Area
        {
            get { return "Dashboard"; }
        }
        public static string ModelKey
        {
            get { return "model"; }
        }

        public static string ActionKey
        {
            get { return "action"; }
        }

        public static string DefaultAction
        {
            get { return "index"; }
        }
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public PagesRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver, Route innerRoute)
        /// <summary>
        /// Initializes a new instance of the <b>DashboardRoute</b> class.
        /// </summary>
        /// <param name="pathResolver"></param>
        /// <param name="virtualPathResolver"></param>
        /// <param name="innerRoute"></param>
        public PagesRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver, Route innerRoute)
        {
            _pathResolver = pathResolver;
            _virtualPathResolver = virtualPathResolver;
            _routeHandler = _routeHandler ?? new MvcRouteHandler();
            _innerRoute = innerRoute ?? new Route("dashboard/{controller}/{action}",
                                                        new RouteValueDictionary(new { controller = "pages", action = "index" }),
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary( new { area ="Dashboard" } ),
                                                        new MvcRouteHandler());
        }
        #endregion
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        #region public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        {
            // get the virtual path of the request
            var virtualPath = httpContext.Request.CurrentExecutionFilePath;
            
            // abort if the virtual path does not contain dashboard
            if (!IsDashboardRoute(virtualPath))
            {
                return null;
            }

            // try to resolve the current item
            var pathData = _pathResolver.ResolvePath(virtualPath.Replace("dashboard/pages/", "").Trim(new[] { '/' }));

            var routeData = new RouteData(this, _routeHandler);

            foreach (var defaultPair in _innerRoute.Defaults)
                routeData.Values[defaultPair.Key] = defaultPair.Value;
            foreach (var tokenPair in _innerRoute.DataTokens)
                routeData.DataTokens[tokenPair.Key] = tokenPair.Value;

            // Abort and proceed to other routes in the route table
            if (pathData == null) {
                return null;
            }

            routeData.ApplyCurrentModel("pages", pathData.Action, pathData.CurrentPageModel);

            return routeData;
        }
        #endregion
        #region public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            var vpd = _innerRoute.GetVirtualPath(requestContext, values);

            if (vpd == null)
                return null;

            vpd.Route = this;

            var item = values[ModelKey] as IPageModel;

            if (item == null)
                return null;

            vpd.VirtualPath = string.Format("dashboard/pages/{0}", _virtualPathResolver.ResolveVirtualPath(item, values));

            return vpd;
        }
        #endregion
        #region private bool IsDashboardRoute(string virtualPath)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns>True if dashboard is route, otherwise false.</returns>
        private static bool IsDashboardRoute(string virtualPath)
        {
            return virtualPath.StartsWith("/dashboard/pages");
        }
        #endregion
    }
}