using Stormbreaker.Models;

namespace Stormbreaker.Web.UI.Navigation {
    public interface INavigationItem {
        //int? Level { get; }
        IContentItem ContentItem { get; }
        //IEnumerable<IContentItem> Children { get; }
    }
}