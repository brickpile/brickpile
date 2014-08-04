using System;
using System.Diagnostics;
using BrickPile.Core.Extensions;

namespace BrickPile.Core
{
    /// <summary>
    ///     Represents a reference between pages.
    /// </summary>
    [DebuggerDisplay("Id = {Id}")]
    public class PageReference
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PageReference" /> class.
        /// </summary>
        public PageReference() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="PageReference" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public PageReference(string id)
        {
            this.Id = id;
        }

        /// <summary>
        ///     Pages the reference.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator PageReference(Page page)
        {
            if (page.IsDraft())
            {
                throw new Exception("A draft cannot act as parent to another page.");
            }

            return new PageReference(page.Id);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Id;
        }
    }
}