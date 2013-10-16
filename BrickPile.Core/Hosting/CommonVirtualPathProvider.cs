using System.Web.Hosting;

namespace BrickPile.Core.Hosting {
    public abstract class CommonVirtualPathProvider : VirtualPathProvider {

        /// <summary>
        /// Gets the virtual path root.
        /// </summary>
        public abstract string VirtualPathRoot { get; }
    }
}
