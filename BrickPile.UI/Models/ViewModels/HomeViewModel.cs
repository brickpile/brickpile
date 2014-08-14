using System.Collections.Generic;
using BrickPile.Samples.Models.ContentParts;
using BrickPile.Samples.Models.ContentTypes;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Samples.Models.ViewModels
{
    public class HomeViewModel : IViewModel<Home>
    {
        public Home CurrentPage { get; set; }

        public Hero Hero { get; set; }

        public string FeaturesHeading { get; set; }

        public List<Feature> Features { get; set; }

        public List<Service> Services { get; set; }
    }
}