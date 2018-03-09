using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using RavenDB.AspNet.Identity;

namespace RavenDB.AspNet.Identity.Tests
{
    public abstract class BaseTest
    {
        protected EmbeddableDocumentStore NewDocStore()
        {
            var embeddedStore = new EmbeddableDocumentStore
            {
                Configuration =
                {
                    RunInMemory = true,
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true
                }
            };

            embeddedStore.Initialize();

            new RavenDocumentsByEntityName().Execute(embeddedStore);

            return embeddedStore;
        }

        protected UserStore<TUser> NewUserStore<TUser>(IDocumentStore docStore) where TUser : IdentityUser
        {
            return new UserStore<TUser>(docStore.OpenAsyncSession);
        }

        protected UserManager<TUser> NewUserManager<TUser>(IDocumentStore docStore) where TUser : IdentityUser
        {
            return new UserManager<TUser>(new UserStore<TUser>(docStore.OpenAsyncSession));
        }
    }
}
