using System.Configuration;
using System.Web;
using System.Web.Hosting;

namespace BrickPile.UI.Web.Hosting {
    public class AmazonS3VirtualPathProvider : VirtualPathProvider {
        //private readonly AmazonS3 _client;
        /// <summary>
        /// Gets the virtual path root.
        /// </summary>
        public string VirtualPathRoot {
            get { return VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["S3VirtualPathRoot"])); }
        }
        /// <summary>
        /// Gets the name of the bucket.
        /// </summary>
        /// <value>
        /// The name of the bucket.
        /// </value>
        public string BucketName {
            get {
                return ConfigurationManager.AppSettings["S3Bucket"];
            }
        }
        public string HostName {
            get {
                return ConfigurationManager.AppSettings["S3Hostname"];
            }
        }
        /// <summary>
        /// Gets the physical path.
        /// </summary>
        public string PhysicalPath {
            // todo get from vpp
            get { return @"D:\Projects\Git\Stormbreaker\src\BrickPile\BrickPile.Sample\cache"; }
        }
        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile"/> class that represents a file in the virtual file system.
        /// </returns>
        public override VirtualFile GetFile(string virtualPath) {
            if(virtualPath.StartsWith(VirtualPathRoot)) {
                return new AmazonS3VirtualFile(this,virtualPath);
            }
            return this.Previous.GetFile(virtualPath);
        }
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="virtualDirectory">The virtual directory.</param>
        /// <returns></returns>
        public override VirtualDirectory GetDirectory(string virtualDirectory) {
            if (virtualDirectory.StartsWith(VirtualPathRoot)) {
                return new AmazonS3VirtualDirectory(this, virtualDirectory);
            }
            return Previous.GetDirectory(virtualDirectory);
        }

        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        //public override bool FileExists(string virtualPath) {
        //    if(virtualPath.StartsWith(VirtualPathRoot)) {
        //        try {
        //            virtualPath = virtualPath.Replace(VirtualPathRoot, string.Empty);
        //           _client.GetObjectMetadata(new GetObjectMetadataRequest()
        //                                            .WithBucketName(this.BucketName)
        //                                            .WithKey(virtualPath));

        //            Debug.WriteLine("Making a request to Amazon: File exists");

        //            return true;

        //        } catch (AmazonS3Exception ex) {
        //            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //                return false;

        //            throw;
        //        }
        //    }
        //    return base.FileExists(virtualPath);
        //}
        /// <summary>
        /// Gets a value that indicates whether a directory exists in the virtual file system.
        /// </summary>
        /// <param name="virtualDir">The path to the virtual directory.</param>
        /// <returns>
        /// true if the directory exists in the virtual file system; otherwise, false.
        /// </returns>
        //public override bool DirectoryExists(string virtualDir) {
        //    if (virtualDir.StartsWith(VirtualPathRoot)) {
        //        try {
        //            virtualDir = virtualDir.Replace(VirtualPathRoot, string.Empty);
        //            if (string.IsNullOrEmpty(virtualDir)) {
        //                return true;
        //            }
        //            _client.GetObjectMetadata(new GetObjectMetadataRequest()
        //                                          .WithBucketName(this.BucketName)
        //                                          .WithKey(virtualDir));

        //            Debug.WriteLine("Making a request to Amazon: Directory exists");

        //            return true;
        //        }
        //        catch (AmazonS3Exception ex) {
        //            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        //                return false;

        //            throw;
        //        }
        //    }
        //    return base.DirectoryExists(virtualDir);
        //}
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonS3VirtualPathProvider"/> class.
        /// </summary>
        //public AmazonS3VirtualPathProvider() {
        //    this._client =
        //        AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config
        //                                                  {
        //                                                      ServiceURL = "s3.amazonaws.com",
        //                                                      CommunicationProtocol = Protocol.HTTP
        //                                                  });
        //}
    }
}