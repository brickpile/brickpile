using System;
using System.IO;
using System.Web.Helpers;
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
        /// <param name="virtualFile">The virtual file.</param>
        /// <returns></returns>
        public static string Image(this UrlHelper helper, AmazonS3VirtualFile virtualFile) {
            var baseUri = new Uri("http://s3.getbrickpile.com");
            return new Uri(baseUri,virtualFile.VirtualPath).ToString();
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
}