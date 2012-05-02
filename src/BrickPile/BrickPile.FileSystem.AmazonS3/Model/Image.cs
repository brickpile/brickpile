using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using BrickPile.FileSystem.AmazonS3.Hosting;

namespace BrickPile.FileSystem.AmazonS3.Model {
    public class Image {
        readonly AmazonS3VirtualFile _virtualFile;
        int _width;
        int _height;
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="s3VirtualFile">The s3 virtual file.</param>
        public Image(AmazonS3VirtualFile s3VirtualFile) {
            this._virtualFile = s3VirtualFile;
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

            // check if it does exist
            //var virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(this._virtualPath) as AmazonS3VirtualFile;

            //if (_virtualFile == null || string.IsNullOrEmpty(_virtualFile.Etag)) {
            //    return string.Empty;
            //    //throw new HttpException(404, "File not found");
            //}

            var provider = HostingEnvironment.VirtualPathProvider as AmazonS3VirtualPathProvider;

            if (provider == null) {
                throw new Exception("Provider not found");
            }

            // no resizing
            if (this._width <= 0 && this._height <= 0) {
                return _virtualFile.Url;
            }

            // local path to source
            
            var directory = Path.Combine(provider.LocalPath, _virtualFile.VirtualPath.Replace(provider.VirtualPathRoot,null).Replace(VirtualPathUtility.GetFileName(_virtualFile.VirtualPath),null), _virtualFile.Etag.Replace("\"", ""));

            // create local directory for the file
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            string tmpFile = Path.Combine(directory, Path.ChangeExtension(VirtualPathUtility.GetFileName(_virtualFile.VirtualPath), null) + "_" + this._width + "_" + this._height + ".tmp");
            string localPath = Path.Combine(directory, Path.ChangeExtension(VirtualPathUtility.GetFileName(_virtualFile.VirtualPath), null) + "_" + this._width + "_" + this._height + Path.GetExtension(_virtualFile.VirtualPath));

            if (!File.Exists(localPath) && !File.Exists(tmpFile)) {
                var fs = File.Create(tmpFile);
                fs.Close();
            }

            return string.Format("{0}_{1}_{2}{3}", Path.ChangeExtension(this._virtualFile.VirtualPath, null), this._width, this._height, Path.GetExtension(this._virtualFile.VirtualPath));
        }
    }
}
