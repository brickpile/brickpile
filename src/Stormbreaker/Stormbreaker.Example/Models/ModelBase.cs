using System.ComponentModel.DataAnnotations;
using Stormbreaker.Example.App_LocalResources;
using Stormbreaker.Models;

namespace Stormbreaker.Example.Models {
    public abstract class ModelBase : PageModel {
        /* *******************************************************************
	    *  Metadata 
	    * *******************************************************************/
        [Display(Name = "Title", ResourceType = typeof(Details), Order = 30)]
        public virtual string Title { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Details), Order = 40)]
        public virtual string Description { get; set; }
        /* *******************************************************************
	    *  Content 
	    * *******************************************************************/
        [Display(Name = "Heading", ResourceType = typeof(Details), Order = 50)]
        public virtual string Heading { get; set; }

    }
}