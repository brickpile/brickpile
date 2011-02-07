using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.ViewModels {

    public class DefaultViewModel<T> : IViewModel<T> where T : IPageModel {
        /// <summary>
        /// Get/Sets the CurrentModel of the DefaultViewModel
        /// </summary>
        /// <value></value>
        public virtual T CurrentModel { get; private set; }
        /// <summary>
        /// Get/Sets the StructureInfo of the DefaultViewModel
        /// </summary>
        /// <value></value>
        public virtual IStructureInfo StructureInfo { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewModel{T}" /> class.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="repository"></param>
        public DefaultViewModel(T model, IRepository repository)
        {
            CurrentModel = model;
            StructureInfo = new StructureInfo(repository, model);
        }
    }
}