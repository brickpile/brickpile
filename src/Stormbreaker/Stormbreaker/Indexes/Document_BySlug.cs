using System.Linq;
using Raven.Client.Indexes;
using Raven.Database.Indexing;
using Stormbreaker.Models;

namespace Stormbreaker.Indexes {
    public class Document_BySlug : AbstractIndexCreationTask
    {
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition<IDocument>
            {
                Map = documents => from document in documents
                                select new { document.Slug }
            }
            .ToIndexDefinition(DocumentStore.Conventions);
        }        
    }
}