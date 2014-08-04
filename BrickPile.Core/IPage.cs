namespace BrickPile.Core
{
    /// <summary>
    ///     Defines the properties that are required for an <see cref="IPage" />.
    /// </summary>
    public interface IPage
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        ///     Gets the metadata.
        /// </summary>
        /// <value>
        ///     The metadata.
        /// </value>
        Metadata Metadata { get; }

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        PageReference Parent { get; set; }
    }
}