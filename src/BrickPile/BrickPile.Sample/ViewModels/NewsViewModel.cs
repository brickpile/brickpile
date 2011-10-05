using BrickPile.Sample.Models;
using BrickPile.UI;

namespace BrickPile.Sample.ViewModels {
    public class NewsViewModel : BaseViewModel<News> {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public NewsViewModel(News model, IStructureInfo structureInfo) : base(model, structureInfo) { }
    }
}