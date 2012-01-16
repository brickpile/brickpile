using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using Amazon.S3.Model;

namespace BrickPile.UI.Web.Hosting {
    public class AmazonS3VirtualDirectory : VirtualDirectory {
        private readonly AmazonS3VirtualPathProvider _virtualPathProvider;
        private readonly string _virtualPath;
        private readonly Amazon.S3.AmazonS3Client _client;
        /// <summary>
        /// Gets a list of all the subdirectories contained in this directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualDirectory"/> objects.</returns>
        public override IEnumerable Directories {
            get {
                var folder = _virtualPath.Replace(_virtualPathProvider.VirtualPathRoot, string.Empty);
                var request = new ListObjectsRequest().WithBucketName(_virtualPathProvider.BucketName).WithPrefix(folder);
                using (var response = this._client.ListObjects(request)) {
                    if (folder == string.Empty || folder == "/") {
                        // get the objects at the TOP LEVEL, i.e. not inside any folders
                        var objects = response.S3Objects.Where(o => !o.Key.Contains(@"/"));

                        // get the folders at the TOP LEVEL only
                        return response.S3Objects.Except(objects).Where(o => o.Key.Last() == '/' && o.Key.IndexOf(@"/") == o.Key.LastIndexOf(@"/")).Select(n => n.Key);
                    }

                    var directories = new List<string>();

                    foreach (var split in response.S3Objects.Select(s3Object => s3Object.Key.Replace(folder, string.Empty).Split('/')).Where(splits => splits.Count() > 1 && !directories.Contains(splits.First()))) {
                        directories.Add(split.First());
                    }

                    return directories;
                }                
            }
        }
        /// <summary>
        /// Gets a list of all files contained in this directory.
        /// </summary>
        /// <returns>An object implementing the <see cref="T:System.Collections.IEnumerable"/> interface containing <see cref="T:System.Web.Hosting.VirtualFile"/> objects.</returns>
        public override IEnumerable Files {
            get {
                var request = new ListObjectsRequest().WithBucketName(_virtualPathProvider.BucketName).WithPrefix(_virtualPath.Replace(_virtualPathProvider.VirtualPathRoot,string.Empty)).WithDelimiter("/");

                using (var response = this._client.ListObjects(request)) {
                    return response.S3Objects.Where(o => o.Key.Last() != '/');
                }                
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
        /// Initializes a new instance of the <see cref="AmazonS3VirtualDirectory"/> class.
        /// </summary>
        /// <param name="virtualPathProvider">The virtual path provider.</param>
        /// <param name="virtualPath">The virtual path to the resource represented by this instance.</param>
        public AmazonS3VirtualDirectory(AmazonS3VirtualPathProvider virtualPathProvider, string virtualPath)
            : base(virtualPath) {
            _client = new Amazon.S3.AmazonS3Client();
            _virtualPathProvider = virtualPathProvider;
            _virtualPath = virtualPath;
        }
    }
}