using System.Linq;
using System.Web;
using System.Web.Routing;

namespace BrickPile.Core.Routing
{
    public interface IVirtualPathResolver {
        /// <summary>
        /// Resolves the virtual path.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        string ResolveVirtualPath(IPage page, RouteValueDictionary routeValueDictionary);
    }

    public class VirtualPathResolver : IVirtualPathResolver {
        private string _action;

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
            _action = routeValueDictionary[PageRoute.ActionKey] as string;

            if (_action != null && !_action.ToLower().Equals(PageRoute.DefaultAction))
            {
                return VirtualPathUtility.AppendTrailingSlash(string.Join("/", new[] { url, _action }.Where(item => !string.IsNullOrWhiteSpace(item)))).ToLower();
            }

            return VirtualPathUtility.AppendTrailingSlash(url).ToLower();
        }
    }
}
