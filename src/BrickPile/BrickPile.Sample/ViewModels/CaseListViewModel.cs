using BrickPile.Sample.Models;
using BrickPile.UI;

namespace BrickPile.Sample.ViewModels {
    public class CaseListViewModel : BaseViewModel<CaseList> {
        /// <summary>
        /// Gets or sets the case list.
        /// </summary>
        /// <value>
        /// The case list.
        /// </value>
        public PagedList.IPagedList<Case> CaseList { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseListViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="caseList">The case list.</param>
        public CaseListViewModel(CaseList model, IStructureInfo structureInfo, PagedList.IPagedList<Case> caseList) : base(model, structureInfo) {
            CaseList = caseList;
        }
    }
}