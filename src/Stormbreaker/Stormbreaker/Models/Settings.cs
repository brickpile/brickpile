using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Models {
    /// <summary>
    /// Responsible for some basic settings
    /// </summary>
    public class Settings : ISettings {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string AwsAccessKey { get; set; }
        /// <summary>
        /// Gets or sets the aws secret access key.
        /// </summary>
        /// <value>
        /// The aws secret access key.
        /// </value>
        public string AwsSecretAccessKey { get; set; }
        /// <summary>
        /// Gets or sets the name of the bucket.
        /// </summary>
        /// <value>
        /// The name of the bucket.
        /// </value>
        public string BucketName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings() {
            Id = "stormbreaker/settings";
        }
    }
}
