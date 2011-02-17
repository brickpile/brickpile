using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Example.Models {
    [PageModel("Form page")]
    public class Form : ModelBase {
        [DisplayName("Title")]
        public string Title { get; set; }
    }
}