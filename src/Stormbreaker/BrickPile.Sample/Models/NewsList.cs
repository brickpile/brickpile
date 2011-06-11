using System.ComponentModel.DataAnnotations;
using BrickPile.Domain;

namespace BrickPile.Sample.Models {
    [PageModel("News list")]
    public class NewsList : BaseModel {
        /// <summary>
        /// Gets or sets the news count.
        /// </summary>
        /// <value>
        /// The news count.
        /// </value>
        [UIHint("Number")]
        public int? NewsCount { get; set; }
    }
}