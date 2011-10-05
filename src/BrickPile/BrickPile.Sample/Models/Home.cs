using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;
using System.ComponentModel.DataAnnotations;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Start page", ControllerType = typeof(HomeController))]
    public class Home : BaseModel {
        [Display(Name="Heading")]
        public string Heading { get; set; }

        [Display(Name = "Body")]
        public string Body { get; set; }
    }
}