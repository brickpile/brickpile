using System.Linq;
using Newtonsoft.Json.Linq;
using Raven.Database.Plugins;
using Raven.Http;
using Stormbreaker.Models;

namespace Stormbreaker.Raven.Triggers {
    public class UpdateTrigger : AbstractPutTrigger {
        public override void AfterPut(string key, JObject document, JObject metadata, System.Guid etag, TransactionInformation transactionInformation)
        {
            if (key.StartsWith("Raven/")) // we don't deal with system documents
                return;
            if (TriggerContext.IsInTriggerContext)
                return;

            using (TriggerContext.Enter())
            {

                JToken parentReference;

                if (document.TryGetValue("Parent", out parentReference) && parentReference.Type != JTokenType.Null)
                {

                    var parent = Database.Get(parentReference.Value<string>("Id"), transactionInformation);

                    JToken children;
                    if (parent.DataAsJson.TryGetValue("Children", out children)) {

                        JToken metaData;
                        document.TryGetValue("MetaData", out metaData);

                        JToken slug = metaData.Value<string>("Slug");
                        
                        if (children["$values"].Count() > 0) {
                            bool containsDocument = false;
                            foreach (var child in children["$values"])
                            {
                                if (child.Value<string>("Id") == key)
                                {
                                    child["Slug"] = slug;
                                    containsDocument = true;
                                }
                            }
                            if(!containsDocument) {
                                ((JArray)children["$values"]).Add(JObject.FromObject(new DenormalizedReference<IPageModel> { Id = key, Slug = (string) slug }));
                            }
                        }
                        else
                        {
                            ((JArray)children["$values"]).Add(JObject.FromObject(new DenormalizedReference<IPageModel> { Id = key, Slug = (string) slug }));
                        }
                    }
                    Database.Put(parent.Key, parent.Etag, parent.DataAsJson, parent.Metadata, transactionInformation);
                }
            }
            base.AfterPut(key, document, metadata, etag, transactionInformation);
        }
    }
}