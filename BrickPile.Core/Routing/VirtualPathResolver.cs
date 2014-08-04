using System.Linq;
using System.Web;
using System.Web.Routing;

namespace BrickPile.Core.Routing
{
    public class VirtualPathResolver : IVirtualPathResolver {
        private string action;

        /// <summary>
        /// Resolves the virtual path.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        public virtual string ResolveVirtualPath(IPage page, RouteValueDictionary routeValueDictionary)
        {
            var url = page.Metadata.Url ?? string.Empty;

            if (!routeValueDictionary.ContainsKey(PageRoute.ActionKey))
                return VirtualPathUtility.AppendTrailingSlash(url).ToLower();
            this.action = routeValueDictionary[PageRoute.ActionKey] as string;

            if (this.action != null && !this.action.ToLower().Equals(PageRoute.DefaultAction))
            {
                return VirtualPathUtility.AppendTrailingSlash(string.Join("/", new[] { url, this.action }.Where(item => !string.IsNullOrWhiteSpace(item)))).ToLower();
            }

            return VirtualPathUtility.AppendTrailingSlash(url).ToLower();
        }
    }
}
