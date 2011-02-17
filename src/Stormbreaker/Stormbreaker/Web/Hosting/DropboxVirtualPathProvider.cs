using System.Web.Hosting;

namespace Stormbreaker.Web.Hosting {
    public class DropboxVirtualPathProvider : VirtualPathProvider
    {
        private const string _apiKey = "ksmxt4l46011hxt";
        private const int _version = 0;
        private UserLogin _userLogin;
        private readonly DropboxDirectory _root;

        public override VirtualFile GetFile(string virtualPath) {
            //_userLogin = Login("marcus@meridium.se", "pkrMum");
            
            return base.GetFile(virtualPath);
        }

        //public UserLogin Login(string email, string password)
        //{
            //var restClient = new RestClient("https://api.getdropbox.com");
            //var request = new RestRequest(Method.GET);
            //request.Resource = "{version}/token";
            //request.AddParameter("version", _version, ParameterType.UrlSegment);

            //request.AddParameter("oauth_consumer_key", _apiKey);

            //request.AddParameter("email", email);
            //request.AddParameter("password", password);

            //var response = restClient.Execute<UserLogin>(request);

            //_userLogin = response.Data;

            //return response.Data;
        //}

        public override bool FileExists(string virtualPath)
        {
            //if (virtualPath.StartsWith(this._root.VirtualPath, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    return (this.FindFile(virtualPath) != null);
            //}
            return base.Previous.FileExists(virtualPath);
        }

    }
    public class UserLogin
    {
        public string Token { get; set; }
        public string Secret { get; set; }
    }

}