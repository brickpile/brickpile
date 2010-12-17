using System.Collections.Generic;

namespace Stormbreaker.Models {
    public interface IDocument
    {
        string Id { get; set; }
        string Name { get; set; }
        DenormalizedReference<IDocument> Parent { get; set; }
        IList<DenormalizedReference<IDocument>> Children { get; set; }
        string Slug { get; set; }
    }
}