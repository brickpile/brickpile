using System.Collections.Generic;
using BrickPile.Samples.Models.ContentTypes;

namespace BrickPile.Samples.Models
{
    public class Features
    {        
        public string Heading { get; set; }

        public List<ContentPart> FeatureList { get; set; } // Points to three different features

        public Features()
        {
            FeatureList = new List<ContentPart>();
        }
    }
}