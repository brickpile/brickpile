using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Post", ControllerType = typeof(PageController))]
    public class Post : PageModel {
    }
}