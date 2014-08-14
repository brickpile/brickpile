using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using BrickPile.Core;
using BrickPile.Core.Exceptions;
using BrickPile.Core.Extensions;
using BrickPile.Core.Routing.Trie;
using BrickPile.Tests.Fakes;
using FakeItEasy;
using Raven.Client;
using Raven.Tests.Helpers;
using StructureMap;
using Xunit;
using IPage = BrickPile.Core.IPage;

namespace BrickPile.Tests.Infrastructure.Listeners
{
    public class StoreListenerTests : RavenTestBase
    {        
        public class BeforeStore : RavenTestBase {

            private IDocumentStore SetupContext() {
                
                HttpContext.Current = null;
                RouteTable.Routes.Clear();

                var store = NewDocumentStore();

                var bootStrapper = new FakeBootstrapper();
                var field = typeof(DefaultBrickPileBootstrapper).GetField("DocStore",
                    BindingFlags.Static | BindingFlags.NonPublic);

                field.SetValue(null, new Lazy<IDocumentStore>(() => store));
                bootStrapper.Initialise();

                HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null))
                {
                    User = A.Fake<IPrincipal>()
                };
                return store;
            }

            [Fact]
            public void Can_Save_Start_Page()
            {
                // Given
                IPage page, draft;
                Trie structureInfo;
                var store = SetupContext();
               
                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage(), StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<IPage>("FakePages/1");
                    draft = session.Load<IPage>("FakePages/1/Draft");
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.NotNull(structureInfo.RootNode.PageId);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);

                Assert.NotNull(page);
                Assert.Null(draft);

