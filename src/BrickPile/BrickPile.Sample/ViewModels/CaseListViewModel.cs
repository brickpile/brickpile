using BrickPile.Sample.Models;
using PagedList;

namespace BrickPile.Sample.ViewModels {
    public class CaseListViewModel : BaseViewModel<CaseList> {
        /// <summary>
        /// Gets or sets the case list.
        /// </summary>
        /// <value>
        /// The case list.
        /// </value>
        public IPagedList<Case> CaseList { get; set; }
    }
}