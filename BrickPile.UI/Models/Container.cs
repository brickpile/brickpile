using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Samples.Controllers;
using BrickPile.UI.Web;

namespace BrickPile.Samples.Models {
    [ContentType(Name = "Container", ControllerType = typeof(ContainerController))]
    public class Container : IContent {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public string Heading { get; set; }        
        [Required]
        public PageReference ContainerPage { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CurrentDate { get; set; }
    }
}