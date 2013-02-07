/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System.Collections.Generic;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
using BrickPile.UI.Models;
using BrickPile.UI.Web.Hosting;
using Raven.Client;

namespace BrickPile.UI.Controllers {
    public class LibraryController : Controller {

        public ActionResult OpenBucket() {

            //var request = new ListObjectsRequest { BucketName = "KloojedDump" };

            //var response = _client.ListObjects(request);

            //var assets = new List<AssetModel>(response.S3Objects.Count);

            var directory = HostingEnvironment.VirtualPathProvider.GetDirectory("~/s3/Images/");

            foreach (S3Object file in directory.Files) {


                //if (s3Object.Size == 0)
                //    continue;

                var path = string.Concat("http://s3.amazonaws.com/",
                                         VirtualPathUtility.AppendTrailingSlash(((AmazonS3VirtualPathProvider)HostingEnvironment.VirtualPathProvider).BucketName),
                                         file.Key);

                //assets.Add(new AssetModel
                //{
                //    Name = s3Object.Key,
                //    FileSize = s3Object.Size,
                //    Path = path,
                //    LastModified = s3Object.LastModified,
                //    //Class = selectedAsset.Equals(path) ? "selected" : "unselected"

                //});
            }
            //var viewModel = new FileBrowserModel
            //                    {
            //                        Assets = assets,
            //                        BackAction = "Index",
            //                    };

            return new JsonResult()
                       {
                           Data = directory.Files,JsonRequestBehavior = JsonRequestBehavior.AllowGet
                       };
            //return PartialView("_FileBrowser", viewModel);
        }

        //public ActionResult Save() {

        //    var path = Request.QueryString["image"];
        //    if (!string.IsNullOrEmpty(path)) {

        //        using(var client = new WebClient()) {
        //            using(var stream = new MemoryStream(client.DownloadData(new Uri(path)))) {
        //                var request = new PutObjectRequest();
        //                request.WithBucketName(_settings.BucketName)
        //                    .WithCannedACL(S3CannedACL.PublicRead)
        //                    .WithContentType(MimeMapping.GetMimeMapping(path))
        //                    .WithKey("Images/" + Request.QueryString["title"] + Path.GetExtension(path)).InputStream = stream;

        //                _client.PutObject(request);                        
        //            }
        //        }
        //    }

        //    return View();
        //}

        public ActionResult Index() {
            //ViewBag.Class = "dashboard";

            //var request = new ListObjectsRequest { BucketName = _settings.BucketName };
            //var model = _client.ListObjects(request);

            //return View(model.S3Objects);

            return new EmptyResult();
        }

        //public LibraryController(IDocumentSession session) {
        //    _settings = session.Load<Settings>("brickpile/settings");
        //    _client = new AmazonS3Client(_settings.AwsAccessKey, _settings.AwsSecretAccessKey);
        //}
    }
}
