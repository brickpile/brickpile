using System;
using System.Web.Hosting;

namespace BrickPile.Core.Hosting
{
    public abstract class CommonVirtualPathProvider : VirtualPathProvider
    {
        /// <summary>
        ///     Gets the physical path.
        /// </summary>
        public virtual string PhysicalPath
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Gets the virtual path root.
        /// </summary>
        public virtual string VirtualPathRoot
        {
            get { throw new NotImplementedException(); }
        }
    }
}