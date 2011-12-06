using Raven.Client.Embedded;

namespace BrickPile.Docs.csharp {
    public class Configuration {
        public void Test() {
            #region Test region
            var store = new EmbeddableDocumentStore();
            using(var session = store.OpenSession()) {
                // Foo                
            }
            #endregion
        }
    }
}
