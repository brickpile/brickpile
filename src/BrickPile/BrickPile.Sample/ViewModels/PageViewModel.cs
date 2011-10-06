using BrickPile.Sample.Models;
using BrickPile.UI;

namespace BrickPile.Sample.ViewModels {
    public class PageViewModel : BaseViewModel<Page> {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public PageViewModel(Page model, IStructureInfo structureInfo) : base(model, structureInfo) { }
    }
}