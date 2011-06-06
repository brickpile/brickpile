using System.ComponentModel.DataAnnotations;
using Stormbreaker.Web;

namespace Stormbreaker.Example.Models {
    [PageModel("Home page")]
    public class Home : ModelBase {
        
        //[Display(Name = "News container")]
        //public PageReference PageLink { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Header { get; set; }

        [Display(Name = "Teaser image")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image2 { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image3 { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image4 { get; set; }

    } 
}