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
using ApiScheduling.Models;
using System.Net.Http.Headers;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;
using Spire.Xls;
using ApiScheduling.ViewModel;

namespace ApiScheduling.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SchedulingController : ApiController
    {

        [Route("api/test")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> GetTEST()
        {
            var _context = new Models.dbEntities();

            var result = _context.FDPs.Take(10).ToList();
            return Ok(result);

            // return new DataResponse() { IsSuccess = false };
        }

        [Route("api/sch/crew/valid/")]

        //nookp
        public IHttpActionResult GetValidCrewForRoster( DateTime dt)
        {
            //nooz
            //this.context.Database.CommandTimeout = 160;

            var context = new Models.dbEntities();
            var query = context.ViewCrewValidFTLs.ToList();



            // return result.OrderBy(q => q.STD);
            return Ok(query);

        }

        [Route("api/sch/roster/fdps/")]

        //nookp
        public async Task<IHttpActionResult> GetCrewDuties(DateTime df, DateTime dt)
        {
            //nooz
            //this.context.Database.CommandTimeout = 160;

            var context = new Models.dbEntities();
            df = df.Date;
            dt = dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var _start = df.ToUniversalTime();
            var _end = dt.ToUniversalTime();
            var fdps = await  context.FDPs.Where(q => q.DutyType == 1165 && q.CrewId != null && q.InitStart >= _start && q.InitStart <= _end
              && !string.IsNullOrEmpty(q.Key)
            ).ToListAsync();
            var _ids = fdps.Select(q => q.Id).ToList();
            //  var viewfdps=await this.context.ViewFDPRests.Where(q => _ids.Contains(q.Id)).ToListAsync();
            var result = new List<RosterFDPDto>();
            foreach (var x in fdps)
            {
                //   var rfdp = viewfdps.FirstOrDefault(q => q.Id == x.Id);
                var item = new RosterFDPDto()
                {
                    Id = x.Id,
                    crewId = (int)x.CrewId,
                    flts = x.InitFlts,
                    from = Convert.ToInt32(x.InitFromIATA),
                    group = x.InitGroup,
                    homeBase = (int)x.InitHomeBase,
                    index = (int)x.InitIndex,
                    key = x.Key.Replace("*0", ""),
                    no = x.InitNo,
                    rank = x.InitRank,
                    route = x.InitRoute,
                    scheduleName = x.InitScheduleName,
                    to = Convert.ToInt32(x.InitToIATA),
                    flights = x.InitFlights.Split('*').ToList(),

                };
                // if (rfdp!=null)
                //  {
                //     item.IsSplitDuty = rfdp.ExtendedBySplitDuty > 0;
                //     item.SplitValue = rfdp.DelayAmount;
                // }
                item.ids = new List<RosterFDPId>();
                foreach (var f in item.flights)
                {
                    var prts = f.Split('_').ToList();
                    item.ids.Add(new RosterFDPId() { id = Convert.ToInt32(prts[0]), dh = Convert.ToInt32(prts[1]) });
                }
                result.Add(item);

            }

            return Ok(result);


        }

        [Route("api/fdp/log")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> GetFDPLog(DateTime dt,DateTime df)
        {
            var _context = new Models.dbEntities();
            var _dt = dt.Date.AddDays(1);
            var _df = df.Date;

            var query = from x in _context.ViewFDPLogs
                        where x.DateAction>=_df && x.DateAction<=_dt
                        select x;

            var result =await query.OrderBy(q => q.DateAction).ToListAsync();
            return Ok(result);

            // return new DataResponse() { IsSuccess = false };
        }


        [Route("api/event/group/save")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> SaveEventGroup(eventDto dto)
        {
            var ctx = new Models.dbEntities();
            var df = toDate(dto.df, dto.tf);
            var dt = toDate(dto.dt, dto.tt);
            df = df.AddMinutes(-210);
            dt = dt.AddMinutes(-210);
            foreach(var id in dto.ids)
            {
                var duty = new FDP();
                duty.DateStart = df;
                duty.DateEnd = dt;
                duty.CityId =   null;
                duty.CrewId = id;
                duty.DutyType = dto.type;
                duty.GUID = Guid.NewGuid();
                duty.IsTemplate = false;
                duty.Remark = dto.remark != null ? Convert.ToString(dto.remark) : "";
                duty.UPD = 1;
                duty.InitStart = duty.DateStart;
                duty.InitEnd = duty.DateEnd;
                duty.InitRestTo = duty.DateEnd;
                ctx.FDPs.Add(duty);


            }

            //  var duty = new FDP();
            //  DateTime _date = Convert.ToDateTime(dto.DateStart);
            // _date = _date.Date;



            // var rest = new List<int>() { 1167, 1168, 1170, 5000, 5001, 100001, 100003, 300010 };
            // duty.InitRestTo = rest.Contains(duty.DutyType) ? ((DateTime)duty.InitEnd).AddHours(12) : duty.DateEnd;


            await ctx.SaveChangesAsync();

            return Ok(true);
        }

        DateTime toDate (string dt,string t)
        {
            var year = Convert.ToInt32(dt.Substring(0, 4));
            var month = Convert.ToInt32(dt.Substring(4, 2));
            var day = Convert.ToInt32(dt.Substring(6, 2));
            var hour= Convert.ToInt32(t.Substring(0, 2));
            var minute = Convert.ToInt32(t.Substring(2, 2));

            return new DateTime(year, month, day, hour, minute, 0);
        }

    }


    public class eventDto
    {
        public List<int> ids { get; set; }
        public int type { get; set; }
        public string df { get; set; }
        public string dt { get; set; }
        public string tf { get; set; }
        public string tt { get; set; }
        public string remark { get; set; }
    }
}
