using Stormbreaker.Models;

namespace Stormbreaker.Web.UI.Navigation {
    public class NavigationItem : INavigationItem
    {
        //public int? Level { get; private set; }
        public IContentItem ContentItem { get; private set; }
        //public IEnumerable<IContentItem> Children { get; private set; }

        public NavigationItem(IContentItem contentItem) {
            ContentItem = contentItem;
        }
    }
}