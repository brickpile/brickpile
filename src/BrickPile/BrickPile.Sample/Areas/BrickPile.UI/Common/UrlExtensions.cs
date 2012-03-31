using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using BrickPile.UI.Web.Hosting;

namespace BrickPile.UI.Common {
    /// <summary>
    /// Summary description for UrlExtensions.
    /// </summary>
    /// <remarks>
    /// 2012-01-21 marcus: Created
    /// </remarks>
    public static class UrlExtensions {
        /// <summary>
        /// Images the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        public static Image Image(this UrlHelper helper, string virtualPath) {
            return new Image(virtualPath);
        }
        /// <summary>
        /// Images the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="virtualFile">The virtual file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        //public static string Image(this UrlHelper helper, AmazonS3VirtualFile virtualFile, int width, int height) {

        //    if (!Directory.Exists(virtualFile.ThumbnailDirectory)) {
        //        Directory.CreateDirectory(virtualFile.ThumbnailDirectory);
        //    }

        //    var imagePath = Path.Combine(virtualFile.ThumbnailDirectory, width + "_" + height + virtualFile.Extension);

        //    //my-bucket.s3.amazonaws.com/thumbs/path/to/file.png/"78skfghsfsfkgjh"small.png

        //    if (!File.Exists(imagePath)) {
        //        using (var stream = virtualFile.Open()) {
        //            var thumb = new WebImage(stream).Resize(width + 1, height + 1);
        //            thumb.Crop(1, 1);
        //            thumb.Save(imagePath, virtualFile.Extension.Remove(0, 1));
        //        }
        //    }

        //    return virtualFile.LocalPath
        //           + "/"
        //           + width
        //           + "_"
        //           + height
        //           + virtualFile.Extension;
        //}
    }
    public class Image {
        readonly string _virtualPath;
        int _width;
        int _height;
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        public Image(string virtualPath) {
            this._virtualPath = virtualPath;
        }
        /// <summary>
        /// Resizes the specified width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public Image Resize(int width,int height) {
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
            
            // check if it does exist
            var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(this._virtualPath) as AmazonS3VirtualFile;

            if (virtualFile == null) {
                throw new HttpException(404, "File not found");
            }

            var provider = HostingEnvironment.VirtualPathProvider as AmazonS3VirtualPathProvider;

            if(provider == null) {
                throw new Exception("Provider not found");
            }

            // no resizing
            if(this._width <= 0 && this._height <= 0) {
                return virtualFile.Url;
            }

            // local path to source
            var directory = Path.Combine(provider.LocalPath, virtualFile.Etag.Replace("\"", ""));

            // create local directory for the file
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            string tmpFile = Path.Combine(directory, Path.ChangeExtension(virtualFile.VirtualPath.Replace(provider.VirtualPathRoot, null), null) + "_" + this._width + "_" + this._height + ".tmp");
            string localPath = Path.Combine(directory, Path.ChangeExtension(virtualFile.VirtualPath.Replace(provider.VirtualPathRoot, null), null) + "_" + this._width + "_" + this._height + Path.GetExtension(virtualFile.VirtualPath));

            if (!File.Exists(localPath) && !File.Exists(tmpFile)) {
                var fs = File.Create(tmpFile);
                fs.Close();
            }

            return string.Format("{0}_{1}_{2}{3}", Path.ChangeExtension(this._virtualPath, null), this._width, this._height, Path.GetExtension(this._virtualPath));
        }
    }
}