using System.Linq;
using BrickPile.Domain.Models;
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    public class PageModelWithParentsAndChildren : AbstractIndexCreationTask<Ancestor> {

        public PageModelWithParentsAndChildren() {

            Map = pages => from page in pages
                           select new { page.Id, page.Children, page.Metadata.Name, page.Metadata.IsPublished };

            TransformResults = (database, pages) => from page in pages
                                                    let ancestors = Recurse(page, c => database.Load<Ancestor>(c.Parent.Id))
                                                    select new
                                                    {
                                                        page.Id,
                                                        page.Metadata.Name,
                                                        Ancestors =
                                                        (
                                                           from ancestor in ancestors
                                                           select new
                                                           {
                                                               ancestor.Id,
                                                               ancestor.Metadata.Name,
                                                               ancestor.Children
                                                           }),
                                                        page.Children
                                                    };
        }
    }
}
