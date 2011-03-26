using System.Configuration;
using RestSharp;
using Stormbreaker.Dropbox.Helpers;
using Stormbreaker.Dropbox.Models;
using Stormbreaker.Dropbox.Authenticators;

namespace Stormbreaker.Repositories {
    public class DropboxRepository : IDropboxRepository {

        private UserLogin _userLogin;
        private readonly RestClient _client;
        private readonly string _apiKey = ConfigurationManager.AppSettings["apiKey"];
        private readonly string _appSecret = ConfigurationManager.AppSettings["appSecret"];
        private readonly IRequestHelper _requestHelper;

        /// <summary>
        /// The number of requests that have been made by the current Client instance
        /// </summary>
        public int RequestCount { get; set; }
        /// <summary>
        /// The total Bytes returned from the requests made by the current Client instance
        /// </summary>
        public long DataCount { get; set; }
        /// <summary>
        /// Authenticates the Dropbox user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        private void Authenticate(string email, string password) {
            var request = _requestHelper.CreateLoginRequest(_apiKey, email, password);
            var response = _client.Execute<UserLogin>(request);
            _userLogin = response.Data;
        }
        /// <summary>
        /// Gets the meta data.
        /// </summary>
        /// <returns></returns>
        public MetaData GetMetaData() {
            return GetMetaData(string.Empty);
        }
        /// <summary>
        /// Gets the meta data.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public MetaData GetMetaData(string path) {
            _client.BaseUrl = "https://api.dropbox.com";
            _client.Authenticator = new OAuthAuthenticator(_client.BaseUrl, _apiKey, _appSecret, _userLogin.Token, _userLogin.Secret);
            var request = _requestHelper.CreateMetadataRequest(path);
            var response = _client.Execute<MetaData>(request);
            return response.Data;
        }
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public byte[] GetFile(string path) {
            if (!path.StartsWith("/")) path = "/" + path;
            //This has to be here as Dropbox change their base URL between calls
            _client.BaseUrl = "https://api-content.dropbox.com";
            _client.Authenticator = new OAuthAuthenticator(_client.BaseUrl, _apiKey, _appSecret, _userLogin.Token, _userLogin.Secret);
            var request = _requestHelper.CreateGetFileRequest(path);
            var responseData = _client.DownloadData(request);
            return responseData;            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DropboxRepository"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="requestHelper">The request helper.</param>
        public DropboxRepository(IRestClient client, IRequestHelper requestHelper) {
            _client = (RestClient) client;
            _requestHelper = requestHelper;
            Authenticate(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
        }
    }
}