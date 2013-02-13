using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.UI.Controllers;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.UI.Models {
    [ContentType(Name = "Home", ControllerType = typeof(HomeController))]
    public class Home : IContent {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public Image Image { get; set; }

        [DataType(DataType.Html)]
        public string Html { get; set; }
    }
}