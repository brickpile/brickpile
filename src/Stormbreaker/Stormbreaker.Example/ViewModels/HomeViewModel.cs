using Stormbreaker.Example.Models;
using Stormbreaker.Models;
using Stormbreaker.Web.Mvc.ViewModels;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.ViewModels {
    public class HomeViewModel : DefaultViewModel<Home>
    {
        public IPagedList<Page> PagedList { get; set; }
        public HomeViewModel(IPageModel model, IStructureInfo structureInfo, IPagedList<Page> pagedList)
            : base((Home)model, structureInfo)
        {
            PagedList = pagedList;
        }
    }
}