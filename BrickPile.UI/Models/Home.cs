using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;

namespace BrickPile.Samples.Models {
    [ContentType(Name = "Home", ControllerType = typeof(HomeController))]
    public class Home : IContent {
        [ScaffoldColumn(false)]
        public string Id { get; set; }
    }
}