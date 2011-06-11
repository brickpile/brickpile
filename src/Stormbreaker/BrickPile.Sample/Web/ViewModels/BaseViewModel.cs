using BrickPile.Domain.Models;
using BrickPile.Sample.Configuration;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.Web.ViewModels {
    /// <summary>
    /// Base view model for all views
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class BaseViewModel<TModel> : DefaultViewModel<TModel>, IBaseViewModel<TModel> where TModel : IPageModel {

        public virtual string @Class { get; set; }

        public string Title { get; set; }

        protected BaseViewModel(TModel model, IStructureInfo structureInfo) : base(model, structureInfo) {
            var page = model as BaseModel;
            if(page != null) {
                Title = Constants.SiteName + " - " + page.Title;
            }
        }
    }
}