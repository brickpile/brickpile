namespace Stormbreaker.Models {
    public interface ISettings : IDocumentModel {
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        string AwsAccessKey { get; set; }
        /// <summary>
        /// Gets or sets the aws secret access key.
        /// </summary>
        /// <value>
        /// The aws secret access key.
        /// </value>
        string AwsSecretAccessKey { get; set; }
    }
}