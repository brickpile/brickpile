using System;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using Stormbreaker.Repositories;

namespace Stormbreaker.Web.Hosting {
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DropboxVirtualFile : VirtualFile {
        private readonly DropboxVirtualPathProvider _provider;
        private readonly IDropboxRepository _dropboxRepository;
        public override string Name {
            get {
                return HttpUtility.UrlDecode(base.Name);
            }
        }
        //private readonly DropboxClient _client;
        /// <summary>
        /// Gets the local path of the virtual file.
        /// </summary>
        public string PhysicalPath {
            get {
                if(string.IsNullOrEmpty(this.Name)) {
                    throw new ArgumentNullException("Name", "cannot be null or empty");
                }
                
                return Path.Combine(_provider.PhysicalPath, Name);
            }
        }
        /// <summary>
        /// Gets a value indicating whether this <see cref="DropboxVirtualFile"/> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists {
            get {
                // Return true if the file exists on disk
                if (File.Exists(this.PhysicalPath)) {
                    return true;
                }

                var directory = VirtualPathUtility.GetDirectory(VirtualPath.Remove(0,_provider.VirtualPath.Length - 1));
                var fileName = VirtualPathUtility.GetFileName(this.VirtualPath);

                if (string.IsNullOrEmpty(fileName)) {
                    throw new ArgumentNullException("FileName", "cannot be null or empty");
                }

                // try to load the metadata for the file
                var metaData = _dropboxRepository.GetMetaData(Path.Combine(directory, fileName));

                if(!string.IsNullOrEmpty(metaData.Path)) {
                    return true;
                }

                // default is false
                return false;
            }
        }
        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open() {
            if (File.Exists(this.PhysicalPath)) {
                return new FileStream(PhysicalPath, FileMode.Open, FileAccess.Read);
            }
            var directory = VirtualPathUtility.GetDirectory(VirtualPath.Remove(0, _provider.VirtualPath.Length - 1));
            var fileName = VirtualPathUtility.GetFileName(this.VirtualPath);

            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException("FileName", "cannot be null or empty");
            }

            var buffer = _dropboxRepository.GetFile(Path.Combine(directory, fileName));
            using (var fileStream = new FileStream(PhysicalPath, FileMode.Create, FileAccess.Write)) {
                var writer = new BinaryWriter(fileStream);
                writer.Write(buffer);
                writer.Close();
            }
            return new FileStream(PhysicalPath, FileMode.Open, FileAccess.Read);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DropboxVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="dropboxRepository">The dropbox repository.</param>
        public DropboxVirtualFile(string virtualPath, DropboxVirtualPathProvider provider, IDropboxRepository dropboxRepository) : base(virtualPath) {
            _provider = provider;
            _dropboxRepository = dropboxRepository;
        }
    }
}