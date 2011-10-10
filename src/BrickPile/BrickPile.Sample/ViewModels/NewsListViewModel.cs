using BrickPile.Sample.Models;
using PagedList;

namespace BrickPile.Sample.ViewModels {
    public class NewsListViewModel : BaseViewModel<NewsList> {
        /// <summary>
        /// Gets or sets the case list.
        /// </summary>
        /// <value>
        /// The case list.
        /// </value>
        public IPagedList<News> NewsList { get; set; }
    }
}