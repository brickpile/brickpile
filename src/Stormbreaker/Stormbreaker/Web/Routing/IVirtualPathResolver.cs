using System.Web.Routing;
using Stormbreaker.Models;

namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// The IVirtualPathResolver interface 
    /// </summary>
    public interface IVirtualPathResolver {
        string ResolveVirtualPath(IContentItem contentItem, RouteValueDictionary routeValueDictionary);
    }
}