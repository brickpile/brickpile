using System;
using System.Web;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace BrickPile.Core.Routing.Trie
{
    public class RouteResolverTrie : IRouteResolverTrie
    {
        private readonly IDocumentStore documentStore;
        private readonly Func<HttpContextBase> httpContext;

        /// <summary>
        ///     Gets the context base.
        /// </summary>
        /// <value>
        ///     The context base.
        /// </value>
        protected HttpContextBase ContextBase
        {
            get { return this.httpContext.Invoke(); }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RouteResolverTrie" /> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="documentStore">The document store.</param>
        public RouteResolverTrie(Func<HttpContextBase> httpContext, IDocumentStore documentStore)
        {
            this.httpContext = httpContext;
            this.documentStore = documentStore;
        }

        /// <summary>
        ///     Loads the trie.
        /// </summary>
        /// <returns></returns>
        public Trie LoadTrie()
        {
            if (this.ContextBase.Items.Contains("brickpile:trie"))
            {
                return this.ContextBase.Items["brickpile:trie"] as Trie;
            }

            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var trie = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                //TODO check if we can make a document readonly
                //session.Advanced.MarkReadOnly(trie);
                this.ContextBase.Items["brickpile:trie"] = trie;
                return trie;
            }
        }
    }
}