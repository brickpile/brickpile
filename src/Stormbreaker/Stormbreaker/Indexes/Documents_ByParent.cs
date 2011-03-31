using System.Linq;
using Raven.Client.Indexes;
using Raven.Database.Indexing;
using Stormbreaker.Models;

namespace Stormbreaker.Indexes {
    /// <summary>
    /// Used for registrations of an index in RavenDB
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class Documents_ByParent : AbstractIndexCreationTask
    {
        /// <summary>
        /// Creates the index definition for getting a page model by parent id
        /// </summary>
        /// <returns></returns>
        public override IndexDefinition CreateIndexDefinition() {
            return new IndexDefinition<IPageModel>
            {
                Map = documents => from document in documents
                                   select new { document.Parent.Id }
            }
            .ToIndexDefinition(Conventions);
        }
    }
}