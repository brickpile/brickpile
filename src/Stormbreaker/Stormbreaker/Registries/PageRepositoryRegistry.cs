using Stormbreaker.Repositories;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// Responsible for the registration of <see cref="PageRepository"/> class.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PageRepositoryRegistry : Registry {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRepositoryRegistry"/> class.
        /// </summary>
        public PageRepositoryRegistry()
        {
            //For<IPageRepository>().Use<PageRepository>();
        }
    }
}