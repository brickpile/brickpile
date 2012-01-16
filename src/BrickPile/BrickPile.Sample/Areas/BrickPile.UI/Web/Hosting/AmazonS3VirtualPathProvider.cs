using System.Configuration;
using System.Web.Hosting;
using Amazon;
using Amazon.Runtime;
using Amazon.S3.Model;

namespace BrickPile.UI.Web.Hosting {
    public class AmazonS3VirtualPathProvider : VirtualPathProvider {
        private GetObjectResponse _response;
        /// <summary>
        /// Gets the virtual path root.
        /// </summary>
        public string VirtualPathRoot {
            get { return ConfigurationManager.AppSettings["S3VirtualPathRoot"]; }
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

        public override VirtualFile GetFile(string virtualPath) {

            if (virtualPath.StartsWith(VirtualPathRoot)) {
                virtualPath = virtualPath.Replace(VirtualPathRoot, string.Empty);

                var client = AWSClientFactory.CreateAmazonS3Client();
                var request = new GetObjectRequest();
                request.WithBucketName(BucketName).WithKey(virtualPath);

                try {
                    _response = client.GetObject(request);
                    //using (GetObjectResponse response = client.GetObject(request)) { }
                } catch (AmazonServiceException exception) { }

                return new AmazonS3VirtualFile(virtualPath, _response);
            }

            if (string.IsNullOrEmpty(virtualPath)) {
                return null;
            }

            return this.Previous.GetFile(virtualPath);
            
        }

        public override VirtualDirectory GetDirectory(string virtualDirectory) {
            if (virtualDirectory.StartsWith(VirtualPathRoot)) {
                virtualDirectory = virtualDirectory.Replace(VirtualPathRoot, string.Empty);
                return new AmazonS3VirtualDirectory(this, virtualDirectory);
            }
            return Previous.GetDirectory(virtualDirectory);
        }

        //private bool IsPathVirtual(string virtualPath) {
        //    if (string.IsNullOrEmpty(virtualPath))
        //        return false;
        //    var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
        //    return checkPath.StartsWith(VirtualPathUtility.Combine(VirtualPath, "public/"), StringComparison.InvariantCultureIgnoreCase);
        //}

        //public string GetPath(string virtualPath) {
        //    return "/" + VirtualPathUtility.MakeRelative(VirtualPath, virtualPath);
        //}

        //public override bool DirectoryExists(string virtualDir) {
        //    if (IsPathVirtual(virtualDir)) {
        //        var dir = new DropboxVirtualDirectory(virtualDir, this, _service);
        //        return dir.Exists;
        //    }
        //    return Previous.DirectoryExists(virtualDir);
        //}
    }
}