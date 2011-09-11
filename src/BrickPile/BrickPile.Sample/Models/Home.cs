using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Start page", Controller = typeof(HomeController))]
    public class Home : PageModel {
        public string Heading { get; set; }
    }
}