using BrickPile.Domain;
using BrickPile.Sample.Controllers;
using System.ComponentModel.DataAnnotations;

namespace BrickPile.Sample.Models {
    [PageModel(Name = "Start page", ControllerType = typeof(HomeController))]
    public class Home : BaseModel {
        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        [Display(Name="Heading")]
        public string Heading { get; set; }
    }
}