using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace BrickPile.Core.Hosting
{
    public class NativeVirtualPathProvider : CommonVirtualPathProvider
    {
        /// <summary>
        ///     Gets the physical path.
        /// </summary>
        public override string PhysicalPath
        {
            get
            {
                var path = ConfigurationManager.AppSettings["PhysicalPath"];

                // If specified path starts with "~"; make it relative to the server map path.
                if (!string.IsNullOrEmpty(path) && path.StartsWith("~"))
                {
                    path = HostingEnvironment.MapPath(path);
                }

                return path;
            }
        }

        /// <summary>
        ///     Gets the virtual path root.
        /// </summary>
        public override string VirtualPathRoot
        {
            get
            {
                return
                    VirtualPathUtility.AppendTrailingSlash(
                        VirtualPathUtility.ToAbsolute(ConfigurationManager.AppSettings["VirtualPathRoot"]));
            }
        }

        /// <summary>
        ///     Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        ///     true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool FileExists(string virtualPath)
        {
            if (virtualPath.StartsWith(this.VirtualPathRoot))
            {
                return
                    File.Exists(Path.Combine(this.PhysicalPath, virtualPath.Replace(this.VirtualPathRoot, string.Empty)));
            }
            return base.FileExists(virtualPath);
        }

        /// <summary>
        ///     Gets a value that indicates whether a directory exists in the virtual file system.
        /// </summary>
        /// <param name="virtualDir">The path to the virtual directory.</param>
        /// <returns>
        ///     true if the directory exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool DirectoryExists(string virtualDir)
        {
            if (virtualDir.StartsWith(this.VirtualPathRoot))
            {
                return
                    Directory.Exists(Path.Combine(this.PhysicalPath,
                        virtualDir.Replace(this.VirtualPathRoot, string.Empty)));
            }
            return base.DirectoryExists(virtualDir);
        }

        /// <summary>
        ///     Gets a virtual directory from the virtual file system.
        /// </summary>
        /// <param name="virtualDir">The path to the virtual directory.</param>
        /// <returns>
        ///     A descendent of the <see cref="T:System.Web.Hosting.VirtualDirectory" /> class that represents a directory in the
        ///     virtual file system.
        /// </returns>
        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            if (virtualDir.StartsWith(this.VirtualPathRoot))
            {
                return new NativeVirtualDirectory(this, virtualDir);
            }
            return Previous.GetDirectory(virtualDir);
        }

        /// <summary>
        ///     Gets a virtual file from the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        ///     A descendent of the <see cref="T:System.Web.Hosting.VirtualFile" /> class that represents a file in the virtual
        ///     file system.
        /// </returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            if (virtualPath.StartsWith(this.VirtualPathRoot))
            {
                return new NativeVirtualFile(this, virtualPath);
            }
            return Previous.GetFile(virtualPath);
        }
    }
}