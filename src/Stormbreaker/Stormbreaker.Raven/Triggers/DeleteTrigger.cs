using System;
using Newtonsoft.Json.Linq;
using Raven.Database.Data;
using Raven.Database.Plugins;
using Raven.Database.Queries;
using Raven.Http;

namespace Stormbreaker.Raven.Triggers {
    public class DeleteTrigger : AbstractDeleteTrigger {

        public override void OnDelete(string key, TransactionInformation transactionInformation) {

            if (key.StartsWith("Raven/")) // we don't deal with system documents
                return;

            var childrenQuery = new IndexQuery
                                    {
                                        Query = "Parent.Id:" + key
                                    };

            var queryResult = Database.ExecuteDynamicQuery("Documents", childrenQuery);
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

            // Remove parent reference
            RemoveParentReference(key,transactionInformation);

            base.OnDelete(key, transactionInformation);
        }

        private void RemoveParentReference(string key, TransactionInformation transactionInformation) {

            var document = Database.Get(key, transactionInformation);

            JToken parentReference;

            if (document.DataAsJson.TryGetValue("Parent", out parentReference) &&
                parentReference.Type != JTokenType.Null) {
                var parent = Database.Get(parentReference.Value<string>("Id"), transactionInformation);

                JToken children;
                if (parent.DataAsJson.TryGetValue("Children", out children)) {
                    foreach (var child in children["$values"]) {
                        if (child.Value<string>("Id") == key) {
                            ((JArray) children["$values"]).Remove(child);
                            Database.Put(parent.Key, parent.Etag, parent.DataAsJson, parent.Metadata, transactionInformation);
                        }
                    }

                }
            }
        }
    }
}