using System;
using RestSharp;
using RestSharp.Deserializers;
using Stormbreaker.Dropbox.Authenticators;
using Stormbreaker.Dropbox.Helpers;
using Stormbreaker.Dropbox.Models;

namespace Stormbreaker.Dropbox {
    [Obsolete("Use repository", true)]
    public class DropboxClient {
        private const string Version = "0";

        private UserLogin _userLogin;

        private string _apiKey;
        private string _appsecret;

        private RestClient _restClient;
        private RequestHelper _requestHelper;
        /// <summary>
        /// The number of requests that have been made by the current Client instance
        /// </summary>
        public int RequestCount { get; set; }
        /// <summary>
        /// The total Bytes returned from the requests made by the current Client instance
        /// </summary>
        public long DataCount { get; set; }

        /// <summary>
        /// Default Constructor for the DropboxClient
        /// </summary>
        /// <param name="apiKey">The Api Key to use for the Dropbox Requests</param>
        public DropboxClient(string apiKey, string appSecret)
        {
            _apiKey = apiKey;
            _appsecret = appSecret;

            LoadClient();
        }

        //public DropboxClient(string apiKey, string appSecret, string userToken, string userSecret)
        //{
        //    _apiKey = apiKey;
        //    _appsecret = appSecret;

        //    _userLogin = new UserLogin { Token = userToken, Secret = userSecret };

        //    LoadClient();
        //}

        private void LoadClient()
        {
            _restClient = new RestClient("https://api.getdropbox.com");
            _restClient.ClearHandlers();
            _restClient.AddHandler("*", new JsonDeserializer());
            //probly not needed...
            RequestCount = 0;
            DataCount = 0;
            _requestHelper = new RequestHelper(Version);
        }

        public UserLogin Login(string email, string password) {
            _restClient.BaseUrl = "https://api.getdropbox.com";

            var request = _requestHelper.CreateLoginRequest(_apiKey, email, password);

            var response = _restClient.Execute<UserLogin>(request);

            _userLogin = response.Data;

            return _userLogin;
        }
        /// <summary>
        /// Gets MetaData for the root folder.
        /// </summary>
        /// <returns></returns>
        public MetaData GetMetaData() {
            return GetMetaData(string.Empty);
        }

        /// <summary>
        /// Gets MetaData for a File or Folder. For a folder this includes its contents. For a file, this includes details such as file size.
        /// </summary>
        /// <param name="path">The path of the file or folder</param>
        /// <returns></returns>
        public MetaData GetMetaData(string path) {
            //This has to be here as Dropbox change their base URL between calls
            _restClient.BaseUrl = "https://api.dropbox.com";
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, _userLogin.Token, _userLogin.Secret);

            var request = _requestHelper.CreateMetadataRequest(path);

            var response = _restClient.Execute<MetaData>(request);

            return response.Data;
        }

        public byte[] GetFile(string path) {

            if (!path.StartsWith("/")) path = "/" + path;

            //This has to be here as Dropbox change their base URL between calls
            _restClient.BaseUrl = "https://api-content.dropbox.com";
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, _userLogin.Token, _userLogin.Secret);

            var request = _requestHelper.CreateGetFileRequest(path);

            var responseData = _restClient.DownloadData(request);

            return responseData;
        }        
    }
}