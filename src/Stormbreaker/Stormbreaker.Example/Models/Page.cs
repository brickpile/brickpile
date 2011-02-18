using System;
using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Example.Models {
    [PageModel("My off the shelf item")]
    [MetadataType(typeof(PageMetadata))]
    public partial class Page : ModelBase {
        public virtual string MainBody { get; set; }
        public virtual string PageLink { get; set; }
    }
    public partial class PageMetadata {
        [Display(Order = 60)]
        [DataType(DataType.Html)]
        public virtual string MainBody { get; set; }

        [DataType(DataType.Url, ErrorMessage = "fooo")]
        [Display(Name = "Länken")]        
        public virtual string PageLink { get; set; }
    }
}