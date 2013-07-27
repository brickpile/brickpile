using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;

namespace BrickPile.Samples.Models {
    [PageType(Name = "Home", ControllerType = typeof(HomeController))]
    public class Home : Page {

        public string Heading { get; set; }

        [DataType(DataType.MultilineText)]
        public string MainIntro { get; set; }

        //[Required]
        //[ScaffoldColumn(false)]
        //public Image Image { get; set; }
        public TheProp TheProp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        public Home() {
            TheProp = new TheProp();
        }
    }

}