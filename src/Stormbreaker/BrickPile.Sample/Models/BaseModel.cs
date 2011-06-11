using BrickPile.Domain.Models;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// Abstract base model used as a default template for all page models
    /// </summary>
    public abstract class BaseModel : PageModel {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }
    }
}
