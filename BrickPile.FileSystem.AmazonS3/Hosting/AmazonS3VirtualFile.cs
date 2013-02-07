using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BrickPile.Core.Hosting;

namespace BrickPile.FileSystem.AmazonS3.Hosting {

    public class AmazonS3VirtualFile : CommonVirtualFile {

        private readonly AmazonS3VirtualPathProvider _provider;
        private readonly Amazon.S3.AmazonS3 _client ;
        private readonly string _virtualPath;

        /// <summary>
        /// Gets the local path.
        /// </summary>
        public override string Url {
            get {
                var provider = this._provider;
                var baseUrl = VirtualPathUtility.AppendTrailingSlash(provider.HostName);
                return string.Format("{0}{1}", baseUrl, this.VirtualPath.Replace(provider.VirtualPathRoot, string.Empty));
            }
        }
        /// <summary>
        /// Gets the local path.
        /// </summary>
        public override string LocalPath {
            get { return this.VirtualPath; }
        }
        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open() {
            try {
                var request = new GetObjectRequest().WithBucketName(_provider.BucketName).WithKey(this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty));
                var response = this._client.GetObject(request);
                return response.ResponseStream;
            } catch (AmazonS3Exception exception) {
                if (exception.StatusCode == HttpStatusCode.NotFound) {
                    throw new FileNotFoundException();
                }
            }
            return Stream.Null;
        }
        /// <summary>
        /// Opens the specified file mode.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <returns></returns>
        public override Stream Open(FileMode fileMode) {

            byte[] buffer = new byte[_provider.MaxRequestLength];

            S3CopyMemoryStream s3CopyStream = new S3CopyMemoryStream(this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty), buffer, (AmazonS3Client) this._client).WithS3CopyFileStreamEvent(CreateMultiPartS3Blob);

            return s3CopyStream;
        }

        /// <summary>
        /// Deletes the image including all thumbnails
        /// </summary>
        public override void Delete() {
            try {
                var request = new DeleteObjectRequest().WithBucketName(_provider.BucketName).WithKey(this._virtualPath.Replace(_provider.VirtualPathRoot, string.Empty));
                this._client.DeleteObject(request);
            } catch (AmazonS3Exception exception) {
                if (exception.StatusCode == HttpStatusCode.NotFound) {
                    throw new FileNotFoundException();
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonS3VirtualFile"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="virtualPath">The virtual path.</param>
        public AmazonS3VirtualFile(AmazonS3VirtualPathProvider provider, string virtualPath) : base(virtualPath) {
            _provider = provider;
            _virtualPath = virtualPath;
            this._client = AWSClientFactory.CreateAmazonS3Client(new AmazonS3Config
                                                                     {
                                                                         ServiceURL = "s3.amazonaws.com",
                                                                         CommunicationProtocol = Protocol.HTTP
                                                                     });
        }

        static internal void CreateMultiPartS3Blob(AmazonS3Client client, string key, S3CopyMemoryStream stream) {

            if (stream.InitiatingPart) {

                InitiateMultipartUploadRequest initiateMultipartUploadRequest =
                    new InitiateMultipartUploadRequest()
                        .WithBucketName("static.getbrickpile.com")
                        .WithCannedACL(S3CannedACL.PublicRead)
                        .WithKey(key);

                InitiateMultipartUploadResponse initiateResponse =
                    client.InitiateMultipartUpload(initiateMultipartUploadRequest);
                stream.UploadPartId = initiateResponse.UploadId;

            }

            stream.Position = 0;

            UploadPartRequest uploadPartRequest =
                new UploadPartRequest()
                    .WithBucketName("static.getbrickpile.com")
                    .WithKey(key)
                    .WithPartNumber(stream.WriteCount)
                    .WithPartSize(stream.Position)
                    .WithUploadId(stream.UploadPartId)
                    .WithInputStream(stream) as UploadPartRequest;

            UploadPartResponse response = client.UploadPart(uploadPartRequest);
            PartETag etag = new PartETag(response.PartNumber, response.ETag);
            stream.PartETagCollection.Add(etag);
            
            if (stream.EndOfPart) {

                CompleteMultipartUploadRequest completeMultipartUploadRequest =
                    new CompleteMultipartUploadRequest()
                        .WithBucketName("static.getbrickpile.com")
                        .WithKey(key)
                        .WithPartETags(stream.PartETagCollection)
                        .WithUploadId(stream.UploadPartId);
                
                CompleteMultipartUploadResponse completeMultipartUploadResponse = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                string loc = completeMultipartUploadResponse.Location;

            }
        }
    }


    public class S3CopyMemoryStream : MemoryStream {

        private string _key;
        
        private byte[] _buffer;
        private readonly AmazonS3Client _client;
        public StartUploadS3CopyFileStreamEvent StartUploadS3FileStreamEvent { get; set; }

        public int WriteCount { get; private set; }
        public bool EndOfPart { get; private set; }
        public bool InitiatingPart { get; private set; }
        public string UploadPartId { get; set; }
        public List<PartETag> PartETagCollection { get; set; }
        protected Queue<int> QueueWithIncompleteParts { get; set; }

        public S3CopyMemoryStream WithS3CopyFileStreamEvent(StartUploadS3CopyFileStreamEvent doing) {
            S3CopyMemoryStream s3CopyStream = new S3CopyMemoryStream(this._key, this._buffer, _client);

            s3CopyStream.StartUploadS3FileStreamEvent = AmazonS3VirtualFile.CreateMultiPartS3Blob;

            return s3CopyStream;
        }

        public S3CopyMemoryStream(string key, byte[] buffer, AmazonS3Client client) {

            if (buffer.LongLength > int.MaxValue)
                throw new ArgumentException("The length of the buffer may not be longer than int.MaxValue", "buffer");

            InitiatingPart = true;
            EndOfPart = false;
            WriteCount = 1;
            PartETagCollection = new List<PartETag>();

            _buffer = buffer;
            _client = client;
            _key = key;
        }
        

        public delegate void StartUploadS3CopyFileStreamEvent(AmazonS3Client client, string key, S3CopyMemoryStream stream);

        public override bool CanSeek {
            get { return false; }
        }

        public override void Write(byte[] array, int offset, int count) {

            base.Write(array, offset, count);

            if (Position >= 6379156) {
                if (WriteCount > 1)
                    InitiatingPart = false;
                StartUploadS3FileStreamEvent.Invoke(_client, _key, this);

                WriteCount++;
                base.Flush();
                base.Seek(0, SeekOrigin.Begin);
                base.Flush();

            }
        }

        public override void Close() {
            if (WriteCount > 1)
                InitiatingPart = false;

            EndOfPart = true;
            StartUploadS3FileStreamEvent.Invoke(_client, _key, this);
            base.Close();
        }

    }
}