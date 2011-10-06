using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Article", ControllerType = typeof(PageController))]
    public class Page : BaseEditorial {
        /// <summary>
        /// Gets or sets the image caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        [DataType(DataType.Html)]
        public string ImageCaption { get; set; }
    }
}