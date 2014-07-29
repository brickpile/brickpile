using Raven.Client;

namespace BrickPile.Core.Extensions
{
    public static class DocumentStoreExtensions
    {
        public static bool Exists(this IDocumentStore documentStore, string key)
        {
            return documentStore.DatabaseCommands.Head(key) != null;
        }
    }
}
