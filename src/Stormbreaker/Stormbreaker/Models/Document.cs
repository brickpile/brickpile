using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stormbreaker.Models {
    
    public abstract class Document : IDocument {
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        public string Name { get; set; }
        public DenormalizedReference<IDocument> Parent { get; set; }
        public IList<DenormalizedReference<IDocument>> Children { get; set; }
        public string Slug { get; set; }
        protected Document()
        {
            Children = new List<DenormalizedReference<IDocument>>();
        }        
    }
}