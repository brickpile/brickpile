using RestSharp;

namespace Stormbreaker.Dropbox.Helpers {
    public interface IRequestHelper {
        RestRequest CreateMetadataRequest(string path);
        RestRequest CreateGetFileRequest(string path);
        //RestRequest CreateUploadFileRequest(string path, string filename, byte[] fileData);
        //RestRequest CreateDeleteFileRequest(string path);
        //RestRequest CreateCopyFileRequest(string fromPath, string toPath);
        //RestRequest CreateMoveFileRequest(string fromPath, string toPath);
        RestRequest CreateLoginRequest(string apiKey, string email, string password);
        //RestRequest CreateNewAccountRequest(string apiKey, string email, string firstName, string lastName, string password);
        //RestRequest CreateAccountInfoRequest();
        //RestRequest CreateCreateFolderRequest(string path);
    }
}