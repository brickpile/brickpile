using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrickPile.Domain.Models {
    public class Ancestor : IPageModel {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }
        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public IPageMetadata Metadata { get; private set; }
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public DocumentReference<IPageModel> Parent { get; set; }
        /// <summary>
        /// Gets or sets the ancestors.
        /// </summary>
        /// <value>
        /// The ancestors.
        /// </value>
        [ScaffoldColumn(false)]
        public IList<Ancestor> Ancestors { get; set; }
        /// <summary>
        /// Gets the children.
        /// </summary>
        [ScaffoldColumn(false)]
        public List<string> Children { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Ancestor"/> class.
        /// </summary>
        public Ancestor() {
            Metadata = new PageMetadata();
            Ancestors = new List<Ancestor>();
            Children = new List<string>();
        }
    }
}
