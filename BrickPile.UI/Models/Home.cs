using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;
using BrickPile.UI.Web;

namespace BrickPile.Samples.Models {
    [ContentType(Name = "Home", ControllerType = typeof(HomeController))]
    public class Home : IContent {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public string Heading { get; set; }

        [DataType(DataType.MultilineText)]
        public string MainIntro { get; set; }

        [Required]
        public Image Image { get; set; }
    }
}