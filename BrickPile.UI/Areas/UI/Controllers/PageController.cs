using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using StructureMap;

namespace BrickPile.UI.Areas.UI.Controllers
{
    public class ResponseContent {

        public IPage CurrentPage { get; set; }
        public IEnumerable<IPage> Children { get; set; }
        public List<ContentType> ContentTypes { get; set; } 
    }

    public class ContentType
    {
        public string Name { get; set; }
        public string AssemblyQualifiedName { get; set; }
    }

    [Authorize]
    public class PageController : ApiController
    {
        private IDocumentSession _session;

        // GET api/page
        [HttpGet]
        public HttpResponseMessage Get() {

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            var home = _session.Query<IPage, AllPages>()
                .Customize(x => x.Include<IPage, AllPages>(y => y.Children))
                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                .SingleOrDefault(x => x.Parent == null);

            var viewModel = new ResponseContent();

            var children = new List<IPage>();

            if (home != null) {

                children.AddRange(_session.Query<IPage, AllPages>()
                                            .Where(x => x.Parent.Id == home.Id)
                                            .OrderBy(x => x.Metadata.SortOrder));

                viewModel.Children = children;
            }

            viewModel.CurrentPage = home;

            // Add this to some kind of cached list
            viewModel.ContentTypes = (from contentType in ContentTypes
                                      select new ContentType
                                          {
                                              Name = contentType.Name,
                                              AssemblyQualifiedName = contentType.FullName + ", " + contentType.Assembly.GetName().Name
                                          }).ToList();

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(viewModel,jsonSerializerSettings))
            };

            return response;
        }

        // GET api/page/articles/5
        [HttpGet]
        public HttpResponseMessage Get(string id) {

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            var current = _session.Include<IPage,AllPages>(y => y.Children).Load<Page>(id);

            var viewModel = new ResponseContent
                {
                    CurrentPage = current,
                    Children = _session.Query<IPage, AllPages>()
                                      .Where(x => x.Parent.Id == current.Id)
                                      .OrderBy(x => x.Metadata.SortOrder),
                    ContentTypes = (from contentType in ContentTypes
                                    select new ContentType
                                        {
                                            Name = contentType.Name,
                                            AssemblyQualifiedName = contentType.FullName + ", " + contentType.Assembly.GetName().Name
                                        }).ToList()
                };

            // Add this to some kind of cached list

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(viewModel, jsonSerializerSettings)
                    )
            };
            return response;

        }

        // POST api/page
        [HttpPost]
        public HttpResponseMessage Post([FromBody]IPage value) {

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            _session.Store(value);
            _session.SaveChanges();

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(value, jsonSerializerSettings))
            };
            return response;
        }

        // PUT api/page/5
        [HttpPut]
        public HttpResponseMessage Put(string id, [FromBody]IPage value) {

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            value.Metadata.Changed = DateTime.Now;

            _session.Store(value);
            _session.SaveChanges();

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(value, jsonSerializerSettings))
            };
            return response;
        }

        // DELETE api/page/5
        public void Delete(string id) {
            var content = _session.Load<Page>(id);
            _session.Delete(content);
            _session.SaveChanges();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageController" /> class.
        /// </summary>
        /// <param name="session">The session.</param>
        //public PageController(IDocumentSession session)
        //{
        //    _session = session;
        //}

        //public static string GetStringIdFor<T>(int id)
        //{
        //    var session = ObjectFactory.GetInstance<IDocumentSession>();
        //    var c = session.Advanced.DocumentStore.Conventions;
        //    return c.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        //}

        public static List<Type> ContentTypes {
            get {
                if (_contentTypes == null)
                {
                    _contentTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where type.GetCustomAttributes(typeof(ContentTypeAttribute), true).Length > 0
                            select type).ToList();
                    
                }
                return _contentTypes;
            }
        }

        private static List<Type> _contentTypes ;

    }
}