                Assert.False(page.Metadata.IsPublished);
                Assert.Equal(default(DateTime?), page.Metadata.Published);
            }

            [Fact]
            public void Can_Publish_Start_Page()
            {
                // Given
                IPage page, draft;
                Trie structureInfo;
                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage(), StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<IPage>("FakePages/1");
                    draft = session.Load<IPage>("FakePages/1/Draft");
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.NotNull(structureInfo.RootNode.PageId);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);

                Assert.Null(draft);
                Assert.NotNull(page);

                Assert.True(page.Metadata.IsPublished);
                Assert.NotNull(page.Metadata.Published);

            }

            [Fact]
            public void Can_Update_Start_Page_And_Publish()
            {
                // Given

                Trie structureInfo;
                var store = SetupContext();
                IPage page;

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage(), StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<IPage>("FakePages/1");
                    page.Metadata.Name = "Renamed";
                    session.Store(page, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<IPage>("FakePages/1");
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.ParentId);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Equal(0, structureInfo.RootNode.Children.Count);
                Assert.Equal("Renamed", page.Metadata.Name);
            }

            [Fact]
            public void Can_Publish_Start_Page_Then_Save_As_Draft()
            {
                // Given

                Trie structureInfo;
                var store = SetupContext();
                IPage page, draft;

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage(), StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<IPage>("FakePages/1");
                    page.Metadata.Name = "Renamed";
                    session.Store(page, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    draft = session.Load<IPage>("FakePages/1/Draft");
                    page = session.Load<IPage>("FakePages/1");
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then
              
                Assert.NotNull(draft);
                Assert.NotNull(page);
                Assert.NotNull(structureInfo.RootNode.PageId);
            }

            [Fact]
            public void Can_Create_Start_Page_As_Draft_Then_Publish()
            {
                // Given

                Trie structureInfo;
                var store = SetupContext();
                IPage page, draft;

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage(), StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<IPage>("FakePages/1");
                    page.Metadata.Name = "Renamed";
                    session.Store(page, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<IPage>("FakePages/1");
                    draft = session.Load<IPage>("FakePages/1/Draft");
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.ParentId);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Equal(0, structureInfo.RootNode.Children.Count);
                Assert.Equal("Renamed", page.Metadata.Name);
                Assert.Null(page.Metadata.Slug);
                Assert.Null(page.Metadata.Url);
                Assert.Null(draft);
            }

            [Fact]
            public void Can_Save_Start_Page_As_Draft()
            {
                // Given

                Trie structureInfo;
                IPage draft;
                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage(), StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    draft = session.Load<IPage>("FakePages/1");
                }

                // Then
                
                Assert.NotNull(structureInfo.RootNode);
                Assert.NotNull(structureInfo.RootNode.PageId);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);
                Assert.NotNull(draft);

            }

            [Fact]
            public void Can_Save_Child_Page_As_Draft()
            {
                // Given

                Trie structureInfo;
                IPage page;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "FakePages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    IPage draft = new FakePage
                    {
                        Id = "FakePages/2",
                        Parent = new PageReference("FakePages/1"),
                        Metadata =
                        {
                            Name = "Child page"
                        }
                    };
                    session.Store(draft, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {                    
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<FakePage>("FakePages/2");
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.ParentId);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Equal(1, structureInfo.RootNode.Children.Count);                
                Assert.Equal("FakePages/1", structureInfo.RootNode.Children.First().ParentId);                
                Assert.Equal("child-page", structureInfo.RootNode.Children.First().Url);
                Assert.Equal(page.Metadata.Url, structureInfo.RootNode.Children.First().Url);
                Assert.Equal("child-page", page.Metadata.Slug);
            }

            [Fact]
            public void Can_Publish_Child_Page()
            {
                // Given

                Trie structureInfo;
                IPage page;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    IPage p = new FakePage
                    {
                        Id = "fakepages/2",
                        Parent = new PageReference("fakepages/1"),
                        Metadata =
                        {
                            Name = "Child page"
                        }
                    };
                    session.Store(p, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<FakePage>("fakepages/2");
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal("fakepages/1", structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.ParentId);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Equal(1, structureInfo.RootNode.Children.Count);
                Assert.NotNull(structureInfo.RootNode.Children.First().PageId);
                Assert.Equal("fakepages/1", structureInfo.RootNode.Children.First().ParentId);
                Assert.Equal("child-page", structureInfo.RootNode.Children.First().Url);
                Assert.Equal(page.Metadata.Url, structureInfo.RootNode.Children.First().Url);
                Assert.Equal("child-page", page.Metadata.Slug);                
            }

            [Fact]
            public void Can_Alter_Child_Page_And_Save_As_Draft()
            {
                // Given

                Trie structureInfo;
                IPage page;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "FakePages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    IPage p = new FakePage
                    {
                        Id = "FakePages/2",
                        Parent = new PageReference("FakePages/1"),
                        Metadata =
                        {
                            Name = "Child page"
                        }
                    };
                    session.Store(p, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {                    
                    page = session.Load<FakePage>("FakePages/2");
                    page.Metadata.Name = "Page name changed";
                    session.Store(page, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<FakePage>("FakePages/2/draft");
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.ParentId);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Equal(1, structureInfo.RootNode.Children.Count);
                Assert.Equal("FakePages/2", structureInfo.RootNode.Children.First().PageId);
                Assert.Equal("FakePages/1", structureInfo.RootNode.Children.First().ParentId);
                Assert.Equal("child-page", structureInfo.RootNode.Children.First().Url);
                Assert.Equal(page.Metadata.Url, structureInfo.RootNode.Children.First().Url);
                Assert.Equal("child-page", page.Metadata.Slug);                
                Assert.False(page.Metadata.IsPublished);
            }

            [Fact]
            public void Can_Alter_Child_Page_And_Publish()
            {
                // Given

                Trie structureInfo;
                IPage page, draft;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "FakePages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    IPage p = new FakePage
                    {
                        Id = "FakePages/2",
                        Parent = new PageReference("FakePages/1"),
                        Metadata =
                        {
                            Name = "Child page"
                        }
                    };
                    session.Store(p, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<FakePage>("FakePages/2");
                    page.Metadata.Name = "Page name changed";
                    session.Store(page, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    draft = session.Load<FakePage>("FakePages/2/Draft");
                    page = session.Load<FakePage>("FakePages/2");
                }

                // Then

                Assert.Null(draft);
                Assert.NotNull(page);
                Assert.Equal("FakePages/2", structureInfo.RootNode.Children.First().PageId);                
                Assert.Equal(1,structureInfo.RootNode.Children.Count);
            }

            [Fact]
            public void Can_Add_Child_Page()
            {
                // Given

                Trie structureInfo;
                IPage page;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "FakePages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "FakePages/2", Parent = new PageReference("FakePages/1"), Metadata = { Name = "Child page" }}, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<FakePage>("FakePages/2");
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.ParentId);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Equal(1, structureInfo.RootNode.Children.Count);
                Assert.Equal("FakePages/2", structureInfo.RootNode.Children.First().PageId);
                Assert.Equal("FakePages/1", structureInfo.RootNode.Children.First().ParentId);
                Assert.Equal("child-page", structureInfo.RootNode.Children.First().Url);
                Assert.Equal(page.Metadata.Url, structureInfo.RootNode.Children.First().Url);
                Assert.Equal("child-page", page.Metadata.Slug);
            }

            [Fact]
            public void Can_Add_Second_Level_Child_Page()
            {
                // Given

                Trie structureInfo;
                IPage page;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "FakePages/1", Metadata = { Name = "Start" } });
                    session.Store(new FakePage { Id = "FakePages/2", Parent = new PageReference("FakePages/1"), Metadata = { Name = "Child page" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "FakePages/3", Parent = new PageReference("FakePages/2"), Metadata = { Name = "Child page 2" } }, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<FakePage>("FakePages/3");
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal("FakePages/1", structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.ParentId);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Equal(1, structureInfo.RootNode.Children.Count);
                Assert.Equal("FakePages/2", structureInfo.RootNode.Children.First().PageId);
                Assert.Equal("FakePages/1", structureInfo.RootNode.Children.First().ParentId);
                Assert.Equal("child-page", structureInfo.RootNode.Children.First().Url);
                Assert.Equal(page.Metadata.Url, structureInfo.RootNode.Children.First().Children.Single(x => x.PageId == page.Id).Url);
                Assert.Equal("child-page-2", page.Metadata.Slug);
            }

            [Fact]
            public void Can_Move_Page()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "fakepages/2", Parent = new PageReference("fakepages/1"), Metadata = { Name = "Child page" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "fakepages/3", Parent = new PageReference("fakepages/2"), Metadata = { Name = "Child page" } }, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    IPage fakeChild = session.Load<FakePage>("fakepages/3");
                    fakeChild.Parent = new PageReference("fakepages/1");
                    session.SaveChanges();
                }
                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then
                
                Assert.Equal("fakepages/1", structureInfo.RootNode.Children.Single(node => node.PageId == "fakepages/3").ParentId);

            }

            [Fact]
            public void Can_Move_Page_With_Multiple_Levels_Of_Children()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } });
                    session.Store(new FakePage { Id = "fakepages/2", Parent = new PageReference("fakepages/1"), Metadata = { Name = "Child page 1" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "fakepages/3", Parent = new PageReference("fakepages/2"), Metadata = { Name = "Child page 2" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "fakepages/4", Parent = new PageReference("fakepages/3"), Metadata = { Name = "Child page 3" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "fakepages/5", Parent = new PageReference("fakepages/4"), Metadata = { Name = "Child page 4" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "fakepages/6", Parent = new PageReference("fakepages/4"), Metadata = { Name = "Child page 5" } }, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    IPage fakeChild = session.Load<FakePage>("fakepages/4");
                    fakeChild.Parent = new PageReference("fakepages/2");
                    session.SaveChanges();
                }
                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                var parent = structureInfo.RootNode.Flatten(n => n.Children).Single(n => n.PageId == "fakepages/2");
                var childPage3 = structureInfo.RootNode.Flatten(n => n.Children).Single(n => n.PageId == "fakepages/4");
                var childPage4 = structureInfo.RootNode.Flatten(n => n.Children).Single(n => n.PageId == "fakepages/5");
                var childPage5 = structureInfo.RootNode.Flatten(n => n.Children).Single(n => n.PageId == "fakepages/6");

                Assert.Equal(2, parent.Children.Count);
                Assert.Equal("child-page-1/child-page-3", childPage3.Url);
                Assert.Equal(2, childPage3.Children.Count);
                Assert.Equal("child-page-1/child-page-3/child-page-4", childPage4.Url);
                Assert.Equal("child-page-1/child-page-3/child-page-5", childPage5.Url);
            }

            [Fact]
            public void Can_Delete_Page()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "fakepages/2", Parent = new PageReference("fakepages/1"), Metadata = { Name = "Child page" } }, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    IPage page = session.Load<FakePage>("fakepages/2");
                    session.Delete(page);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.NotNull(structureInfo.RootNode);
                Assert.Equal(0, structureInfo.RootNode.Children.Count);
            }

            [Fact]
            public void Can_Change_Slug_On_Page()
            {
                // Given

                Trie structureInfo;
                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "FakePages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "FakePages/2", Parent = new PageReference("FakePages/1"), Metadata = { Name = "Child page" } }, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    IPage page = session.Load<FakePage>("FakePages/2");
                    page.Metadata.Slug = "new-slug";
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.Equal("new-slug", structureInfo.RootNode.Children.First().Url);              
            }

            [Fact]
            public void Slug_Should_Not_Be_Changed_When_Renaming_A_Page()
            {
                // Given

                Trie structureInfo;
                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "FakePages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    session.Store(new FakePage { Id = "FakePages/2", Parent = new PageReference("FakePages/1"), Metadata = { Name = "Child page" } }, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    IPage page = session.Load<FakePage>("FakePages/2");
                    page.Metadata.Name = "The new name";
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.Equal("child-page", structureInfo.RootNode.Children.First().Url);
            }

            [Fact]
            public void Can_Rename_Root_Page()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    IPage page = session.Load<FakePage>("fakepages/1");
                    page.Metadata.Name = "The new name";
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.Null(structureInfo.RootNode.Url);
            }

            [Fact]
            public void Can_Create_Root_Page_As_Draft()
            {
                // Given
                IPage draft;
                Trie structureInfo;

                var store = SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    draft = new FakePage { Metadata = { Name = "Draft" }};
                    session.Store(draft, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    draft = session.Load<IPage>("FakePages/1/draft");
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.NotNull(draft);
                Assert.NotNull(structureInfo.RootNode);
                Assert.Null(structureInfo.RootNode.PageId);
                Assert.Null(structureInfo.RootNode.Url);
            }

            [Fact]
            public void Can_Save_Page_Draft()
            {
                // Given
                IPage fakePage;
                IPage draft;
                var container = A.Fake<IContainer>();
                var store = this.SetupContext();

                // When

                using (var session = store.OpenSession())
                {
                    fakePage = new FakePage
                    {
                        Id = "fakepages/1",
                        Metadata =
                        {
                            Name = "Published version"
                        }
                    };
                    session.Store(fakePage, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    draft = session.Load<IPage>("fakepages/1");
                    draft.Metadata.Name = "Page draft";                    
                    session.Store(draft, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    fakePage = session.Load<IPage>("fakepages/1");
                    draft = session.Load<IPage>("fakepages/1/draft");
                }

                // Then
                
                Assert.NotNull(fakePage);
                Assert.Equal("fakepages/1", fakePage.Id);
                Assert.Equal("Published version", fakePage.Metadata.Name);
                Assert.NotNull(draft);
                Assert.Equal("fakepages/1/draft", draft.Id);
                Assert.Equal("Page draft", draft.Metadata.Name);
            }

            [Fact(Skip = "Not sure if this is a good idea or not")]
            public void Multiple_Pages_With_Same_Url_Throws_Exception()
            {
                using (var store = SetupContext())
                {
                    using (var session = store.OpenSession())
                    {
                        session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } }, StoreAction.Publish);
                        session.Store(new FakePage { Id = "fakepages/2", Metadata = { Name = "Foo" } }, StoreAction.Publish);
                        session.Store(new FakePage { Id = "fakepages/3", Metadata = { Name = "Foo" } }, StoreAction.Publish);

                        Assert.Throws(typeof(DuplicateUrlException), () => session.SaveChanges());
                    }                    
                }                
            }

            [Fact]
            public void Can_Save_Page_Without_A_Name()
            {
                using (var store = SetupContext())
                {
                    using (var session = store.OpenSession())
                    {
                        var fakePage = new FakePage { Id = "fakepages/1" };
                        session.Store(fakePage, StoreAction.Publish);
                        session.Store(new FakePage { Id = "fakepages/2", Parent = fakePage }, StoreAction.Publish);                        
                        session.SaveChanges();
                    }

                    using (var session = store.OpenSession()) {
                        var fakePage = session.Load<FakePage>("fakepages/2");
                        Assert.Equal("fakepages-2", fakePage.Metadata.Slug);
                    }
                }  
            }

            [Fact]
            public void Can_Save_Root_Page()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                IPage page;
                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } });
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<FakePage>("fakepages/1");
                    page.Metadata.Name = "The new name";
                    session.Store(page, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<FakePage>("fakepages/1/draft");
                }

                // Then

                Assert.Null(page.Metadata.Url);
                Assert.Null(page.Metadata.Slug);
                Assert.Null(structureInfo.RootNode.Url);                
            }

            [Fact]
            public void Can_Update_Root_Page_Draft()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                IPage page;
                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Metadata = { Name = "Start" } }, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<FakePage>("FakePages/1");
                    page.Metadata.Name = "The new name";
                    session.Store(page, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    page = session.Load<FakePage>("FakePages/1/Draft");
                }

                // Then

                Assert.Null(page.Metadata.Url);
                Assert.Null(page.Metadata.Slug);
                Assert.Null(structureInfo.RootNode.Url);
                Assert.Empty(structureInfo.RootNode.Children);
            }

            [Fact]
            public void Can_Publish_Then_Unpublish_And_Republish_Root_Page_With_Children() {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                IPage start;

                // When

                using (var session = store.OpenSession()) {
                    start = new FakePage {Metadata = {Name = "Start"}};
                    session.Store(start, StoreAction.Publish);
                    session.Store(new FakePage { Parent = new PageReference(start.Id) ,Metadata = { Name = "Child" }}, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    start = session.Load<FakePage>("FakePages/1");
                    session.Store(start, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    start = session.Load<FakePage>("FakePages/1/draft");
                    session.Store(start, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.NotEmpty(structureInfo.RootNode.Children);                
            }

            [Fact]
            public void Can_Publish_Then_Unpublish_And_Republish_Child_Page_With_Children()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                IPage start, child;

                // When

                using (var session = store.OpenSession()) {
                    start = new FakePage {Metadata = {Name = "Start"}};
                    session.Store(start, StoreAction.Publish);
                    child = new FakePage {Parent = new PageReference(start.Id), Metadata = {Name = "Child"}};
                    session.Store(child, StoreAction.Publish);
                    session.Store(new FakePage { Parent = new PageReference(child.Id), Metadata = { Name = "Child2"}}, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    child = session.Load<FakePage>("FakePages/2");
                    session.Store(child, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    child = session.Load<FakePage>("FakePages/2/Draft");
                    child.Metadata.Name = "Do it";
                    session.Store(child, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                    child = session.Load<FakePage>("FakePages/2/Draft");
                }

                // Then

                Assert.NotEmpty(structureInfo.RootNode.Children);
                Assert.NotNull(child);
                //Assert.NotEmpty(structureInfo.RootNode.Children.Single(x => x.DraftId == "FakePages/2/draft").Children);
                //Assert.NotNull(structureInfo.RootNode.Children.Single(x => x.DraftId == "FakePages/2/draft").ParentId);
            }

            [Fact]
            public void Can_Save_Second_Level_Child_With_Draft_As_Parent()
            {
                // Given

                Trie structureInfo;

                var store = SetupContext();

                IPage child;

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new  FakePage(), StoreAction.Publish);

                    child = new FakePage
                    {
                        Parent = new PageReference("FakePages/1"),
                        Metadata = { Name = "Child" }
                    };
                    session.Store(child, StoreAction.Save);

                    session.Store(new FakePage
                    {
                        Parent = new PageReference(child.Id),
                        Metadata = { Name = "Child2" }
                    }, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession()) {

                    child = session.Load<IPage>("FakePages/2");
                    child.Metadata.Name = "Changed name";
                    session.Store(child, StoreAction.Publish);
                    session.SaveChanges();

                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.NotNull(child);
                Assert.NotEmpty(structureInfo.RootNode.Children);
                Assert.NotEmpty(structureInfo.Get("FakePages/2").Children);
                //Assert.NotNull(structureInfo.GetNode("FakePages/3/draft").ParentId);
            }

            [Fact]
            public void Can_Publish_Second_Level_Child_With_Draft_As_Child()
            {
                // Given

                var store = SetupContext();

                IPage start, child;

                // When

                using (var session = store.OpenSession())
                {
                    start = new FakePage { Metadata = { Name = "Start" } };
                    session.Store(start, StoreAction.Publish);                    

                    child = new FakePage { Parent = new PageReference(start.Id), Metadata = { Name = "Child" } };
                    session.Store(child, StoreAction.Save);                    

                    session.Store(new FakePage { Parent = new PageReference(child.Id), Metadata = { Name = "Child2" } }, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    child = session.Load<IPage>("FakePages/2");
                    child.Metadata.Name = "Changed name";
                    session.Store(child, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    child = session.Load<IPage>("FakePages/3");                    
                }

                // Then

                Assert.Equal(child.Parent.Id,"FakePages/2");
                Assert.Equal(default(DateTime?), child.Metadata.Published);
            }

            [Fact]
            public void Can_Publish_Root_Level_With_Draft_As_Child()
            {
                // Given

                var store = SetupContext();

                IPage start, child;

                // When

                using (var session = store.OpenSession())
                {
                    start = new FakePage { Metadata = { Name = "Start" } };

                    session.Store(start, StoreAction.Save);
                    session.SaveChanges();

                    child = new FakePage { Parent = new PageReference(start.Id), Metadata = { Name = "Child" } };
                    session.Store(child, StoreAction.Save);
                    session.SaveChanges();

                }

                using (var session = store.OpenSession())
                {
                    child = session.Load<IPage>("FakePages/1");
                    child.Metadata.Name = "Changed name";
                    session.Store(child, StoreAction.Publish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {

                    child = session.Load<IPage>("FakePages/2");
                }

                // Then

                Assert.Equal(child.Parent.Id, "FakePages/1");
            }

            [Fact]
            public void Can_UnPublish_Page() {
                // Given

                var store = SetupContext();
                IPage start, child;

                // When

                using (var session = store.OpenSession())
                {
                    start = new FakePage { Metadata = { Name = "Start" } };

                    session.Store(start, StoreAction.Publish);
                    session.SaveChanges();

                }

                using (var session = store.OpenSession())
                {
                    child = session.Load<IPage>("FakePages/1");
                    child.Metadata.Name = "changed";
                    session.Store(child, StoreAction.UnPublish);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {

                    child = session.Load<IPage>("FakePages/1");
                }

                // Then
    
                Assert.Equal(default(DateTime?), child.Metadata.Published);
            }

            /// <summary>
            /// Can_s the save_ start_ page_ and_ add_ child.
            /// </summary>
            [Fact]
            public void Can_Save_Start_Page_And_Add_Child()
            {
                // Given

                var store = SetupContext();
                Trie structureInfo;

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } }, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/2", Parent = new PageReference("fakepages/1") }, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {

                    structureInfo = session.Load<Trie>(DefaultBrickPileBootstrapper.TrieId);
                }

                // Then

                Assert.Equal(2, structureInfo.RootNode.Flatten(node => node.Children).Count());
            }

            [Fact]
            public void Edit_Page_That_Has_Never_Been_Published_Should_Not_Create_A_Draft()
            {
                // Given

                var store = SetupContext();               
                IPage page, draft;

                // When

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/1", Metadata = { Name = "Start" } }, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    session.Store(new FakePage { Id = "fakepages/2", Parent = new PageReference("fakepages/1") }, StoreAction.Save);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    page = session.Load<IPage>("fakepages/2");
                    page.Metadata.Name = "Changed name";
                    session.Store(page, StoreAction.Save);
                }


                using (var session = store.OpenSession()) {
                    page = session.Load<IPage>("fakepages/2");
                    draft = session.Load<IPage>("fakepages/2/draft");
                }

                // Then
                
                Assert.NotNull(page);
                Assert.Null(draft);
            }
        }
    }
}
