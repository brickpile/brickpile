using BrickPile.UI;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Startup))]
namespace BrickPile.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            InitialiseBootstrapper();
        }
    }
}