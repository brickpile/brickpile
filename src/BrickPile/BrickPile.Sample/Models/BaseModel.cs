using BrickPile.Domain.Models;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// Abstract base model used as a default template for all page models
    /// </summary>
    public abstract class BaseModel : PageModel {
        public virtual string Title { get; set; }
    }
}
