using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.ViewModels {
    public interface IBaseViewModel<out TModel> : IViewModel<TModel> {
        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        string @Class { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }
    }
}