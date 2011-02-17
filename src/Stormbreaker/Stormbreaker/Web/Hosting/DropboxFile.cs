using System;
using System.IO;
using System.Web.Hosting;

namespace Stormbreaker.Web.Hosting {
    public class DropboxFile : VirtualFile {
        public DropboxFile(string virtualPath) : base(virtualPath) {}
        public override Stream Open() {
            throw new NotImplementedException();
        }
    }
}