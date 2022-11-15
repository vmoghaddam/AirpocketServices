using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;


using System.Web.Http.Description;

using System.Data.Entity.Validation;

using System.Web.Http.ModelBinding;

using System.Text;
using System.Configuration;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using System.IO;
using System.Xml;
using System.Web;
using System.Text.RegularExpressions;
using Formatting = Newtonsoft.Json.Formatting;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using ApiMSG.Models;
using System.Net.Http.Headers;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;


namespace ApiMSG.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MagfaController : ApiController
    {
        [Route("api/magfa/test")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetMagfaTest()
        {
            Magfa m = new Magfa();
            var smsResult= m.enqueue(1, "09306678047", "HELO APIMSG")[0];
            var refids = new List<Int64>() { smsResult };
            System.Threading.Thread.Sleep(5000);
            var status = m.getStatus(refids);

            return Ok(status);
        }


        [Route("api/magfa/send/bulk")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetMagfaSendBulk()
        {
            var context = new ppa_vareshEntities();
            var refids = new List<Int64>() ;
            var rows = context.BulkMsgs.ToList();
            var message = rows.First().Message;
            foreach(var x in rows)
            {
                Magfa m = new Magfa();
                var smsResult = m.enqueue(1, x.Mobile, message)[0];
                x.RefId = smsResult;
                x.Message = message;
                refids.Add(smsResult);
            }

            context.SaveChanges();
           // var refids = new List<Int64>() { smsResult };
           // System.Threading.Thread.Sleep(5000);
           // var status = m.getStatus(refids);

            return Ok(refids);
        }


        [Route("api/magfa/status/bulk/{skip}/{take}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetMagfaStatusBulk(int skip,int take)
        {
            var context = new ppa_vareshEntities();
            Magfa m = new Magfa();
            var rows = context.BulkMsgs.ToList().OrderBy(q=>q.Id).Skip(skip).Take(take).ToList();
            var refids = rows.Select(q =>(Int64) q.RefId).ToList();
            var reslt=m.getStatus(refids);
            var c = 0;
            foreach (var x in rows)
            {
                x.Status = reslt[c];
                c++;
            }

            context.SaveChanges();
            // var refids = new List<Int64>() { smsResult };
            // System.Threading.Thread.Sleep(5000);
            // var status = m.getStatus(refids);

            return Ok(reslt);
        }





    }
}
