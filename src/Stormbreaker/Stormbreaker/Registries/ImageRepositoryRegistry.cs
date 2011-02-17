using Stormbreaker.Repositories;
using StructureMap.Configuration.DSL;

namespace Stormbreaker.Registries {
    /// <summary>
    /// Responsible for the registration of <see cref="ImageRepositoryRegistry"/> class.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class ImageRepositoryRegistry : Registry {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRepositoryRegistry"/> class.
        /// </summary>
        public ImageRepositoryRegistry()
        {
            For<IDocumentRepository>().Use<DocumentRepository>();
        }        
    }
}