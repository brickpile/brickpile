using System.ComponentModel.DataAnnotations;
using Stormbreaker.Models;

namespace Stormbreaker.Example.Models {
    [PageModel("My off the shelf item")]
    public class Page : ModelBase {

        [Display(Order = 60)]
        [DataType(DataType.Html)]
        public virtual string MainBody { get; set; }

        [Display(Name = "Hämta nyhetr från")]
        public PageReference PageLink { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}