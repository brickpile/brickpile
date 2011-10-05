using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "News list", ControllerType = typeof(NewsListController))]
    public class NewsList : BaseModel { }
}