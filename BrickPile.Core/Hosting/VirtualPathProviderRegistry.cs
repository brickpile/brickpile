using System.Collections.Generic;
using System.Web.Hosting;

namespace BrickPile.Core.Hosting {
    public class VirtualPathProviderRegistry {
        internal static List<CommonProviderSettings> RegistratedProviders {
            get {
                if(_registratedProviders==null) {
                    _registratedProviders = new List<CommonProviderSettings>();
                }
                return _registratedProviders;
            }
        }

        private static List<CommonProviderSettings> _registratedProviders;
        public static void RegisterProvider(CommonVirtualPathProvider provider, string friendlyName) {
            HostingEnvironment.RegisterVirtualPathProvider(provider);
            RegistratedProviders.Add(new CommonProviderSettings { FriendlyName = friendlyName, VirtualPathRoot = provider.VirtualPathRoot });
        }
        public class CommonProviderSettings {
            public string VirtualPathRoot { get; set; }
            public string FriendlyName { get; set; }
        }
    }

}
