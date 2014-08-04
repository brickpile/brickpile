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
    internal class PageRoute : RouteBase
    {
        public const string ControllerKey = "controller";

        protected VirtualPathResolver VirtualPathResolver { get; private set; }
        protected IRouteResolver RouteResolver { get; private set; }
        protected Func<IDocumentStore> DocumentStore { get; private set; }
        protected IControllerMapper ControllerMapper { get; private set; }
        protected StructureInfo StructureInfo { get; set; }

        public static string CurrentPageKey {
            get { return "currentPage"; }
        }

        public static string ActionKey {
            get { return "action"; }
        }

        public static string DefaultAction {
            get { return "index"; }
        }

        public PageRoute(VirtualPathResolver virtualPathResolver, IRouteResolver routeResolver,
            Func<IDocumentStore> documentStore, IControllerMapper controllerMapper) {
            if (virtualPathResolver == null)
            {
                throw new ArgumentNullException("virtualPathResolver");
            }

            if (routeResolver == null)
            {
                throw new ArgumentNullException("routeResolver");
            }

            if (documentStore == null)
            {
                throw new ArgumentNullException("documentStore");
            }

            if (controllerMapper == null)
            {
                throw new ArgumentNullException("controllerMapper");
            }

            this.VirtualPathResolver = virtualPathResolver;
            this.RouteResolver = routeResolver;
            this.DocumentStore = documentStore;
            this.ControllerMapper = controllerMapper;
        }

        /// <summary>
        ///     When overridden in a derived class, returns route information about the request.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <returns>
        ///     An object that contains the values from the route definition if the route matches the current request, or null if
        ///     the route does not match the request.
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext) {
            // Abort and proceed to other routes in the route table if path contains api or ui
            string[] segments = httpContext.Request.Path.Split(new[] {'/'});
            if (segments.Any(segment => segment.Equals("api", StringComparison.OrdinalIgnoreCase) ||
                                        segment.Equals("ui", StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            using (IDocumentSession session = this.DocumentStore.Invoke().OpenSession())
            {
                this.StructureInfo = session.Load<StructureInfo>(DefaultBrickPileBootstrapper.StructureInfoDocumentId);
            }

            Tuple<StructureInfo.Node, string> nodeAndAction = this.RouteResolver.ResolveRoute(this.StructureInfo,
                httpContext.Request.Path);

            if (nodeAndAction == null)
            {
                return null;
            }

            NavigationContext navigationContext = this.PrepareNavigationContext(
                httpContext.Request.RequestContext,
                nodeAndAction);

            string controllerName = this.ResolveControllerName(navigationContext.CurrentPage);

            if (!this.ControllerMapper.ControllerHasAction(controllerName, nodeAndAction.Item2))
            {
                return null;
            }

            RouteData routeData = this.PrepareRouteData(
                this.StructureInfo,
                navigationContext,
                controllerName,
                nodeAndAction.Item2);

            return routeData;
        }

        protected NavigationContext PrepareNavigationContext(RequestContext requestContext,
            Tuple<StructureInfo.Node, string> nodeAndAction) {
            using (IDocumentSession session = this.DocumentStore.Invoke().OpenSession())
            {
                IPage[] pages =
                    session.Load<IPage>(this.StructureInfo.GetAncestorIdsFor(nodeAndAction.Item1.PageId, true));
                return new NavigationContext(requestContext)
                {
                    CurrentContext = pages,
                    CurrentPage = pages.SingleOrDefault(page => page.Id == nodeAndAction.Item1.PageId)
                };
            }
        }

        protected RouteData PrepareRouteData(StructureInfo structureInfo, NavigationContext navigationContext,
            string controllerName, string action) {
            var routeData = new RouteData(this, new MvcRouteHandler());
            routeData.ApplyStructureInfo(structureInfo);
            routeData.ApplyCurrentContext(navigationContext.CurrentContext);
            routeData.Values[ControllerKey] = controllerName;
            routeData.Values[ActionKey] = action;
            routeData.Values[CurrentPageKey] = navigationContext.CurrentPage;
            return routeData;
        }

        protected virtual string ResolveControllerName(IPage currentPage) {
            var contentTypeAttribute = currentPage.GetType().GetAttribute<ContentTypeAttribute>();

            if (contentTypeAttribute == null)
            {
                throw new NullReferenceException("Missing ContentType attribute");
            }

            return contentTypeAttribute.ControllerType == null
                ? currentPage.GetType().Name
                : this.ControllerMapper.GetControllerName(contentTypeAttribute.ControllerType);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            IPage model = values[CurrentPageKey] as IPage ?? requestContext.RouteData.Values[CurrentPageKey] as IPage;

            if (model == null)
            {
                return null;
            }

            var vpd = new VirtualPathData(this, this.VirtualPathResolver.ResolveVirtualPath(model, values));

            string queryParams = String.Empty;
            // add query string parameters
            foreach (var kvp in values)
            {
                if (kvp.Key.Equals(CurrentPageKey) || kvp.Key.Equals(ControllerKey) || kvp.Key.Equals(ActionKey))
                {
                    continue;
                }
                queryParams = queryParams.AddQueryParam(kvp.Key, kvp.Value.ToString());
            }
            vpd.VirtualPath += queryParams;
            return vpd;
        }
    }
}