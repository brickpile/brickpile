using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Infrastructure {
    public interface IPageService {
        IPageModel GetById(string id);
        IPageModel GetByUrl(string url);
        IPageModel GetHomePage();
        IEnumerable<IPageModel> GetChildren(IPageModel parent);
    }
}
