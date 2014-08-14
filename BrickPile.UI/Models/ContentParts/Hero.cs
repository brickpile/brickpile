using System.ComponentModel.DataAnnotations;

namespace BrickPile.Samples.Models.ContentParts
{
    public class Hero
    {
        public string Heading { get; set; }

        public string Caption { get; set; }

        [Display(GroupName = "Action")]
        public string ArticleUrl { get; set; }

        [Display(GroupName = "Action")]
        public string ActionText { get; set; }
    }
}