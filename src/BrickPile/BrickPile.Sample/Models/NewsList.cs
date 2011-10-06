using BrickPile.Domain;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageModel(Name = "News list", ControllerType = typeof(NewsListController))]
    public class NewsList : BaseModel { }
}