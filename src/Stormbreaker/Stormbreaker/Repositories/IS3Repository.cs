using System.Linq;
using LitS3;

namespace Stormbreaker.Repositories {
    public interface IS3Repository {
        IQueryable<T> FindAllBuckets<T>();
        IQueryable<T> FindAllObjects<T>(string bucketName);
        IQueryable<T> ListAllOjbects<T>(string bucketName, string directory);
    }
}