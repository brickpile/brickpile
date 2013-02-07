using System;
using System.Collections;
using System.Web.Hosting;

namespace BrickPile.Core.Hosting {
    /// <summary>
    /// 
    /// </summary>
    public abstract class CommonVirtualDirectory : VirtualDirectory {
        /// <summary>
        /// Gets the parent.
        /// </summary>
        public virtual CommonVirtualDirectory Parent {
            get {
                throw new NotSupportedException("CommonVirtualDirectory base class doesn't support property 'Parent'.");
            }
        }
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        public virtual CommonVirtualDirectory CreateDirectory(string virtualPath) {
            throw new NotSupportedException("CommonVirtualDirectory base class doesn't support method 'CreateDirectory'.");
        }
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public virtual void Delete() {
            throw new NotSupportedException("This method or operation is not supported");
        }
        /// <summary>
        /// Unifieds the virtual file.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual CommonVirtualFile CreateFile(string name) {
            throw new NotSupportedException("This method or operation is not supported");
        }
        /// <summary>
        /// Gets a list of all the subdirectories contained in this directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualDirectory"/> objects.</returns>
        public override IEnumerable Directories {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a list of all files contained in this directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualFile"/> objects.</returns>
        public override IEnumerable Files {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a list of the files and subdirectories contained in this virtual directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualFile"/> and <see cref="T:System.Web.Hosting.VirtualDirectory"/> objects.</returns>
        public override IEnumerable Children {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonVirtualDirectory"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path to the resource represented by this instance.</param>
        protected CommonVirtualDirectory(string virtualPath) : base(virtualPath) { }
    }
}