using System;
using System.Web.Hosting;
using BrickPile.Core.Hosting;

namespace BrickPile.FileSystem.AmazonS3.Model {
    public class Image {
        private readonly string _virtualUrl;
        int _width;
        int _height;
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="virtualUrl">The virtual URL.</param>
        public Image(string virtualUrl) {
            _virtualUrl = virtualUrl;
        }

        /// <summary>
        /// Resizes the specified width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public Image Resize(int width, int height) {
            this._width = width;
            this._height = height;
            return this;
        }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {

            // Abort if file does not exist
            if (string.IsNullOrEmpty(_virtualUrl)) {
                return string.Empty;
            }

            var provider = HostingEnvironment.VirtualPathProvider as CommonVirtualPathProvider;
            if (provider == null) {
                throw new Exception("Provider not found");
            }

            var virtualFile = provider.GetFile(this._virtualUrl) as CommonVirtualFile;

            if(virtualFile == null) {
                return string.Empty;
            }

            // no resizing
            if (this._width <= 0 && this._height <= 0) {
                return virtualFile.Url;
            }

            return string.Format("{0}?width={1}&height={2}", virtualFile.VirtualPath, this._width, this._height).ToLower();
        }
    }
}
