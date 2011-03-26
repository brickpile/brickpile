namespace Stormbreaker.Models {
    /// <summary>
    /// Represents the page model
    /// </summary>
    public interface IPageModel : IDocumentModel {
        /// <summary>
        ///   <see cref="PageModel.MetaData"/>
        /// </summary>
        IPageMetaData MetaData { get; }
        /// <summary>
        ///   <see cref="PageModel.Parent"/>
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        DenormalizedReference<IPageModel> Parent { get; set; }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        int? SortOrder { get; set; }
    }
}