using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BrickPile.Core
{
    /// <summary>
    ///     Represents the standard <see cref="Page" /> in BrickPile.
    /// </summary>
    [DebuggerDisplay("Id = {Id}, Url = {Metadata.Url}, Slug = {Metadata.Slug}, ParentId = {Parent}")]
    public abstract class Page : IPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Page" /> class.
        /// </summary>
        protected Page()
        {
            this.Metadata = new Metadata();
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        /// <summary>
        ///     Gets the metadata.
        /// </summary>
        /// <value>
        ///     The metadata.
        /// </value>
        [ScaffoldColumn(false)]
        public Metadata Metadata { get; private set; }

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        [ScaffoldColumn(false)]
        public PageReference Parent { get; set; }
    }
}