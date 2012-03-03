using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace BrickPile.UI.Web.Hosting {

    public class AmazonS3VirtualFile : VirtualFile {

        private readonly AmazonS3VirtualPathProvider _provider;
        private readonly AmazonS3 _client ;
        private readonly string _virtualPath;
        private GetObjectResponse _response;

        //private string _etag;
        //private string _thumbnailDirectory;
        //private string _localPath;
        //private string _extension;

        /// <summary>
        /// Gets the response.
        /// </summary>
        private GetObjectResponse Response {
            get {
                try {
                    if(_response == null) {
                        _response = _client.GetObject(new GetObjectRequest()
                            .WithBucketName(_provider.BucketName)
                            .WithKey(this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty)));

                        Debug.WriteLine("Making a request to Amazon: Response");
                    }
                    return _response;

                } catch(AmazonS3Exception exception) {
                    if (exception.StatusCode == System.Net.HttpStatusCode.NotFound) {
                        throw new FileNotFoundException();
                    }
                }
                return _response;
            }
        }
        /// <summary>
        /// Gets the local path.
        /// </summary>
        public string Url {
            get {
                //TODO get scheme and hostname from provider
                var provider = this._provider;

                string baseUrl = VirtualPathUtility.AppendTrailingSlash(provider.HostName);

                return string.Format("{0}{1}", baseUrl, this.VirtualPath.Replace(provider.VirtualPathRoot, string.Empty));
            }
        }
        /// <summary>
        /// Gets or sets the etag.
        /// </summary>
        /// <value>
        /// The etag.
        /// </value>
        //public string Etag {
        //    get { return _etag ?? (_etag = this.Response.ETag); }
        //    set { _etag = value; }
        //}
        /// <summary>
        /// Gets the thumbnail directory.
        /// </summary>
        //public string ThumbnailDirectory {
        //    get {
        //        if(string.IsNullOrEmpty(_thumbnailDirectory)) {
        //            try {
        //                _thumbnailDirectory = Path.Combine(this._provider.PhysicalPath,
        //                    this._provider.BucketName,
        //                    this.VirtualPath.Remove(this.VirtualPath.LastIndexOf("/", StringComparison.Ordinal),
        //                    this.VirtualPath.Length - this.VirtualPath.LastIndexOf("/", StringComparison.Ordinal))
        //                    .Replace("/", "\\"),
        //                    this.Etag.Replace("\"", ""));
        //                } catch {
        //                    throw;
        //                }
        //        }
        //        return _thumbnailDirectory;
        //    }
        //}
        /// <summary>
        /// Gets the local path.
        /// </summary>
        //public string LocalPath {
        //    get {
        //        if(string.IsNullOrEmpty(_localPath)) {
        //            _localPath = "/cache/"
        //                         + ((AmazonS3VirtualPathProvider) HostingEnvironment.VirtualPathProvider).BucketName
        //                         + "/"
        //                         + this.VirtualPath.Remove(this.VirtualPath.LastIndexOf("/", StringComparison.Ordinal),
        //                                                   this.VirtualPath.Length -
        //                                                   this.VirtualPath.LastIndexOf("/", StringComparison.Ordinal))
        //                               .Replace("/", "\\")
        //                         + "/"
        //                         + this.Etag.Replace("\"", "");
        //        }
        //        return _localPath;
        //    }
        //}
        /// <summary>
        /// Gets the extension.
        /// </summary>
        //public string Extension {
        //    get {
        //        if(string.IsNullOrEmpty(_extension)) {
        //            _extension = VirtualPathUtility.GetExtension(this.VirtualPath);
        //        }
        //        return _extension;
        //    }
        //}
        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open() {

            //TODO check if the virtual path is root before doing this

            //var directoryPath = Path.Combine(@"c:\temp\thumbs\",
            //    this.Response.Key.Remove(this.Response.Key.LastIndexOf("/", System.StringComparison.Ordinal),
            //    this.Response.Key.Length - this.Response.Key.LastIndexOf("/", System.StringComparison.Ordinal))
            //    .Replace("/", "\\"));

            //var directoryPath = Path.Combine(HostingEnvironment.MapPath("~/App_Data"),
            //    this._provider.BucketName,
            //    this.Response.Key.Remove(this.Response.Key.LastIndexOf("/", StringComparison.Ordinal),
            //    this.Response.Key.Length - this.Response.Key.LastIndexOf("/", StringComparison.Ordinal))
            //    .Replace("/", "\\"));

            //if (!Directory.Exists(directoryPath)) {
            //    Directory.CreateDirectory(directoryPath);
            //}

            //var imagePath = Path.Combine(directoryPath, this.Response.ETag.Replace("\"", "") + "small");

            ////my-bucket.s3.amazonaws.com/thumbs/path/to/file.png/"78skfghsfsfkgjh"small.png

            //if (!File.Exists(imagePath)) {
            //    var thumb = new WebImage(Response.ResponseStream).Resize(200, 200);
            //    _thumbnail = thumb.Save(imagePath, "png").FileName;
            //}
            Debug.WriteLine("Making a request to Amazon: Open");

            return this.Response.ResponseStream;

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonS3VirtualFile"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="virtualPath">The virtual path.</param>
        public AmazonS3VirtualFile(AmazonS3VirtualPathProvider provider, string virtualPath)
            : base(virtualPath) {
            _provider = provider;
            _virtualPath = virtualPath;
            this._client = AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config
                                                                     {
                                                                         ServiceURL = "s3.amazonaws.com",
                                                                         CommunicationProtocol = Protocol.HTTP
                                                                     });
        }
    }
}