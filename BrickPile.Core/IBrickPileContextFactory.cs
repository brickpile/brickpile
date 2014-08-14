using System.Web.Routing;

namespace BrickPile.Core
{
    public interface IBrickPileContextFactory
    {
        /// <summary>
        ///     Creates the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns></returns>
        BrickPileContext Create(RequestContext requestContext);
    }
}