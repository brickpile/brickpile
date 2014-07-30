using System.Diagnostics;

namespace BrickPile.Core
{
    [DebuggerDisplay("Id = {Id}")]
    public class PageReference
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Pages the reference.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static implicit operator PageReference(Page page)
        {
            return new PageReference(page.Id.Replace("/draft",""));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference"/> class.
        /// </summary>
        public PageReference()
        {    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public PageReference(string id)
        {
            Id = id;
        }
    }
}
