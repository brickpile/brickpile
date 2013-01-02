using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using BrickPile.Core.Hosting;

namespace BrickPile.FileSystem.AmazonS3.Hosting {
    public class AmazonS3VirtualPathProvider : CommonVirtualPathProvider {

        private string _physicalPath;
        /// <summary>
        /// Gets the physical path.
        /// </summary>
        public override string PhysicalPath {
            // todo get from vpp configuration
            get {
                if (string.IsNullOrEmpty(_physicalPath)) {
                    _physicalPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(),
                                              this.BucketName);
                }
                return _physicalPath;
            }
        }

        /// <summary>
        /// Gets the virtual path root.
        /// </summary>
        public override string VirtualPathRoot {
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

        /// <summary>
        /// Gets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName {
            get {
                return ConfigurationManager.AppSettings["S3Hostname"];
            }
        }

        /// <summary>
        /// Gets the length of the max request.
        /// </summary>
        /// <value>
        /// The length of the max request.
        /// </value>
        public int MaxRequestLength {
            get {
                // Set the maximum file size for uploads in bytes.
                var section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                if (section != null) {
                    // Retreive the maximum request length from the web.config and convert to bytes.
                    return (section.MaxRequestLength * 1024);
                }
                return 4194304;
            }
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
    }
}