using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrickPile.Core;
using BrickPile.Domain;
using BrickPile.Samples.Controllers;

namespace BrickPile.Samples.Models.ContentTypes {
    [ContentType]    
    public class Home : Page {

        [Display(GroupName = "Hero unit")]
        public ContentPart Hero { get; set; } // Points to a single hero unit

        [Display(GroupName = "Introduction")]
        public Introduction Introduction { get; set; }

        [Display(GroupName = "Features")]
        public Features Features { get; set; }

        [Display(GroupName = "Services")]
        public List<ContentPart> ServicesCarousel { get; set; } // Points to a list of content parts

        // Swag, not editable atm

        [Display(GroupName = "Portfolio")]
        public List<ContentPart> PortfolioCarousel { get; set; }

        // Button, ignite your passion, not editable atm

        public List<ContentPart> TeamCarousel { get; set; }

        // Form, not editable atm

        // Contact, not editable atm

        public Home()
        {
            ServicesCarousel = new List<ContentPart>();
            PortfolioCarousel = new List<ContentPart>();
        }
    }

    public class ContentPart
    {
        public string Id { get; set; }
    }
}
