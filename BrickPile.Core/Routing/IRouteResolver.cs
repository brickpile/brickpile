using System;

namespace BrickPile.Core.Routing {
    public interface IRouteResolver {
        //TODO: document this
        Tuple<StructureInfo.Node, string> ResolveRoute(StructureInfo structureInfo, string virtualPath);
    }
}