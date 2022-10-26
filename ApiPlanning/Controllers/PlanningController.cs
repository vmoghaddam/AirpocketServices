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
using ApiPlanning.Models;
using System.Net.Http.Headers;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;

namespace ApiPlanning.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PlanningController : ApiController
    {

        [Route("api/plan/newtime")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostNewTime(SimpleDto dto)
        {
            var _context = new Models.dbEntities();
            var flights = await _context.FlightInformations.Where(q => dto.ids.Contains(q.ID)).ToListAsync();
            foreach (var f in flights)
                if (f.NewTime == null || f.NewTime == 0)
                    f.NewTime = 1;
                else
                    f.NewTime = 0;
            var result= await _context.SaveChangesAsync();
            return Ok(flights);



        }
        [Route("api/plan/newregister")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostNewRegister(SimpleDto dto)
        {
            var _context = new Models.dbEntities();
            var flights = await _context.FlightInformations.Where(q => dto.ids.Contains(q.ID)).ToListAsync();
            foreach (var f in flights)
                if (f.NewReg == null || f.NewReg == 0)
                    f.NewReg = 1;
                else
                    f.NewReg = 0;
            var result = await _context.SaveChangesAsync();
            return Ok(flights);



        }



    }
    public class SimpleDto
    {
        public string username { get; set; }
        public List<int> ids { get; set; }
    }
}
