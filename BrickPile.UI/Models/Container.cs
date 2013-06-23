using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;

namespace BrickPile.Samples.Models {
    [ContentType(Name = "Container", ControllerType = typeof(ContainerController))]
    public class Container : Page {

        public string Heading { get; set; }

        [UIHint("Markdown")]
        public string MainBody { get; set; }

        //[Required]
        //public PageReference ContainerPage { get; set; }
    }
    public class ContainerViewModel {
        public Container CurrentPage { get; set; }
    }
}