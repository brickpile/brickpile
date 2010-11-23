using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ContentRoute : RouteBase {
        private readonly IPathResolver _pathResolver;
        private readonly IVirtualPathResolver _virtualPathResolver;
        private readonly IRouteHandler _routeHandler;

        private readonly Route _innerRoute;
        public const string ControllerKey = "controller";
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        #region public static string ContentItemKey
        /// <summary>
        /// Gets the ContentItemKey of the ContentRoute
        /// </summary>
        /// <value></value>
        public static string ContentItemKey
        {
            get { return "item"; }
        }
        #endregion
        #region public static string ActionKey
        /// <summary>
        /// Gets the ActionKey of the ContentRoute
        /// </summary>
        /// <value></value>
        public static string ActionKey
        {
            get { return "action"; }
        }
        #endregion
        #region public static string DefaultAction
        /// <summary>
        /// Gets the DefaultAction of the ContentRoute
        /// </summary>
        /// <value></value>
        public static string DefaultAction
        {
            get { return "index"; }
        }
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public ContentRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver)
        /// <summary>
        /// Initializes a new instance of the <b>ContentRoute</b> class.
        /// </summary>
        /// <param name="pathResolver"></param>
        /// <param name="virtualPathResolver"></param>
        public ContentRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver) : this(pathResolver, virtualPathResolver, null) { }
        #endregion
        #region public ContentRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver, Route innerRoute)
        /// <summary>
        /// Initializes a new instance of the <b>ContentRoute</b> class.
        /// </summary>
        /// <param name="pathResolver"></param>
        /// <param name="virtualPathResolver"></param>
        /// <param name="innerRoute"></param>
        public ContentRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver, Route innerRoute)
        {
            _pathResolver = pathResolver;
            _virtualPathResolver = virtualPathResolver;
            _routeHandler = _routeHandler ?? new MvcRouteHandler();
            _innerRoute = innerRoute ?? new Route("{controller}/{action}",
                                                        new RouteValueDictionary(new { action = "Index" }),
                                                        new RouteValueDictionary(),
                                                        new RouteValueDictionary(),
                                                        _routeHandler);
        }
        #endregion
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        #region public override RouteData GetRouteData(HttpContextBase httpContextBase)
        /// <summary>
        /// Responsible for unlocking the route data
        /// </summary>
        /// <param name="httpContextBase"></param>
        /// <returns>Route data for current request containing the controller name, action and</returns>
        public override RouteData GetRouteData(HttpContextBase httpContextBase)
        {
            var routeData = new RouteData(this, _routeHandler);

            foreach (var defaultPair in _innerRoute.Defaults)
                routeData.Values[defaultPair.Key] = defaultPair.Value;
            foreach (var tokenPair in _innerRoute.DataTokens)
                routeData.DataTokens[tokenPair.Key] = tokenPair.Value;

            // get the virtual path of the request
            var virtualPath = httpContextBase.Request.CurrentExecutionFilePath;

            // try to resolve the current item
            var pathData = _pathResolver.ResolvePath(virtualPath.Trim(new[] { '/' }));

            // Abort and proceed to other routes in the route table
            if (pathData == null)
            {
                return null;
            }

            routeData.ApplyCurrentItem(pathData.Controller, pathData.Action, pathData.CurrentItem);

            return routeData;
        }
        #endregion
        #region public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        /// <summary>
        /// Responsible for resolving the virtual path to every item
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns>VirtualPathData for each and every item</returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var vpd = _innerRoute.GetVirtualPath(requestContext, values);

            if (vpd == null)
                return null;

            vpd.Route = this;

            var item = values[ContentItemKey] as IContentItem;


            vpd.VirtualPath = _virtualPathResolver.ResolveVirtualPath(item, values);

            return vpd;
        }
        #endregion
    }
}