using System;
using System.IO;
using System.Web.Hosting;

namespace BrickPile.Core.Hosting {
    public abstract class CommonVirtualFile : VirtualFile {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        public virtual string Url {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Gets the local path.
        /// </summary>
        public virtual string LocalPath {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open() {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Opens the specified file mode.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <returns></returns>
        public virtual Stream Open(FileMode fileMode) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public virtual void Delete() {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path to the resource represented by this instance.</param>
        protected CommonVirtualFile(string virtualPath) : base(virtualPath) { }
    }
}
