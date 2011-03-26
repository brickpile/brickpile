using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using Stormbreaker.Repositories;

namespace Stormbreaker.Web.Hosting {
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
    public class DropboxVirtualPathProvider : VirtualPathProvider {
        private readonly IDropboxRepository _repository;
        /// <summary>
        /// Gets the API key.
        /// </summary>
        public string ApiKey {
            get {
                return ConfigurationManager.AppSettings["apiKey"];
            }
        }
        /// <summary>
        /// Gets the app secret.
        /// </summary>
        public string AppSecret {
            get {
                return ConfigurationManager.AppSettings["appSecret"];
            }
        }
        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username {
            get {
                return ConfigurationManager.AppSettings["username"];
            }
        }
        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password {
            get {
                return ConfigurationManager.AppSettings["password"];
            }
        }
        /// <summary>
        /// Gets the physical path.
        /// </summary>
        public string PhysicalPath {
            get {
                return ConfigurationManager.AppSettings["physicalPath"]; }
        }
        /// <summary>
        /// Gets the virtual path.
        /// </summary>
        public string VirtualPath {
            get { return ConfigurationManager.AppSettings["virtualPath"]; }
        }
        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool FileExists(string virtualPath) {
            if (IsPathVirtual(virtualPath)) {
                var file = (DropboxVirtualFile) GetFile(virtualPath);
                return file.Exists;
            }
            return Previous.FileExists(virtualPath);
        }
        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile"/> class that represents a file in the virtual file system.
        /// </returns>
        public override VirtualFile GetFile(string virtualPath) {
            if (IsPathVirtual(virtualPath)) {   
                return new DropboxVirtualFile(virtualPath,this,_repository);
            }
            return Previous.GetFile(virtualPath);
        }
        /// <summary>
        /// Determines whether [is path virtual] [the specified virtual path].
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        ///   <c>true</c> if [is path virtual] [the specified virtual path]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPathVirtual(string virtualPath) {
            var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith(this.VirtualPath, StringComparison.InvariantCultureIgnoreCase);
        }
        public DropboxVirtualPathProvider(IDropboxRepository repository) {
            _repository = repository;
        }
    }
}