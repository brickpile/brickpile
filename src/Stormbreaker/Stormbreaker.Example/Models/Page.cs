using System.ComponentModel.DataAnnotations;
using Stormbreaker.Models;

namespace Stormbreaker.Example.Models {
    [PageModel("My off the shelf item")]
    public class Page : ModelBase {
        
        [Display(Order = 60)]
        [DataType(DataType.Html)]
        public virtual string MainBody { get; set; }
        
        public string ImageUrl { get; set; }

        public PageReference Container { get; set; }
    }
}