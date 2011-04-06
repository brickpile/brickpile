using System;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Raven.Database.Data;
using Raven.Database.Plugins;
using Raven.Http;

namespace Stormbreaker.Raven.Triggers {
    public class UpdateTrigger : AbstractPutTrigger {

        public override void OnPut(string key, JObject document, JObject metadata, TransactionInformation transactionInformation) {
            if (key.StartsWith("Raven/",true,CultureInfo.InvariantCulture)) // we don't deal with system documents
                return;

            if (TriggerContext.IsInTriggerContext)
                return;

            using (TriggerContext.Enter()) {
                JToken meta;
                document.TryGetValue("MetaData", out meta);
                var slug = meta.Value<string>("Slug");
                JToken parent;
                if (document.TryGetValue("Parent", out parent) && parent.Type != JTokenType.Null) {
                    var parentId = parent.Value<string>("Id");
                    var parentDocument = Database.Get(parentId, transactionInformation);
                    var parentUrl = parentDocument.DataAsJson.Value<JObject>("MetaData").Value<string>("Url");
                    if (parentUrl != null) {
                        meta["Url"] = new JValue(string.Format("{0}/{1}", parentUrl, slug));
                        base.OnPut(key, document, metadata, transactionInformation);
                        return;
                    }
                }
                meta["Url"] = new JValue(slug);
            }
            base.OnPut(key, document, metadata, transactionInformation);
        }

        public override void AfterPut(string key, JObject document, JObject metadata, System.Guid etag, TransactionInformation transactionInformation) {

            if (key.StartsWith("Raven/",true,CultureInfo.InvariantCulture)) // we don't deal with system documents
                return;

            if (TriggerContext.IsInTriggerContext)
                return;

            using (TriggerContext.Enter()) {
                UpdateChildren(key,document,transactionInformation);
            }

            base.AfterPut(key, document, metadata, etag, transactionInformation);
        }

        public void UpdateChildren(string parentKey, JObject parentDocument, TransactionInformation transactionInformation) {

            var childrenQuery = new IndexQuery
            {
                Query = "Id:" + parentKey
            };

            var queryResult = Database.Query("Documents/ByParent", childrenQuery);

            if (queryResult.Results.Count > 0) {

                JToken parentMetaData;
                parentDocument.TryGetValue("MetaData", out parentMetaData);

                var parentUrl = parentMetaData.Value<string>("Url");

                foreach (var result in queryResult.Results) {

                    var metadataJObject = result.Value<JObject>("@metadata");
                    if (metadataJObject != null) {

                        JToken metaData;
                        result.TryGetValue("MetaData", out metaData);

                        var slug = metaData.Value<string>("Slug");

                        if(string.IsNullOrEmpty(parentUrl)) {
                            metaData["Url"] = new JValue(slug);
                        }
                        else {
                            metaData["Url"] = new JValue(string.Format("{0}/{1}", parentUrl, slug));    
                        }

                        var childEtag = metadataJObject.Value<string>("@etag");
                        var childId = metadataJObject.Value<string>("@id");

                        Database.Put(childId, Guid.Parse(childEtag), result, metadataJObject, transactionInformation);
                        UpdateChildren(childId,result,transactionInformation);
                    }
                }
            }
        }
    }
}