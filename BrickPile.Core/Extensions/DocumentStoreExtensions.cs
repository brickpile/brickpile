using Raven.Client;
using Raven.Client.Documents;

namespace BrickPile.Core.Extensions
{
    /// <summary>
    ///     Provides BrickPile <see cref="IDocumentStore" /> helper methods.
    /// </summary>
    public static class DocumentStoreExtensions
    {
        /// <summary>
        ///     Check if an existing document Id is present in the <see cref="IDocumentStore" />.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        //public static bool Exists(this IDocumentStore documentStore, string key)
        //{
        //    return documentStore.DatabaseCommands.Head(key) != null;
        //}
    }
}