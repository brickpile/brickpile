using System.Linq;
using BrickPile.Domain.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    public class AllAssets : AbstractMultiMapIndexCreationTask<Asset> {
        
        public AllAssets() {

            AddMapForAll<Asset>(assets => from asset in assets
                                          select new
                                          {
                                              asset.Id,
                                              asset.Name,
                                              asset.ContentType,
                                              asset.DateUploaded,
                                              asset.VirtualPath,
                                              asset.ContentLength
                                          });

            Indexes.Add(x => x.ContentType, FieldIndexing.Analyzed);

        }
    }
}