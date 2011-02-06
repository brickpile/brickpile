using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.ViewModels {

    public class DefaultViewModel<T> : IViewModel<T> where T : IPageModel {

        public virtual T CurrentModel { get; private set; }
        /// <summary>
        /// Get/Sets the StructureInfo of the DefaultViewModel
        /// </summary>
        /// <value></value>
        public virtual IStructureInfo StructureInfo { get; private set; }

        public DefaultViewModel(T currentModel, IRepository repository)
        {
            CurrentModel = currentModel;
            StructureInfo = new StructureInfo(repository, currentModel);
        }
    }
}