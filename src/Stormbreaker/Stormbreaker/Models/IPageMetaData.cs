namespace Stormbreaker.Models {
    /// <summary>
    /// Represents the page metadata
    /// </summary>
    public interface IPageMetaData {
        /// <summary>
        /// <see cref="PageModel.MetaData.Name"/>
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// <see cref="PageModel.MetaData.Slug"/>
        /// </summary>
        string Slug { get; set; }
    }
}