using BrickPile.Sample.Models;
using BrickPile.UI;

namespace BrickPile.Sample.ViewModels {
    public class NewsListViewModel : BaseViewModel<NewsList> {
        /// <summary>
        /// Gets or sets the case list.
        /// </summary>
        /// <value>
        /// The case list.
        /// </value>
        public PagedList.IPagedList<News> NewsList { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseListViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="caseList">The case list.</param>
        public NewsListViewModel(NewsList model, IStructureInfo structureInfo, PagedList.IPagedList<News> caseList)
            : base(model, structureInfo) {
            NewsList = caseList;
        }
    }
}