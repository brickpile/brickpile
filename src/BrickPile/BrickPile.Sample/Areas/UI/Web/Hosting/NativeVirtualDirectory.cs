using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using BrickPile.Core.Hosting;

namespace BrickPile.UI.Web.Hosting {
    public class NativeVirtualDirectory : CommonVirtualDirectory {
        private readonly NativeVirtualPathProvider _provider;
        private readonly string _virtualDir;
        private readonly DirectoryInfo _directoryInfo;
        /// <summary>
        /// Gets a list of all the subdirectories contained in this directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualDirectory"/> objects.</returns>
        public override IEnumerable Directories {
            get {
                if (!this._directoryInfo.Exists) {
                    throw new DirectoryNotFoundException(string.Format("Directory no longer exists in native file system: '{0}'.", this._directoryInfo.FullName));
                }
                return this._directoryInfo.GetDirectories().Select(info => new NativeVirtualDirectory(this._provider, Path.Combine(this._virtualDir, info.Name))).ToList();
            }
        }
        /// <summary>
        /// Gets a list of all files contained in this directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualFile"/> objects.</returns>
        public override IEnumerable Files {
            get {
                if (!this._directoryInfo.Exists) {
                    throw new DirectoryNotFoundException(string.Format("Directory no longer exists in native file system: '{0}'.", this._directoryInfo.FullName));
                }
                return this._directoryInfo.GetFiles().Select(info => new NativeVirtualFile(this._provider, Path.Combine(this._virtualDir, info.Name))).ToList();
            }
        }
        /// <summary>
        /// Gets a list of the files and subdirectories contained in this virtual directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualFile"/> and <see cref="T:System.Web.Hosting.VirtualDirectory"/> objects.</returns>
        public override IEnumerable Children {
            get {
                yield return Files;
                yield return Directories;
            }
        }
        /// <summary>
        /// Gets the parent.
        /// </summary>
        public override CommonVirtualDirectory Parent {
            get {
                var directory = VirtualPathUtility.GetDirectory(base.VirtualPath);
                return (_provider.GetDirectory(directory) as CommonVirtualDirectory);
            }
        }
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override CommonVirtualDirectory CreateDirectory(string name) {
            if (Directory.Exists(Path.Combine(this._directoryInfo.FullName, name))) {
                throw new InvalidOperationException(string.Format("Directory already exists in native file system: '{0}'.", Path.Combine(this._virtualDir, name)));
            }
            this._directoryInfo.CreateSubdirectory(name);
            return new NativeVirtualDirectory(this._provider, Path.Combine(this._virtualDir, name));
        }
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public override void Delete() {
            this._directoryInfo.Delete(true);
        }
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override CommonVirtualFile CreateFile(string name) {

            if (File.Exists(Path.Combine(this._provider.PhysicalPath, this._virtualDir.Replace(this._provider.VirtualPathRoot, string.Empty), name))) {
                throw new InvalidOperationException(string.Format("File already exists in native file system: '{0}'.", Path.Combine(this._virtualDir, name)));
            }

            var fs = File.Create(Path.Combine(this._provider.PhysicalPath, this._virtualDir.Replace(this._provider.VirtualPathRoot, string.Empty), name));
            fs.Close();

            return new NativeVirtualFile(this._provider, Path.Combine(this._virtualDir, name));
        }
        /// <summary>
        /// Gets the display name of the virtual resource.
        /// </summary>
        /// <returns>The display name of the virtual file.</returns>
        public override string Name {
            get { return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(base.Name); }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeVirtualDirectory"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="virtualDir">The virtual dir.</param>
        public NativeVirtualDirectory(NativeVirtualPathProvider provider, string virtualDir) : base(virtualDir) {
            _provider = provider;
            _virtualDir = virtualDir;
            this._directoryInfo = new DirectoryInfo(Path.Combine(this._provider.PhysicalPath, virtualDir.Replace(this._provider.VirtualPathRoot, string.Empty)));
            if (!this._directoryInfo.Exists) {
                throw new DirectoryNotFoundException(string.Format("Full path to physical native directory is invalid: '{0}'.", this._directoryInfo.FullName));
            }
            
        }
    }
}