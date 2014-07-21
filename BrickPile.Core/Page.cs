using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BrickPile.Core
{
    [DebuggerDisplay("Id = {Id}, Url = {Metadata.Url}, Slug = {Metadata.Slug}, ParentId = {Parent}")]
    public class Page : IPage
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [ScaffoldColumn(false)]
        public Metadata Metadata { get; private set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        [ScaffoldColumn(false)]
        public PageReference Parent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Page" /> class.
        /// </summary>
        public Page()
        {
            Metadata = new Metadata();
        }
    }
}
