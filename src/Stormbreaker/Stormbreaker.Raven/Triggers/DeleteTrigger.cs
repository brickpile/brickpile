using System;
using Newtonsoft.Json.Linq;
using Raven.Database.Data;
using Raven.Database.Plugins;
using Raven.Http;

namespace Stormbreaker.Raven.Triggers {
    public class DeleteTrigger : AbstractDeleteTrigger {

        public override void OnDelete(string key, TransactionInformation transactionInformation) {

            if (key.StartsWith("Raven/")) // we don't deal with system documents
                return;

            var childrenQuery = new IndexQuery
            {
                Query = "Id:" + key
            };

            var queryResult = Database.Query("Documents/ByParent", childrenQuery);

            if (queryResult.Results.Count > 0) {
                foreach (var result in queryResult.Results) {

                    var metadataJObject = result.Value<JObject>("@metadata");

                    if (metadataJObject != null)
                    {
                        var childId = metadataJObject.Value<string>("@id");

                        var childEtag = metadataJObject.Value<string>("@etag");
                        Database.Delete(childId, Guid.Parse(childEtag), transactionInformation);
                    }
                }
            }

            base.OnDelete(key, transactionInformation);
        }
    }
}