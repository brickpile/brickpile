using System;
using System.ComponentModel.DataAnnotations;
using BrickPile.Core;
using BrickPile.Domain;
using BrickPile.Samples.Controllers;
using BrickPile.UI.Web;

namespace BrickPile.Samples.Models.ContentTypes {
    [ContentType(Name = "Container", ControllerType = typeof(ContainerController))]
    public class Container : Page {

        public string Heading { get; set; }                
        public PageReference ContainerPage { get; set; }                
        public DateTime CurrentDate { get; set; }
        public Image Image { get; set; }
        [DataType(DataType.Html)]
        public string MainBody { get; set; }
    }
}