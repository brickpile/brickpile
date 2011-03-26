using System.Diagnostics;
using NUnit.Framework;
using Stormbreaker.Models;
using Stormbreaker.Test.Repositories;

namespace Stormbreaker.Test.Models {
    [TestFixture]
    public class DocumentTests {

        [Test]
        public void AddChild_AddsChildToDocument() {
            var document = new DummyPageModel();
            var childDocument = new DummyPageModel
                                                    {
                                                        MetaData = { Name = "Test"}
                                                    };
            //document.Children.Add(childDocument);

            //Assert.AreEqual(document.Children.Count, 1);
        }

        [Test]
        public void AddParent_AddsAParentToDocument()
        {
            var document = new DummyPageModel();
            var parentDocument = new DummyPageModel
            {
                MetaData = { Name = "Test" }
            };
            document.Parent = parentDocument;

            Assert.NotNull(document.Parent);
        }


    }
}