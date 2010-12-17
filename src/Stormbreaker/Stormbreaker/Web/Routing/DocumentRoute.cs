using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Stormbreaker.Extensions;
using Stormbreaker.Models;

namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// Responsible for mapping all routes to the ContentRoute
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class DocumentRoute : RouteBase {

        private readonly IPathResolver _pathResolver;
        private readonly IVirtualPathResolver _virtualPathResolver;
        private readonly IRouteHandler _routeHandler;

        private readonly Route _innerRoute;
        public const string ControllerKey = "controller";

        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        public static string DocumentKey
        {
            get { return "document"; }
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
        public DocumentRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver) : this(pathResolver, virtualPathResolver, null) { }
        public DocumentRoute(IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver, Route innerRoute)
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
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        public override RouteData GetRouteData(HttpContextBase httpContextBase)
        {

            var routeData = new RouteData(this, _routeHandler);

            foreach (var defaultPair in _innerRoute.Defaults)
                routeData.Values[defaultPair.Key] = defaultPair.Value;
            foreach (var tokenPair in _innerRoute.DataTokens)
                routeData.DataTokens[tokenPair.Key] = tokenPair.Value;

            // get the virtual path of the request
            var virtualPath = httpContextBase.Request.CurrentExecutionFilePath.Trim(new[] {'/'} );
            if(string.IsNullOrEmpty(virtualPath)) {
                return null;
            }

            // try to resolve the current item
            var pathData = _pathResolver.ResolvePath(virtualPath);

            // Abort and proceed to other routes in the route table
            if (pathData == null) {
                return null;
            }

            routeData.ApplyCurrentDocument(pathData.Controller, pathData.Action, pathData.CurrentDocument);

            return routeData;
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {

            var vpd = _innerRoute.GetVirtualPath(requestContext, values);

            if (vpd == null)
                return null;

            vpd.Route = this;

            var item = values[DocumentKey] as IDocument;


            vpd.VirtualPath = _virtualPathResolver.ResolveVirtualPath(item, values);

            return vpd;
        }
    }
}