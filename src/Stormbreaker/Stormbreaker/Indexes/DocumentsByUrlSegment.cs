using System.Linq;
using Raven.Client.Indexes;
using Raven.Database.Indexing;
using Stormbreaker.Models;

namespace Stormbreaker.Indexes {
    public class DocumentsByUrlSegment : AbstractIndexCreationTask {
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        #region public override IndexDefinition CreateIndexDefinition()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition<ContentItem>
            {
                Map = docs => from doc in docs
                              select new
                              {
                                  doc.MetaData.UrlSegment
                              }
            }
            .ToIndexDefinition(DocumentStore.Conventions);
        }
        #endregion
    }
}