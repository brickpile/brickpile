using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;
using BrickPile.UI.Models;

namespace BrickPile.Sample.Models {
    [PageModel("Start page")]
    public class Home : BaseModel {
        /// <summary>
        /// Gets or sets the teaser one.
        /// </summary>
        /// <value>
        /// The teaser one.
        /// </value>
        [Required]
        [Display(Name = "Teaser 1")]
        public virtual PageReference TeaserOne { get; set; }
        /// <summary>
        /// Gets or sets the teaser one image.
        /// </summary>
        /// <value>
        /// The teaser one image.
        /// </value>
        [Required]
        [DataType(DataType.ImageUrl)]
        public virtual string TeaserOneImage { get; set; }
        /// <summary>
        /// Gets or sets the teaser one heading.
        /// </summary>
        /// <value>
        /// The teaser one heading.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        public virtual string TeaserOneHeading { get; set; }
        /// <summary>
        /// Gets or sets the teaser two.
        /// </summary>
        /// <value>
        /// The teaser two.
        /// </value>
        [Required]
        [Display(Name = "Teaser 2")]
        public virtual PageReference TeaserTwo { get; set; }
        /// <summary>
        /// Gets or sets the teaser two image.
        /// </summary>
        /// <value>
        /// The teaser two image.
        /// </value>
        [Required]
        [DataType(DataType.ImageUrl)]
        public virtual string TeaserTwoImage { get; set; }
        /// <summary>
        /// Gets or sets the teaser two heading.
        /// </summary>
        /// <value>
        /// The teaser two heading.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        public virtual string TeaserTwoHeading { get; set; }

        /// <summary>
        /// Gets or sets the teaser three.
        /// </summary>
        /// <value>
        /// The teaser three.
        /// </value>
        [Required]
        [Display(Name = "Teaser 3")]
        public virtual PageReference TeaserThree { get; set; }
        /// <summary>
        /// Gets or sets the teaser three image.
        /// </summary>
        /// <value>
        /// The teaser three image.
        /// </value>
        [Required]
        [DataType(DataType.ImageUrl)]
        public virtual string TeaserThreeImage { get; set; }
        /// <summary>
        /// Gets or sets the teaser three heading.
        /// </summary>
        /// <value>
        /// The teaser three heading.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        public virtual string TeaserThreeHeading { get; set; }
        /// <summary>
        /// Gets or sets the teaser four.
        /// </summary>
        /// <value>
        /// The teaser four.
        /// </value>
        [Required]
        [Display(Name = "Teaser 4")]
        public virtual PageReference TeaserFour { get; set; }
        /// <summary>
        /// Gets or sets the teaser four image.
        /// </summary>
        /// <value>
        /// The teaser four image.
        /// </value>
        [Required]
        [DataType(DataType.ImageUrl)]
        public virtual string TeaserFourImage { get; set; }
        /// <summary>
        /// Gets or sets the teaser four heading.
        /// </summary>
        /// <value>
        /// The teaser four heading.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        public virtual string TeaserFourHeading { get; set; }
        /// <summary>
        /// Gets or sets the main teaser image.
        /// </summary>
        /// <value>
        /// The main teaser image.
        /// </value>
        [Required]
        [DataType(DataType.ImageUrl)]
        public string MainTeaserImage { get; set; }
        /// <summary>
        /// Gets or sets the main teaser image alt.
        /// </summary>
        /// <value>
        /// The main teaser image alt.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        public string MainTeaserImageAlt { get; set; }
        /// <summary>
        /// Gets or sets the main teaser link.
        /// </summary>
        /// <value>
        /// The main teaser link.
        /// </value>
        [Required]
        public virtual PageReference MainTeaserLink { get; set; }
    }
}