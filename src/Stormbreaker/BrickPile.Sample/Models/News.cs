using System;
using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;

namespace BrickPile.Sample.Models {
    [PageModel("News item")]
    public class News : BaseEditorial {
        /// <summary>
        /// Gets or sets the news image.
        /// </summary>
        /// <value>
        /// The news image.
        /// </value>
        [DataType(DataType.ImageUrl)]
        public string NewsImage { get; set; }
        /// <summary>
        /// Gets or sets the pub date.
        /// </summary>
        /// <value>
        /// The pub date.
        /// </value>
        [DataType(DataType.Date)]
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="News"/> class.
        /// </summary>
        public News() {
            if(PubDate == null) {
                PubDate = DateTime.Now.Date.AddMonths(1);
            }
        }
    }
}