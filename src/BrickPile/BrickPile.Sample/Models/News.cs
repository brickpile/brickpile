using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageModel(Name = "News item", ControllerType = typeof(NewsController))]
    public class News : BaseEditorial { }
}