namespace Stormbreaker.Models {
    /// <summary>
    /// Represents the page metadata
    /// </summary>
    public interface IPageMetaData {
        /// <summary>
        /// <see cref="PageModel.MetaData"/>
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// <see cref="PageModel.MetaData"/>
        /// </summary>
        string Slug { get; set; }
    }
}