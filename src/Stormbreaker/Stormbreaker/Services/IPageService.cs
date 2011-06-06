using System.Collections.Generic;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Stormbreaker.Services {

    public interface IPageService {
        IEnumerable<IPageModel> Children(IPageModel model);
    }

    public class PageService : IPageService {
        private readonly IPageRepository _repository;

        public IEnumerable<IPageModel> Children(IPageModel model) {
            return _repository.GetChildren(model);
        }
        public PageService(IPageRepository repository) {
            _repository = repository;
        }
    }

}