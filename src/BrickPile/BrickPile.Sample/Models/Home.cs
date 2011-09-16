using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;
using System.ComponentModel.DataAnnotations;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Start page", Controller = typeof(HomeController))]
    public class Home : PageModel {
        [Display(Name="Heading", Description="The heading...", GroupName="Group1")]
        public string Heading { get; set; }

        public string Heading1 { get; set; }

        [Display(Name = "Body", Description = "The body...", GroupName = "Group1")]
        public string Body { get; set; }
    }
}