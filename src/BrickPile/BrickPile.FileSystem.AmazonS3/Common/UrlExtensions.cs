using System.Web.Mvc;
using BrickPile.FileSystem.AmazonS3.Hosting;
using BrickPile.FileSystem.AmazonS3.Model;

namespace BrickPile.FileSystem.AmazonS3.Common {
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
        public static Image Image(this UrlHelper helper, AmazonS3VirtualFile s3VirtualFile) {
            return new Image(s3VirtualFile);
        }
    }
}