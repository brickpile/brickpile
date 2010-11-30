namespace Stormbreaker.Web {
    public interface IPathResolver {
        IPathData ResolvePath(string virtualUrl);
    }
}