using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "News item", ControllerType = typeof(NewsController))]
    public class News : BaseEditorial { }
}