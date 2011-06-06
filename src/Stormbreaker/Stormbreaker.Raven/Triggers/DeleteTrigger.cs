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
using Newtonsoft.Json.Linq;
using Raven.Abstractions.Data;
using Raven.Database.Plugins;

namespace Stormbreaker.Raven.Triggers {
    /// <summary>
    /// 
    /// </summary>
    public class DeleteTrigger : AbstractDeleteTrigger {
        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="transactionInformation">The transaction information.</param>
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