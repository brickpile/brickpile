namespace BrickPile.Core.Configuration {
    /// <summary>
    /// 
    /// </summary>
    public interface IConfiguration {
        /// <summary>
        /// Gets the id.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Gets or sets the name of the site.
        /// </summary>
        /// <value>
        /// The name of the site.
        /// </value>
        string SiteName { get; set; }
    }
}