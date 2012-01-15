using System.IO;
using System.Web.Hosting;
using Amazon.S3.Model;

namespace BrickPile.UI.Web.Hosting {

    public class AmazonS3VirtualFile : VirtualFile {
        private readonly GetObjectResponse _response;
        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open() {
            return _response.ResponseStream;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonS3VirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="response">The response.</param>
        public AmazonS3VirtualFile(string virtualPath, GetObjectResponse response)
            : base(virtualPath) {
            _response = response;
        }
    }
}