using System.Linq;
using BrickPile.Core;
using BrickPile.Core.Conventions;
using BrickPile.Core.Hosting;
using BrickPile.Samples.Models;
using BrickPile.Samples.Models.ContentTypes;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace BrickPile.Samples
{
    //public class CustomBootstrapper : DefaultBrickPileBootstrapper
    //{
    //    public override void ConfigureConventions(BrickPileConventions brickPileConventions) {
    //        brickPileConventions.VirtualPathProviderConventions.Register("static",() => new NativeVirtualPathProvider());
    //    }

    //    public override void ConfigureDocumentStore(IDocumentStore documentStore)
    //    {
    //        IndexCreation.CreateIndexes(typeof(CustomBootstrapper).Assembly, documentStore);

    //        // Initialize MiniProfiler
    //        //MvcMiniProfiler.RavenDb.Profiler.AttachTo(documentStore);
    //    }
    //}

    public class MyTransformer : AbstractTransformerCreationTask<Home>
    {
        public MyTransformer()
        {
            TransformResults = homes => from home in homes
                select new
                {
                    CurrentPage = home,
                    Etag = MetadataFor(home)["@etag"]
                };
        }
    }
}