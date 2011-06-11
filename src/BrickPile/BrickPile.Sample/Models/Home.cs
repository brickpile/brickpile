using BrickPile.Domain;
using BrickPile.Domain.Models;

namespace BrickPile.Sample.Models {
    [PageModel("Start page")]
    public class Home : PageModel {
        public string Heading { get; set; }
    }
}