using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Mvc;
using BrickPile.Domain;
using Raven.Client;

namespace BrickPile.Core.Routing
{
    internal class RouteResolver : IRouteResolver {

        private readonly IControllerMapper _controllerMapper;
        private readonly Func<IDocumentStore> _store;
        private IPage _currentPage;
        private StructureInfo _structureInfo;

        /// <summary>
        /// Resolves the route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Missing ContentType attribute</exception>
        public RouteData ResolveRoute(RouteBase route, HttpContextBase httpContext, string virtualPath)
        {
            _currentPage = null;

            // Set the default action to index
            var action = PageRoute.DefaultAction;

            using (var session = _store().OpenSession()) {

                // This can be cached
                _structureInfo = session.Load<StructureInfo>(DefaultBrickPileBootstrapper.StructureInfoDocumentId);
            
                if (_structureInfo == null || _structureInfo.RootNode == null) return null;

                StructureInfo.Node currentNode;

                var segments = virtualPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

                // The requested url is for the start page with no action
                // so just return the start page
                if(!segments.Any())
                {
                    currentNode = _structureInfo.RootNode;
                }
                else {
                    var nodes = _structureInfo.RootNode.Flatten(node => node.Children).ToList();

                    // The normal behaviour is to load the page based on the incoming url
                    currentNode = nodes.SingleOrDefault(n => n.Url == string.Join("/", segments));

                    // Try to find the node without the last segment of the url and set the last segment as action
                    if(currentNode == null)
                    {
                        action = segments.Last();
                        virtualPath = string.Join("/", segments, 0, (segments.Length - 1));
                        currentNode = nodes.SingleOrDefault(n => n.Url == (string.IsNullOrEmpty(virtualPath) ? null : virtualPath));                    
                    }
                }

                if (currentNode == null) {
                    return null;
                }

                var id = currentNode.PageId;

                if (string.IsNullOrEmpty(id)) {
                    return null;
                }

                // Load page with id n, if draft exist, load that to
                var pages = session.Advanced.LoadStartingWith<IPage>(id).ToList();

                // If the user is authenticated and we have recieved a draft, send it att current page
                if (httpContext.User.Identity.IsAuthenticated && pages.Any(page => page.Id == id + "/draft")) {
                    _currentPage = pages.SingleOrDefault(page => page.Id == id + "/draft");
                }
                else {
                    _currentPage = pages.SingleOrDefault(page => page.Id == id);
                }

                if (_currentPage == null)
                {
                    return null;
                }

                var contentTypeAttribute = _currentPage.GetType().GetAttribute<ContentTypeAttribute>();

                if (contentTypeAttribute == null)
                {
                    throw new NullReferenceException("Missing ContentType attribute");
                }

                var controllerName = contentTypeAttribute.ControllerType == null ?
                        _currentPage.GetType().Name :
                        _controllerMapper.GetControllerName(contentTypeAttribute.ControllerType);

                if (!(route is UIRoute) && !_controllerMapper.ControllerHasAction(controllerName, action))
                {
                    return null;
                }
            
                var routeData = new RouteData(route, new MvcRouteHandler());

                routeData.Values[PageRoute.ControllerKey] = controllerName;
                routeData.Values[PageRoute.ActionKey] = action;
                routeData.Values[PageRoute.CurrentPageKey] = _currentPage;

                routeData.DataTokens["structureInfo"] = _structureInfo;
                
                return routeData;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteResolver" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="controllerMapper">The controller mapper.</param>
        public RouteResolver(Func<IDocumentStore> store, IControllerMapper controllerMapper)
        {
            _controllerMapper = controllerMapper;
            _store = store;
        }
    }
}
