using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLogDefault.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiLogDefaultMainController : ApiController
    {
        [Route("api/test")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Test1()
        {
            

            return Ok("test1");
        }

        [Route("api/flight/crews/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightCrews(int id)
        {
            try
            {
                var _context = new Models.dbEntities();
                var result = _context.XFlightCrews.Where(q => q.FlightId == id).OrderBy(q => q.IsPositioning).ThenBy(q => q.GroupOrder).ToList();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return Ok(new List<Models.XFlightCrew> { new Models.XFlightCrew() { Name="ERROR" } });
            }
             
        }

    }
}
