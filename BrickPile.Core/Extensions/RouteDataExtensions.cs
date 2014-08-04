using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using BrickPile.Core.Routing;

namespace BrickPile.Core.Extensions
{
    public static class RouteDataExtensions
    {
        public static T GetCurrentPage<T>(this RouteData data)
        {
            return (T)data.Values[PageRoute.CurrentPageKey];
        }

        public static void ApplyStructureInfo(this RouteData routeData, StructureInfo structureInfo) {
            routeData.DataTokens["brickpile:structureInfo"] = structureInfo;
        }

        public static void ApplyCurrentContext(this RouteData routeData, IEnumerable<IPage> pages)
        {
            routeData.DataTokens["brickpile:currentcontext"] = pages;
        }

        public static IEnumerable<IPage> GetCurrentContext(this RouteData routeData)
        {
            return (IEnumerable<IPage>)routeData.DataTokens["brickpile:currentcontext"];
        }

        public static NavigationContext GetNavigationContext(this RouteData routeData) {
            return new NavigationContext(HttpContext.Current.Request.RequestContext);
        }

        public static StructureInfo GetStructureInfo(this RouteData routeData) {
            return (StructureInfo) routeData.DataTokens["brickpile:structureInfo"];
        }

    }
}
