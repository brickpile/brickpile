namespace BrickPile.Core.Routing
{
    /// <summary>
    ///     Defines the methods that are required for an <see cref="IRouteResolver" />.
    /// </summary>
    internal interface IRouteResolver
    {
        /// <summary>
        /// Resolves the route based on the incoming <see cref="string" /> url.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        ResolveResult Resolve(string virtualPath);
    }
}