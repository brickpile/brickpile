using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.ViewModels {
    /// <summary>
    /// The default view model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultViewModel<T> : IViewModel<T> where T : IPageModel {
        /// <summary>
        /// Gets the current model.
        /// </summary>
        public virtual T CurrentModel { get; private set; }
        /// <summary>
        /// Gets the structure info.
        /// </summary>
        public virtual IStructureInfo StructureInfo { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewModel&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public DefaultViewModel(T model, IStructureInfo structureInfo) {
            CurrentModel = model;
            StructureInfo = structureInfo;
        }
    }
}