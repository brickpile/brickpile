using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;
using BrickPile.UI.Web;


namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageType(Name = "Contact", ControllerType = typeof(ContactController))]
    public class Contact : PageModel {
        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        //[Required]
        [Display(Order = 100, Prompt = "Enter a descriptive heading")]
        public virtual string Heading { get; set; }
        /// <summary>
        /// Gets or sets the main body.
        /// </summary>
        /// <value>
        /// The main body.
        /// </value>
        public HtmlString MainBody { get; set; }
    }
}
