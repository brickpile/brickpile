using System.ComponentModel.DataAnnotations;
using Stormbreaker.Models;

namespace Stormbreaker.Example.Models {
    [PageModel("Home page")]
    public class Home : ModelBase {

        [Display(Name = "Hämta nyheter från")]
        public PageReference PageLink { get; set; }

        [Display(Name = "Ett namn")]
        public bool? IsSelected { get; set; }
        
    } 
}