namespace Stormbreaker.Web.Routing {
    public interface IPathResolver {
        /// <summary>
        /// Resolves the path.
        /// </summary>
        /// <param name="virtualUrl">The virtual URL.</param>
        /// <returns></returns>
        IPathData ResolvePath(string virtualUrl);
    }
}