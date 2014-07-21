using BrickPile.Core;
using BrickPile.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace BrickPile.Tests
{
    public class SlugTests
    {
        public class CreateSlug
        {
            [Theory]
            [InlineData("PA  GE")]
            [InlineData("some swedish letters å ä ö Å Ä Ö")]
            [InlineData("This is  a common page")]
            [InlineData("fakepages/2")]
            public void Slug_Should_Be_Normalized(string name)
            {
                // Given                

                var page = new FakePage
                {
                    Metadata =
                    {
                        Name = name
                    }
                };

                // When

                var result = Slug.CreateSlug(page);

                //Then

                Assert.NotNull(result);
                Assert.DoesNotContain(" ", result);
                Assert.DoesNotContain("--", result);
                Assert.DoesNotContain("å", result);
                Assert.DoesNotContain("ä", result);
                Assert.DoesNotContain("ö", result);
                Assert.DoesNotContain("/", result);
            }
        }
    }
}
