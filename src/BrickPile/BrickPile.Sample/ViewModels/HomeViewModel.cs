using BrickPile.Sample.Models;
using BrickPile.UI;

namespace BrickPile.Sample.ViewModels {
    public class HomeViewModel : BaseViewModel<Home> {
        private readonly Home _model;
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public HomeViewModel(Home model, IStructureInfo structureInfo) : base(model,structureInfo) {
            _model = model;
        }
    }
}