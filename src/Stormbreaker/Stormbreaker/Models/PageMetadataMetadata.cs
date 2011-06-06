using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Models {
    internal class PageMetadataMetadata {
        /// <summary>
        /// Get/Sets the Name of the PageMetaData
        /// </summary>
        /// <value></value>
        [Display(Name = "Name",Order = 10)]
        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }
        /// <summary>
        /// Get/Sets the Slug of the PageMetaData
        /// </summary>
        /// <value></value>
        [Display(Name = "Slug",Order = 20)]
        [ScaffoldColumn(false)]
        public virtual string Slug { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [ScaffoldColumn(false)]
        public string Url { get; set; }
    }
}
