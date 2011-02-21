using System.ComponentModel.DataAnnotations;
using Stormbreaker.Example.Models;
using Stormbreaker.Models;
using Stormbreaker.Web.Mvc.ViewModels;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.ViewModels {
    public class HomeViewModel : DefaultViewModel<Home> {
        /// <summary>
        /// Gets or sets the paged list.
        /// </summary>
        /// <value>
        /// The paged list.
        /// </value>
        public IPagedList<Page> PagedList { get; set; }
        /// <summary>
        /// Gets or sets the news container.
        /// </summary>
        /// <value>
        /// The news container.
        /// </value>
        [DataType(DataType.Url)]
        public IPageModel NewsContainer { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="newsContainer">The news container.</param>
        public HomeViewModel(Home model,
            IStructureInfo structureInfo,
            IPagedList<Page> pagedList,
            IPageModel newsContainer) : base(model, structureInfo) {
            PagedList = pagedList;
            NewsContainer = newsContainer;
        }
    }
}