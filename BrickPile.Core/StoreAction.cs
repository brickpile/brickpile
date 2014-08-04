namespace BrickPile.Core
{
    /// <summary>
    ///     Represents the different store actions available in BrickPile.
    /// </summary>
    public enum StoreAction
    {
        None = 0,

        /// <summary>
        ///     Saves the page as a working copy (draft)
        /// </summary>
        Save = 1,

        /// <summary>
        ///     Publishes the page
        /// </summary>
        Publish = 2,

        /// <summary>
        ///     The un publish
        /// </summary>
        UnPublish = 3
    }
}