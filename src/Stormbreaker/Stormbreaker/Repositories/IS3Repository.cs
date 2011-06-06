using System.IO;
using System.Linq;

namespace Stormbreaker.Repositories {
    public interface IS3Repository {
        IQueryable<T> FindAllBuckets<T>();
        IQueryable<T> FindAllObjects<T>(string bucketName);
        IQueryable<T> ListAllOjbects<T>(string bucketName, string directory);
        string GetUrl(string bucketName, string key);
        Stream GetObjectStream(string bucketName, string key);
    }
}