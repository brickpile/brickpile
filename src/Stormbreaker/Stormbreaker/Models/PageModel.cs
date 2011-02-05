using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Models {
    /// <summary>
    /// The base class for page models
    /// </summary>
    /// <remarks>Use IPageModel to create your own base class</remarks>
    /// <example></example>
    public abstract class PageModel : IPageModel {
        /// <summary>
        /// Get/Sets the Id of the page model
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        /// <summary>
        /// Get/Sets the MetaData of the page model
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        public virtual IPageMetaData MetaData { get; private set; }
        /// <summary>
        /// Get/Sets the Parent of the page model
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        public virtual DenormalizedReference<IPageModel> Parent { get; set; }
        /// <summary>
        /// Get/Sets the Children of the page model
        /// </summary>
        /// <value></value>
        [ScaffoldColumn(false)]
        public virtual IList<DenormalizedReference<IPageModel>> Children { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModel"/> class.
        /// </summary>
        protected PageModel()
        {
            MetaData = new PageMetaData();
            Children = new List<DenormalizedReference<IPageModel>>();
        }
    }
}