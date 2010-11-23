namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// The IPathResolver interface 
    /// </summary>
    public interface IPathResolver {
        IPathData ResolvePath(string virtualUrl);
    }
}