using System;
using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;
using BrickPile.UI.Web;

namespace BrickPile.Samples.Models {
    [ContentType(Name = "Container", ControllerType = typeof(ContainerController))]
    public class Container : Page {

        public string Heading { get; set; }                
        public PageReference ContainerPage { get; set; }                
        public DateTime CurrentDate { get; set; }
    }
}