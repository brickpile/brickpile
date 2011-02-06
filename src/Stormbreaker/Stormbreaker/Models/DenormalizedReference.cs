namespace Stormbreaker.Models {
    /// <summary>
    /// Used as reference between documents in RavenDB
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class DenormalizedReference<T> where T : IPageModel {
        /// <summary>
        /// Get/Sets the Id of the DenormalizedReference
        /// </summary>
        /// <value></value>
        public string Id { get; set; }
        /// <summary>
        /// Get/Sets the Slug of the DenormalizedReference
        /// </summary>
        /// <value></value>
        public string Slug { get; set; }
        /// <summary>
        /// Implicitly converts a page model to a DenormalizedReference
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static implicit operator DenormalizedReference<T>(T document)
        {
            return new DenormalizedReference<T>
                       {
                           Id = document.Id,
                           Slug = document.MetaData.Slug
                       };
        }
    }
}