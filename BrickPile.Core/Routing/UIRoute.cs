using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using BrickPile.Core.Mvc;
using Raven.Client;

namespace BrickPile.Core.Routing
{
    /// <summary>
    /// </summary>
    internal class UiRoute : PageRoute, IRouteWithArea
    {
        private const string ControllerName = "pages";

        public UiRoute(VirtualPathResolver virtualPathResolver, IRouteResolver routeResolver, Func<IDocumentStore> documentStore,
            IControllerMapper controllerMapper) : base(virtualPathResolver, routeResolver, documentStore, controllerMapper) {}

        public string Area {
            get { return "UI"; }
        }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            string[] segments = httpContext.Request.Path.Split(new[] {'/'});
            if (!segments.Any(segment => segment.Equals("ui", StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            using (IDocumentSession session = base.DocumentStore.Invoke().OpenSession())
            {
                base.StructureInfo = session.Load<StructureInfo>(DefaultBrickPileBootstrapper.StructureInfoDocumentId);
            }

            Tuple<StructureInfo.Node, string> nodeAndAction = base.RouteResolver.ResolveRoute(base.StructureInfo,
                httpContext.Request.Path.Replace("/ui/pages", ""));

            if (nodeAndAction == null)
            {
                return null;
            }

            NavigationContext navigationContext = base.PrepareNavigationContext(
                httpContext.Request.RequestContext,
                nodeAndAction);

            RouteData routeData = base.PrepareRouteData(
                base.StructureInfo,
                navigationContext,
                ControllerName,
                nodeAndAction.Item2);

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            IPage model = values[CurrentPageKey] as IPage;

            if (model == null)
            {
                return null;
            }

            var vpd = new VirtualPathData(this, this.VirtualPathResolver.ResolveVirtualPath(model, values));

            vpd.VirtualPath = string.Format("/ui/pages".TrimStart(new[] { '/' }) + "/{0}", vpd.VirtualPath);

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