using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Page", Controller = typeof(PageController))]
    public class Page : PageModel {

        [Display(Prompt = "My awesome heading!")]
        public string Heading { get; set; }

        [DataType(DataType.Html)]
        public string MainBody { get; set; }
    }
}