using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Example.Models {
    [PageModel("My off the shelf item")]
    public class Page : ModelBase {

        [Display(Order = 60)]
        [DataType(DataType.Html)]
        public virtual string MainBody { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}