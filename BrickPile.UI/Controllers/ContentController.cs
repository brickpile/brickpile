using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BrickPile.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using StructureMap;

namespace BrickPile.Samples.Controllers {
    public class ContentController : ApiController {
        // GET api/page
        public IEnumerable<IContent> Get() {
            var session = ObjectFactory.GetInstance<IDocumentSession>();
            //return session.Query<IContent, AllContent>();
            return null;
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

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(session.Load<IContent>(id), jsonSerializerSettings))
            };
            return response;

        }

        // POST api/page
        public void Post([FromBody]IContent value) {
            var session = ObjectFactory.GetInstance<IDocumentSession>();
            session.Store(value);
            session.SaveChanges();
        }

        // PUT api/page/articles/5
        public void Put(string id, [FromBody]IContent value) {
            var session = ObjectFactory.GetInstance<IDocumentSession>();
            session.Store(value);
            session.SaveChanges();
        }

        // DELETE api/page/articles/5
        public void Delete(string id) {
            var session = ObjectFactory.GetInstance<IDocumentSession>();
            var content = session.Load<IContent>(id);
            session.Delete(content);
            session.SaveChanges();
        }
    }
}
