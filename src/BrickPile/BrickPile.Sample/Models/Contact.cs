using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.Sample.Controllers;
using BrickPile.UI.Web;
using BrickPile.UI.Web.ViewModels;


namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [ContentType(Name = "Contact", ControllerType = typeof(ContactController))]
    public class Contact : IContent {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }

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
