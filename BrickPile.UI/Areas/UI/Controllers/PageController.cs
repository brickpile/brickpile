using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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

    //[Authorize]
    public class PageController : ApiController
    {
        private IDocumentSession _session;

        // GET api/page
        [HttpGet]
        public HttpResponseMessage Get() {

            var jsonSerializerSettings = new JsonSerializerSettings {
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

            return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(viewModel,jsonSerializerSettings))
                };
        }

        // GET api/page/articles/5
        [HttpGet]
        public HttpResponseMessage Get(string type, int id) {

            var jsonSerializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            var current = _session.Include<IPage,AllPages>(y => y.Children).Load<Page>(string.Join("/",type,id));

            if (current == null) {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

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

            return new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(
                    JsonConvert.SerializeObject(viewModel, jsonSerializerSettings)
                    )
            };

        }

        // POST api/page
        [HttpPost]
        public HttpResponseMessage Post([FromBody]IPage value) {

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            _session.Store(value);
            _session.SaveChanges();

            var jsonSerializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(JsonConvert.SerializeObject(value, jsonSerializerSettings))
            };

        }

        // PUT api/page/5
        [HttpPut]
        public HttpResponseMessage Put([FromBody]IPage value) {

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            value.Metadata.Changed = DateTime.Now;

            _session.Store(value);
            _session.SaveChanges();

            var jsonSerializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return new HttpResponseMessage(HttpStatusCode.Created) {
                Content = new StringContent(JsonConvert.SerializeObject(value, jsonSerializerSettings))
            };

        }

        // DELETE api/page/5
        public HttpResponseMessage Delete(string type, int id) {

            _session = ObjectFactory.GetInstance<IDocumentSession>();

            var page = _session.Load<Page>(string.Join("/", type,id));
            if (page == null) {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            _session.Delete(page);
            _session.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public static List<Type> ContentTypes {
            get {
                if (_contentTypes == null)
                {
                    _contentTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                                     where type.GetCustomAttributes(typeof(PageTypeAttribute), true).Length > 0
                            select type).ToList();
                    
                }
                return _contentTypes;
            }
        }

        private static List<Type> _contentTypes ;

        public static string GetStringIdFor<T>(int id) {
            var session = ObjectFactory.GetInstance<IDocumentSession>();
            var c = session.Advanced.DocumentStore.Conventions;
            return c.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        }
    }
}
