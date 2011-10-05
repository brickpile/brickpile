using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Article", ControllerType = typeof(PageController))]
    public class Page : BaseEditorial {
        public string ImageCaption { get; set; }
    }
}