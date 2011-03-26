using Stormbreaker.Dropbox.Models;

namespace Stormbreaker.Repositories {
    public interface IDropboxRepository {

        MetaData GetMetaData();
        MetaData GetMetaData(string path);
        byte[] GetFile(string path);

    }
}