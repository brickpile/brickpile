using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Dashboard.Web.Mvc.ViewModels {
    public interface IDashboardViewModel {
        IPageModel CurrentModel { get; }
        IStructureInfo StructureInfo { get; }
    }
}