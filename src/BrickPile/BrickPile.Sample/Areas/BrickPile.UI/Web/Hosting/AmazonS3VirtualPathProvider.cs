using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace BrickPile.UI.Web.Hosting {
    public class AmazonS3VirtualPathProvider : VirtualPathProvider {
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
        /// Gets the physical path mapping to the virtual path root.
        /// </summary>
        public string LocalPath {
            // todo get from vpp configuration
            get {
                if(string.IsNullOrEmpty(_localPath)) {
                    _localPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(),
                                              this.BucketName);
                }
                return _localPath;
            }
        }
        private string _localPath;
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
    }
}