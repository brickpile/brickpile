using System.Linq;
using Raven.Client.Indexes;
using Raven.Database.Indexing;
using Stormbreaker.Models;

namespace Stormbreaker.Indexes {
    class Document_ByUrl : AbstractIndexCreationTask {
        public override IndexDefinition CreateIndexDefinition() {
            return new IndexDefinition<IPageModel>
            {
                Map = documents => from document in documents
                                   select new
                                              {
                                                  document.MetaData.Url                                                  
                                              }
            }
            .ToIndexDefinition(Conventions);
        }
    }
}
