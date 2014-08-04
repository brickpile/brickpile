using System.IO;

namespace BrickPile.Core.Hosting
{
    public class NativeVirtualFile : CommonVirtualFile
    {
        private readonly NativeVirtualPathProvider provider;
        private readonly string virtualPath;

        /// <summary>
        ///     Gets the URL.
        /// </summary>
        public override string Url
        {
            get { return base.VirtualPath.ToLower(); }
        }

        /// <summary>
        ///     Gets the local path.
        /// </summary>
        public override string LocalPath
        {
            get
            {
                return
                    Path.Combine(this.provider.PhysicalPath,
                        this.virtualPath.Replace(this.provider.VirtualPathRoot, string.Empty)).ToLower();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NativeVirtualFile" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="virtualPath">The virtual path.</param>
        public NativeVirtualFile(NativeVirtualPathProvider provider, string virtualPath)
            : base(virtualPath)
        {
            this.provider = provider;
            this.virtualPath = virtualPath;
        }

        /// <summary>
        ///     When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        ///     A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open()
        {
            return this.Open(FileMode.Open);
        }

        /// <summary>
        ///     Opens the specified file mode.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <returns></returns>
        public override Stream Open(FileMode fileMode)
        {
            return
                new FileStream(
                    Path.Combine(this.provider.PhysicalPath,
                        this.virtualPath.Replace(this.provider.VirtualPathRoot, string.Empty)), fileMode);
        }

        /// <summary>
        ///     Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            if (File.Exists(this.LocalPath))
            {
                File.Delete(this.LocalPath);
            }
        }
    }
}