using BrickPile.Core;
using BrickPile.Domain;

namespace BrickPile.Tests.Fakes
{
    [ContentType(Name = "Fake page", ControllerType = typeof(FakeController))]
    public class FakePage : Page
    {
    }
}
