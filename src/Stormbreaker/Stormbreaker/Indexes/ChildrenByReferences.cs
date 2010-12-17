using System.Linq;
using Raven.Client.Indexes;
using Raven.Database.Indexing;
using Stormbreaker.Models;

namespace Stormbreaker.Indexes {
    public class ChildrenByReferences : AbstractIndexCreationTask
    {
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition<IContentItem>
            {
                Map = docs => from doc in docs
                              select new
                              {
                                  doc.StructureInfo
                              }
            }
            .ToIndexDefinition(DocumentStore.Conventions);
        }
    }
}