using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using Raven.Client;
using Stormbreaker.Dashboard.Models;
using Stormbreaker.Models;
using Stormbreaker.Web;
using System;

namespace Stormbreaker.Dashboard.Controllers {
    public class LibraryController : Controller {
        private readonly AmazonS3 _client;
        private readonly Settings _settings;
        public ActionResult OpenBucket() {

            var request = new ListObjectsRequest { BucketName = _settings.BucketName };
            var response = _client.ListObjects(request);
            var assets = new List<AssetModel>(response.S3Objects.Count);
            foreach (var s3Object in response.S3Objects) {
                if(s3Object.Size == 0)
                    continue;

                var path = string.Concat("http://s3.amazonaws.com/",
                                         VirtualPathUtility.AppendTrailingSlash(s3Object.BucketName),
                                         s3Object.Key);

                assets.Add(new AssetModel
                               {
                                   Name = s3Object.Key,
                                   FileSize = s3Object.Size,
                                   Path = path,
                                   LastModified = s3Object.LastModified,
                                   //Class = selectedAsset.Equals(path) ? "selected" : "unselected"

                               });                
            }
            
            return PartialView("_bucket", assets);

        }

        public ActionResult Save() {

            var path = Request.QueryString["image"];
            if (!string.IsNullOrEmpty(path)) {

                using(var client = new WebClient()) {
                    using(var stream = new MemoryStream(client.DownloadData(new Uri(path)))) {
                        var request = new PutObjectRequest();
                        request.WithBucketName(_settings.BucketName)
                            .WithCannedACL(S3CannedACL.PublicRead)
                            .WithContentType(MimeMapping.GetMimeMapping(path))
                            .WithKey("Images/" + Request.QueryString["title"] + Path.GetExtension(path)).InputStream = stream;

                        _client.PutObject(request);                        
                    }
                }
            }

            return View();
        }

        public ActionResult Index() {
            ViewBag.Class = "dashboard";

            var request = new ListObjectsRequest { BucketName = _settings.BucketName };
            var model = _client.ListObjects(request);

            return View(model.S3Objects);
        }

        public LibraryController(IDocumentSession session) {
            _settings = session.Load<Settings>("stormbreaker/settings");
            _client = new AmazonS3Client(_settings.AwsAccessKey, _settings.AwsSecretAccessKey);
        }
    }
}
