using System;
using System.Collections.Generic;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Stormbreaker.Infrastructure {
    public class PageService : IPageService {
        private readonly IRepository<IPageModel> _repository;

        public IPageModel GetById(string id) {
            return _repository.SingleOrDefault<IPageModel>(x => x.Id.Equals(id));
        }

        public IPageModel GetByUrl(string url) {
            return _repository.SingleOrDefault<IPageModel>(x => x.MetaData.Url.Equals(url));
        }

        public IPageModel GetHomePage() {
            return _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
        }

        public IEnumerable<IPageModel> GetChildren(IPageModel parent) {
            throw new NotImplementedException();
        }

        public PageService(IRepository<IPageModel> repository) {
            _repository = repository;
        }
    }
}
