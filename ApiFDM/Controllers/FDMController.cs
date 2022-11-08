using ApiFDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiFDM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FDMController : ApiController
    {
        [Route("api/fdm/action/save")]
        [AcceptVerbs("POST")]
        public IHttpActionResult  PostEmployee(FDMDto dto)
        {
            var context = new Models.dbEntities();
            var fdmAction = new Models.FDMEventAction();
            fdmAction.FDMId = dto.eventId;
            fdmAction.EndDate = ConvertToDate(dto.end);
            fdmAction.StartDate = ConvertToDate(dto.start);
            fdmAction.ActionInfo = dto.remark;
            fdmAction.ActionType = dto.type;
            fdmAction.CrewId = dto.crew;
            fdmAction.DateCreate = DateTime.Now;

            context.FDMEventActions.Add(fdmAction);

            if (dto.type == "GROUND")
            {
                var fdp = new FDP();
                fdmAction.FDP = fdp;
                fdp.DutyType = 100000;
                fdp.IsTemplate =false;
                fdp.CrewId = dto.crew;
                fdp.DateStart = ((DateTime)fdmAction.StartDate).AddMinutes(-210);
                fdp.DateEnd = ((DateTime)fdmAction.EndDate).AddMinutes(-210);
                fdp.CityId = -1;
                fdp.GUID = Guid.NewGuid();
                fdp.UPD = 1;
                fdp.InitStart = fdp.DateStart;
                fdp.InitEnd = fdp.DateEnd;
                fdp.InitRestTo = fdp.DateEnd;
                fdp.Remark = "FDM";
               // context.FDPs.Add(fdp);



            }
            if (dto.type == "TRAINING")
            {
                var fdp = new FDP();
                fdmAction.FDP = fdp;
                fdp.DutyType  =5000;
                fdp.IsTemplate = false;
                fdp.CrewId = dto.crew;
                fdp.DateStart = ((DateTime)fdmAction.StartDate).AddMinutes(-210);
                fdp.DateEnd = ((DateTime)fdmAction.EndDate).AddMinutes(-210);
                fdp.CityId = -1;
                fdp.GUID = Guid.NewGuid();
                fdp.UPD = 1;
                fdp.InitStart = fdp.DateStart;
                fdp.InitEnd = fdp.DateEnd;
                fdp.InitRestTo = fdp.DateEnd;
                fdp.Remark = dto.course;
                // context.FDPs.Add(fdp);



            }

            context.SaveChanges();
            return Ok(fdmAction.Id);
        }

        [Route("api/fdm/actions")]
        public  IHttpActionResult  GetActions()
        {
            var context = new Models.dbEntities();
            var actions =   context.ViewFDMEventActions.OrderByDescending(q => q.DateCreate).ToList();
            return Ok(actions);

        }

        [Route("api/fdm/actions/crew/{id}")]
        public IHttpActionResult GetActionsByCrew(int id)
        {
            var context = new Models.dbEntities();
            var actions = context.ViewFDMEventActions.Where(q=>q.CrewId==id).OrderByDescending(q => q.DateCreate).ToList();
            return Ok(actions);

        }

        [Route("api/fdm/actions/event/{id}")]
        public IHttpActionResult GetActionsByEvent(int id)
        {
            var context = new Models.dbEntities();
            var actions = context.ViewFDMEventActions.Where(q => q.FDMId == id).OrderByDescending(q => q.DateCreate).ToList();
            return Ok(actions);

        }


        DateTime ConvertToDate(string str)
        {
            var yy =Convert.ToInt32( str.Substring(0, 4));
            var mm = Convert.ToInt32(str.Substring(4, 2));
            var dd = Convert.ToInt32(str.Substring(6, 2));
            return new DateTime(yy, mm, dd);
        }

        public class FDMDto
        {
            public int eventId { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string remark { get; set; }
            public string type { get; set; }
            public string course { get; set; }
            public int crew { get; set; }
        }

    }
}
