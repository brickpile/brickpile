using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace BrickPile.Core.Hosting
{
    public class NativeVirtualDirectory : CommonVirtualDirectory
    {
        private readonly DirectoryInfo directoryInfo;
        private readonly NativeVirtualPathProvider provider;
        private readonly string virtualDir;

        /// <summary>
        ///     Gets a list of all the subdirectories contained in this directory.
        /// </summary>
        /// <returns>
        ///     An object implementing the <see cref="T:System.Collections.IEnumerable" /> interface containing
        ///     <see cref="T:System.Web.Hosting.VirtualDirectory" /> objects.
        /// </returns>
        public override IEnumerable Directories
        {
            get
            {
                if (!this.directoryInfo.Exists)
                {
                    throw new DirectoryNotFoundException(
                        string.Format("Directory no longer exists in native file system: '{0}'.",
                            this.directoryInfo.FullName));
                }
                return
                    this.directoryInfo.GetDirectories()
                        .Select(
                            info =>
                                new NativeVirtualDirectory(this.provider, Path.Combine(this.virtualDir, info.Name)))
                        .ToList();
            }
        }

        /// <summary>
        ///     Gets a list of all files contained in this directory.
        /// </summary>
        /// <returns>
        ///     An object implementing the <see cref="T:System.Collections.IEnumerable" /> interface containing
        ///     <see cref="T:System.Web.Hosting.VirtualFile" /> objects.
        /// </returns>
        public override IEnumerable Files
        {
            get
            {
                if (!this.directoryInfo.Exists)
                {
                    throw new DirectoryNotFoundException(
                        string.Format("Directory no longer exists in native file system: '{0}'.",
                            this.directoryInfo.FullName));
                }
                return
                    this.directoryInfo.GetFiles()
                        .Select(info => new NativeVirtualFile(this.provider, Path.Combine(this.virtualDir, info.Name)))
                        .ToList();
            }
        }

        /// <summary>
        ///     Gets a list of the files and subdirectories contained in this virtual directory.
        /// </summary>
        /// <returns>
        ///     An object implementing the <see cref="T:System.Collections.IEnumerable" /> interface containing
        ///     <see cref="T:System.Web.Hosting.VirtualFile" /> and <see cref="T:System.Web.Hosting.VirtualDirectory" /> objects.
        /// </returns>
        public override IEnumerable Children
        {
            get
            {
                yield return this.Files;
                yield return this.Directories;
            }
        }

        /// <summary>
        ///     Gets the parent.
        /// </summary>
        public override CommonVirtualDirectory Parent
        {
            get
            {
                var directory = VirtualPathUtility.GetDirectory(base.VirtualPath);
                return (this.provider.GetDirectory(directory) as CommonVirtualDirectory);
            }
        }

        /// <summary>
        ///     Gets the display name of the virtual resource.
        /// </summary>
        /// <returns>The display name of the virtual file.</returns>
        public override string Name
        {
            get { return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(base.Name); }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NativeVirtualDirectory" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="virtualDir">The virtual dir.</param>
        public NativeVirtualDirectory(NativeVirtualPathProvider provider, string virtualDir) : base(virtualDir)
        {
            this.provider = provider;
            this.virtualDir = virtualDir;
            this.directoryInfo =
                new DirectoryInfo(Path.Combine(this.provider.PhysicalPath,
                    virtualDir.Replace(this.provider.VirtualPathRoot, string.Empty)));
            if (!this.directoryInfo.Exists)
            {
                throw new DirectoryNotFoundException(
                    string.Format("Full path to physical native directory is invalid: '{0}'.",
                        this.directoryInfo.FullName));
            }
        }

        /// <summary>
        ///     Creates the directory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override CommonVirtualDirectory CreateDirectory(string name)
        {
            if (Directory.Exists(Path.Combine(this.directoryInfo.FullName, name)))
            {
                throw new InvalidOperationException(
                    string.Format("Directory already exists in native file system: '{0}'.",
                        Path.Combine(this.virtualDir, name)));
            }
            this.directoryInfo.CreateSubdirectory(name);
            return new NativeVirtualDirectory(this.provider, Path.Combine(this.virtualDir, name));
        }

        /// <summary>
        ///     Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public override void Delete()
        {
            this.directoryInfo.Delete(true);
        }

        /// <summary>
        ///     Creates the file.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override CommonVirtualFile CreateFile(string name)
        {
            if (
                File.Exists(Path.Combine(this.provider.PhysicalPath,
                    this.virtualDir.Replace(this.provider.VirtualPathRoot, string.Empty), name)))
            {
                throw new InvalidOperationException(string.Format("File already exists in native file system: '{0}'.",
                    Path.Combine(this.virtualDir, name)));
            }

            var fs =
                File.Create(Path.Combine(this.provider.PhysicalPath,
                    this.virtualDir.Replace(this.provider.VirtualPathRoot, string.Empty), name));
            fs.Close();

            return new NativeVirtualFile(this.provider, Path.Combine(this.virtualDir, name));
        }
    }
}