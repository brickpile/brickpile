using System.Collections;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;

namespace Stormbreaker.Web.Hosting {
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DropboxVirtualDirectory : VirtualDirectory {

        private readonly string _virtualPath;
        private readonly DropboxVirtualPathProvider _provider;

        private readonly ArrayList _directories = new ArrayList();
        public override IEnumerable Directories {
            get { return _directories; }
        }

        private readonly ArrayList _files = new ArrayList();
        public override IEnumerable Files {
            get { return _files; }
        }

        public override IEnumerable Children {
            get {
                var list = Directories.Cast<object>().ToList();
                list.AddRange(Files.Cast<object>());
                return list;
            }
        }

        public DropboxVirtualDirectory(string virtualPath, DropboxVirtualPathProvider provider) : base(virtualPath) {
            _virtualPath = virtualPath;
            _provider = provider;
        }
    }
}