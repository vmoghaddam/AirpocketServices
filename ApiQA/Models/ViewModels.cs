using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ApiQA.ViewModels
{
    

    public class CustomActionResult : IHttpActionResult
    {

        private System.Net.HttpStatusCode statusCode;

        public object data;
        public System.Net.HttpStatusCode Code { get { return statusCode; } }
        public CustomActionResult(System.Net.HttpStatusCode statusCode, object data)
        {

            this.statusCode = statusCode;

            this.data = data;

        }
        public HttpResponseMessage CreateResponse(System.Net.HttpStatusCode statusCode, object data)
        {

            HttpRequestMessage request = new HttpRequestMessage();
            request.Properties.Add(System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            HttpResponseMessage response = request.CreateResponse(statusCode, data);

            return response;

        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(CreateResponse(this.statusCode, data));
        }

    }


    public class updateLogResult
    {
        public bool sendNira { get; set; }
        public int flight { get; set; }
        public List<int> offIds { get; set; }
        public List<int> fltIds { get; set; }
        public List<offcrew> offcrews { get; set; }
    }

    public class offcrew
    {
        public int? flightId { get; set; }
        public List<int?> crews { get; set; }
    }

    public class BaseSummary
    {
        public int? BaseId { get; set; }
        public string BaseIATA { get; set; }
        public string BaseName { get; set; }
        public int Total { get; set; }
        public int TakeOff { get; set; }
        public int Landing { get; set; }
        public int Canceled { get; set; }
        public int Redirected { get; set; }
        public int Diverted { get; set; }
        public int? TotalDelays { get; set; }
        public int? DepartedPax { get; set; }
        public int? ArrivedPax { get; set; }
    }

}