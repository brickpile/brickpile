namespace BrickPile.Domain.Models {
    public class Image : Asset {
        /// <summary>
        /// Gets or sets the alt text.
        /// </summary>
        /// <value>
        /// The alt text.
        /// </value>
        public string AltText { get;set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int? Height { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int? Width { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image() {
            Id = "assets/images/";
            
        }
    }
}