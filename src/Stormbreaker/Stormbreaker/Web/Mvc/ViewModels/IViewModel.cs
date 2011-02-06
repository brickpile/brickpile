using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Mvc.ViewModels {
    /// <summary>
    /// Represents the view model
    /// </summary>
    public interface IViewModel<out T> {
        /// <summary>
        /// <see cref="DefaultViewModel{T}.CurrentModel"/>
        /// </summary>
        T CurrentModel { get; }
        /// <summary>
        /// <see cref="DefaultViewModel{T}.StructureInfo"/>
        /// </summary>
        IStructureInfo StructureInfo { get; }
    }
}