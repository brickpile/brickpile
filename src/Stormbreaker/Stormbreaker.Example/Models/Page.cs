using System;
using System.ComponentModel.DataAnnotations;
using Stormbreaker.Web;

namespace Stormbreaker.Example.Models {
    [PageModel("My off the shelf item")]
    public class Page : ModelBase {
        
        [Display(Order = 60,Name = "Text")]
        [DataType(DataType.Html)]
        public virtual string MainBody { get; set; }
        
        public string ImageUrl { get; set; }
        
        //public PageReference Container { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start publish")]
        public DateTime? PublishDate { get; set; }
    }
}