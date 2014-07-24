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

        public static NavigationContext GetNavigationContext(this RouteData data) {
            return new NavigationContext(HttpContext.Current.Request.RequestContext);            
        }

    }
}
