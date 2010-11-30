using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Web.UI.Navigation {
    public interface INavigationInfo {
        IContentItem StartItem { get; }
        IContentItem CurrentItem { get; }
        IEnumerable<INavigationItem> NavigationItems { get; }
    }
}