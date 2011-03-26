using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Models {
    /// <summary>
    /// Represents the metadata of the page
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PageMetaData : IPageMetaData {
        /// <summary>
        /// Get/Sets the Name of the PageMetaData
        /// </summary>
        /// <value></value>
        [Display(
            Name = "Name",
            Order = 10,
            ResourceType = typeof(Resources.PageMetaData))]
        [Required(
            ErrorMessageResourceName = "Name_Required",
            ErrorMessageResourceType = typeof(Resources.PageMetaData))]
        public virtual string Name { get; set; }
        /// <summary>
        /// Get/Sets the Slug of the PageMetaData
        /// </summary>
        /// <value></value>
        [Display(
            Name = "Slug",
            Order = 20,
            ResourceType = typeof(Resources.PageMetaData))]
        [Required(
            ErrorMessageResourceName = "Slug_Required",
            ErrorMessageResourceType = typeof(Resources.PageMetaData))]
        public virtual string Slug { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [Required]
        public string Url { get; set; }
    }
}