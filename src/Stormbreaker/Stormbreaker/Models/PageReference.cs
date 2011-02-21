using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Models {
    public class PageReference {
        /// <summary>
        /// Gets or sets the selected page model.
        /// </summary>
        /// <value>
        /// The selected page model.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }
    }
}