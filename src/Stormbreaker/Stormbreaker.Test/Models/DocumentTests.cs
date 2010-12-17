using NUnit.Framework;
using Stormbreaker.Test.Repositories;

namespace Stormbreaker.Test.Models {
    [TestFixture]
    public class DocumentTests {

        [Test]
        public void AddChild_AddsChildToDocument() {
            var document = new DummyDocument();
            var childDocument = new DummyDocument
                                                    {
                                                        Name = "Test"
                                                    };
            document.Children.Add(childDocument);

            Assert.AreEqual(document.Children.Count, 1);
        }

        [Test]
        public void AddParent_AddsAParentToDocument()
        {
            var document = new DummyDocument();
            var parentDocument = new DummyDocument
            {
                Name = "Test"
            };
            document.Parent = parentDocument;

            Assert.NotNull(document.Parent);
        }
    }
}