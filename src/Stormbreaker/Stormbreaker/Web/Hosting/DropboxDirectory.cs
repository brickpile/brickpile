using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Hosting;

namespace Stormbreaker.Web.Hosting {
    public class DropboxDirectory : VirtualDirectory
    {
        public DropboxDirectory(string virtualPath) : base(virtualPath) { }

        public override IEnumerable Directories
        {
            get
            {
                var directories = new List<DropboxDirectory>();
                return directories;
            }
        }

        public override IEnumerable Files
        {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerable Children
        {
            get { throw new NotImplementedException(); }
        }
    }
}