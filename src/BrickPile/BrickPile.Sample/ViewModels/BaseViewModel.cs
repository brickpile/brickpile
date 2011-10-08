using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.UI;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel&lt;TModel&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        protected BaseViewModel(TModel model, IStructureInfo structureInfo) : base(model, structureInfo) {
            var page = model as BaseModel;
            if (page != null) {
                Title = "Acme" + " - " + page.Metadata.Title;
            }
        }
    }
}