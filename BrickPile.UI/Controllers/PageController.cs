using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BrickPile.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using StructureMap;

namespace BrickPile.Samples.Controllers
{
    public class ResponseContent {
        public PageModel CurrentPage { get; set; }
        public IEnumerable<PageModel> Children { get; set; } 
    }
    [Authorize]
    public class PageController : ApiController {
        // GET api/page
        public HttpResponseMessage Get() {
            var session = ObjectFactory.GetInstance<IDocumentSession>();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var home = session.Query<PageModel>()
                .Customize(x => x.Include<PageModel>(y => y.Children))
                .SingleOrDefault(x => x.Parent == null);

            var viewModel = new ResponseContent
            {
                CurrentPage = home,
                Children = session.Query<PageModel>()
                    .Where(x => x.Parent.Id == home.Id)
                    .OrderBy(x => x.Metadata.SortOrder)
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(viewModel,jsonSerializerSettings)
                    )
            };
            return response;
        }

        // GET api/page/articles/5
        public HttpResponseMessage Get(string id) {

            var session = ObjectFactory.GetInstance<IDocumentSession>();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var current = session.Include<PageModel>(y => y.Children).Load<PageModel>(id);

            var viewModel = new ResponseContent
            {
                CurrentPage = current,
                Children = session.Query<PageModel>()
                    .Where(x => x.Parent.Id == current.Id)
                    .OrderBy(x => x.Metadata.SortOrder)
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(viewModel, jsonSerializerSettings)
                    )
            };
            return response;

        }

        // POST api/page
        public HttpResponseMessage Post([FromBody]PageModel value) {

            var session = ObjectFactory.GetInstance<IDocumentSession>();
            session.Store(value);
            session.SaveChanges();

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

        // PUT api/page/articles/5
        public HttpResponseMessage Put(string id, [FromBody]PageModel value) {

            var session = ObjectFactory.GetInstance<IDocumentSession>();
            value.Metadata.Changed = DateTime.Now;
            session.Store(value);
            session.SaveChanges();

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

        // DELETE api/page/articles/5
        public void Delete(string id) {
            var session = ObjectFactory.GetInstance<IDocumentSession>();
            var content = session.Load<PageModel>(id);
            session.Delete(content);
            session.SaveChanges();
        }

        //public static string GetStringIdFor<T>(this IDocumentSession session, int id) {
        //    var c = session.Advanced.DocumentStore.Conventions;
        //    return c.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        //}
    }

}
