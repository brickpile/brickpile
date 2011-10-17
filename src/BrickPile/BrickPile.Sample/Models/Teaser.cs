using System.ComponentModel.DataAnnotations;
using BrickPile.UI.Models;

namespace BrickPile.Sample.Models {
    public class Teaser {
        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        [Display(Name = "Teaser heading")]
        public string Heading { get; set; }
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        [Display(Name = "Teaser link", Prompt = "Select page...")]
        public virtual ModelReference Link { get; set; }
    }
}