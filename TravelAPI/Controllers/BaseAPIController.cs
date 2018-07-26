using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TravelAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class BaseAPIController : ApiController
    {
        protected HttpResponseMessage ToJson(dynamic obj, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            

            var response = Request.CreateResponse(httpStatusCode);
            response.Content = new StringContent(
                JsonConvert.SerializeObject(obj, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }),
            Encoding.UTF8, "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
