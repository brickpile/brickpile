using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LitS3;

namespace Stormbreaker.Repositories {
    public class S3Repository : IS3Repository {
        private readonly S3Service _s3Service;

        public IQueryable<T> FindAllBuckets<T>() {
            return _s3Service.GetAllBuckets()
                .Cast<T>()
                .AsQueryable();
        }

        public IQueryable<T> FindAllObjects<T>(string bucketName) {
            return _s3Service.ListAllObjects(bucketName)
                .Cast<T>()
                .AsQueryable();
        }

        public IQueryable<T> ListAllOjbects<T>(string bucketName,string directory) {
            var list = new List<ListEntry>();
            _s3Service.ForEachObject(bucketName, directory, list.Add);
            return list.Cast<T>().AsQueryable();
        }

        public S3Repository() {
            _s3Service = new S3Service
                             {
                                 AccessKeyID = "AKIAJ5XX23HO3MK5GZMQ",
                                 SecretAccessKey = "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"
                             };
        }
    }
}
