using System.Web.Mvc;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Controllers {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class DocumentController<T> : Controller where T : IDocument {
        private readonly IRepository _repository;
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        public T CurrentDocument
        {
            get
            {
                if (_currentDocument == null)
                {
                    _currentDocument = (T) RouteData.DataTokens[DocumentRoute.DocumentKey];
                    if (_currentDocument == null)
                    {
                        _currentDocument = (T)_repository.Load<IDocument>("documents/1");
                        RouteData.ApplyCurrentDocument("Home", "Index", _currentDocument);
                    }

                }
                return _currentDocument;
            }
        }
        private T _currentDocument;
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        public DocumentController(IRepository repository)
        {
            _repository = repository;
        }
    }
}