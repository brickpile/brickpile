/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Raven.Abstractions.Data;
using Raven.Database.Plugins;
using Raven.Json.Linq;

namespace BrickPile.Raven.Plugins.Triggers {
    /// <summary>
    /// 
    /// </summary>
    public class UpdateTrigger : AbstractPutTrigger {
        /// <summary>
        /// Called when [put].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="document">The document.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="transactionInformation">The transaction information.</param>
        public override void OnPut(string key, RavenJObject document, RavenJObject metadata, TransactionInformation transactionInformation) {

            if (key.StartsWith("Raven/",true,CultureInfo.InvariantCulture)) // we don't deal with system documents
                return;

            if (TriggerContext.IsInTriggerContext)
                return;

            using (TriggerContext.Enter()) {
                var meta = document["MetaData"] as RavenJObject; 
                if(meta != null) {
                    var slug = meta.Value<string>("Slug");
                    RavenJToken parent;
                    if (document.TryGetValue("Parent", out parent) && parent.Type != JTokenType.Null) {
                        var parentId = parent.Value<string>("Id");
                        var parentDocument = Database.Get(parentId, transactionInformation);
                        var parentUrl = parentDocument.DataAsJson.Value<JObject>("Metadata").Value<string>("Url");
                        if (parentUrl != null) {
                            meta["Url"] = string.Format("{0}/{1}", parentUrl, slug);
                            base.OnPut(key, document, metadata, transactionInformation);
                            return;
                        }
                    }
                    meta["Url"] = slug;
                }
            }
            base.OnPut(key, document, metadata, transactionInformation);
        }
        /// <summary>
        /// Afters the put.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="document">The document.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="etag">The etag.</param>
        /// <param name="transactionInformation">The transaction information.</param>
        public override void AfterPut(string key, RavenJObject document, RavenJObject metadata, Guid etag, TransactionInformation transactionInformation) {

            if (key.StartsWith("Raven/",true,CultureInfo.InvariantCulture)) // we don't deal with system documents
                return;

            if (TriggerContext.IsInTriggerContext)
                return;

            using (TriggerContext.Enter()) {
                UpdateChildren(key,document,transactionInformation);
            }

            base.AfterPut(key, document, metadata, etag, transactionInformation);
        }
        /// <summary>
        /// Updates the children.
        /// </summary>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="parentDocument">The parent document.</param>
        /// <param name="transactionInformation">The transaction information.</param>
        public void UpdateChildren(string parentKey, RavenJObject parentDocument, TransactionInformation transactionInformation) {

            var childrenQuery = new IndexQuery
            {
                Query = "Id:" + parentKey
            };

            var queryResult = Database.Query("Documents/ByParent", childrenQuery);

            if (queryResult.Results.Count > 0) {

                RavenJToken parentMetaData;
                parentDocument.TryGetValue("Metadata", out parentMetaData);

                var parentUrl = parentMetaData.Value<string>("Url");

                foreach (var result in queryResult.Results) {

                    var metadataJObject = result.Value<RavenJObject>("@metadata");
                    if (metadataJObject != null) {

                        var metaData = result["MetaData"] as RavenJObject; 
                        if(metaData != null) {
                            var slug = metaData.Value<string>("Slug");

                            if(string.IsNullOrEmpty(parentUrl)) {
                                metaData["Url"] = slug;
                            }
                            else {
                                metaData["Url"] = string.Format("{0}/{1}", parentUrl, slug);    
                            }
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