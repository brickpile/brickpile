using Stormbreaker.Models;
using Stormbreaker.Web.UI.Navigation;

namespace Stormbreaker.Web.Mvc.ViewModels {
    public interface IViewModel<out T> {
        INavigationInfo NavigationInfo { get; }
        T CurrentItem { get; }
    }
}