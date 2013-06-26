using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;
using BrickPile.UI.Web;

namespace BrickPile.Samples.Models {
    [ContentType(Name = "Container", ControllerType = typeof(ContainerController))]
    public class Container : PageModel {

        public string Heading { get; set; }        
        [Required]
        public PageReference ContainerPage { get; set; }

        public string Test { get; set; }
    }
}