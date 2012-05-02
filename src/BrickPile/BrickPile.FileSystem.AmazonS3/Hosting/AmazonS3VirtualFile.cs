using System.IO;
using System.Net;
using System.Web;
using System.Web.Hosting;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace BrickPile.FileSystem.AmazonS3.Hosting {

    public class AmazonS3VirtualFile : VirtualFile {

        private readonly AmazonS3VirtualPathProvider _provider;
        private readonly Amazon.S3.AmazonS3 _client ;
        private readonly string _virtualPath;
        //private string _etag;

        /// <summary>
        /// Gets the local path.
        /// </summary>
        public string Url {
            get {
                var provider = this._provider;
                string baseUrl = VirtualPathUtility.AppendTrailingSlash(provider.HostName);
                return string.Format("{0}{1}", baseUrl, this.VirtualPath.Replace(provider.VirtualPathRoot, string.Empty));
            }
        }
        /// <summary>
        /// Gets or sets the etag.
        /// </summary>
        /// <value>
        /// The etag.
        /// </value>
        public string Etag {
            get {
                try {
                    if(HttpContext.Current.Cache[this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty)] == null ) {
                        using (var response = _client.GetObjectMetadata(new GetObjectMetadataRequest().WithBucketName(_provider.BucketName).WithKey(this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty)))) {
                            HttpContext.Current.Cache[this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty)] = response.ETag;
                        }
                    }

                } catch(AmazonS3Exception exception) {
                    // handle this
                }
                return (string) HttpContext.Current.Cache[this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty)];
            }
        }

        public string LocalPath {
            get { return this.VirtualPath; }
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open() {

            try {

                var request = new GetObjectRequest().WithBucketName(_provider.BucketName).WithKey(this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty));
                var response = this._client.GetObject(request);
                return response.ResponseStream;

            } catch (AmazonS3Exception exception) {

                if (exception.StatusCode == HttpStatusCode.NotFound) {
                    throw new FileNotFoundException();
                }

            }
            return Stream.Null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonS3VirtualFile"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="virtualPath">The virtual path.</param>
        public AmazonS3VirtualFile(AmazonS3VirtualPathProvider provider, string virtualPath) : base(virtualPath) {
            _provider = provider;
            _virtualPath = virtualPath;
            this._client = AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config
                                                                     {
                                                                         ServiceURL = "s3.amazonaws.com",
                                                                         CommunicationProtocol = Protocol.HTTP
                                                                     });
        }
    }
}