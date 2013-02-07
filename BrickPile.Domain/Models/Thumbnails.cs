namespace BrickPile.Domain.Models {
    public class Thumbnails {
        /// <summary>
        /// Gets or sets the small.
        /// </summary>
        /// <value>
        /// The small.
        /// </value>
        public Thumbnail Small { get; set; }
        /// <summary>
        /// Gets or sets the medium.
        /// </summary>
        /// <value>
        /// The medium.
        /// </value>
        public Thumbnail Medium { get; set; }
        public Thumbnails() {
            Small = new Thumbnail();
            Medium = new Thumbnail();
        }
    }
}