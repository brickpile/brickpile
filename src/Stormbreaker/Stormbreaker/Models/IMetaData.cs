using System;

namespace Stormbreaker.Models {
    public interface IMetaData {
        string Name { get; set; }
        DateTime? Created { get; set; }
        DateTime? Updated { get; set; }
        DateTime? StartPublish { get; set; }
        DateTime? StopPublish { get; set; }
        string UrlSegment { get; set; }
    }
}