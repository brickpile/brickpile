using BrickPile.Domain.Models;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.ViewModels {
    /// <summary>
    /// Base view model for all views
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class BaseViewModel<TModel> : DefaultViewModel<TModel>, IBaseViewModel<TModel> where TModel : IPageModel {
        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        public virtual string @Class { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
    }
}