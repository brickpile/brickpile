using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BrickPile.Core;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;
using BrickPile.UI.Web;

namespace BrickPile.Samples.Models {
    [ContentType(Name = "Home", ControllerType = typeof(HomeController))]    
    public class Home : Page {

        [Required]
        public string Heading { get; set; }

        [DataType(DataType.MultilineText)]
        public string MainIntro { get; set; }

        [DataType(DataType.Html)]
        public string MainBody { get; set; }
        
        public Image Image { get; set; }
    }
}