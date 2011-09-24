using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Page", ControllerType = typeof(PageController))]
    public class Page : PageModel {

        [Display(Prompt = "My awesome heading!")]
        public string Heading { get; set; }

        [DataType(DataType.Html)]
        [Display(Name = "Content")]
        public string MainBody { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }
    }
}