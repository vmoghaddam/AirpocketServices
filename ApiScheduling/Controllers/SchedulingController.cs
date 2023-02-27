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
using System.Diagnostics;
using System.Globalization;

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
        public IHttpActionResult GetValidCrewForRoster(DateTime dt)
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
            var fdps = await context.FDPs.Where(q => q.DutyType == 1165 && q.CrewId != null && q.InitStart >= _start && q.InitStart <= _end
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
        public async Task<IHttpActionResult> GetFDPLog(DateTime dt, DateTime df)
        {
            var _context = new Models.dbEntities();
            var _dt = dt.Date.AddDays(1);
            var _df = df.Date;

            var query = from x in _context.ViewFDPLogs
                        where x.DateAction >= _df && x.DateAction <= _dt
                        select x;

            var result = await query.OrderBy(q => q.DateAction).ToListAsync();
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
            foreach (var id in dto.ids)
            {
                var duty = new FDP();
                duty.DateStart = df;
                duty.DateEnd = dt;
                duty.CityId = null;
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

        private string FormatTwoDigits(Int32 i)
        {
            string functionReturnValue = null;
            if (10 > i)
            {
                functionReturnValue = "0" + i.ToString();
            }
            else
            {
                functionReturnValue = i.ToString();
            }
            return functionReturnValue;
        }

        public string GetDutyTypeTitle(int t)
        {
            switch (t)
            {
                case 1165: return "FDP";
                case 1166: return "Day Off";
                case 1167: return "STBY";
                case 1168: return "STBY";
                case 1169: return "Vacation";
                case 1170: return "Reserve ";
                case 5000: return "Training";
                case 5001: return "Office";
                case 10000: return "RERRP/DayOff";
                case 10001: return "RERRP2/DayOff";
                case 100000: return "Ground";
                case 100001: return "Meeting";
                case 100002: return "Sick";
                case 100003: return "Simulator";
                case 100004: return "Expired Licence";
                case 100005: return "Expired Medical";
                case 100006: return "Expired Passport";
                case 100007: return "No Flight";
                case 100008: return "Requested Off";
                case 100009: return "Refuse";
                case 100020: return "Canceled(Rescheduling)";
                case 100021: return "Canceled(Flight cancellation)";
                case 100022: return "Canceled(Change of A/C Type)";
                case 100023: return "Canceled(FTL)";
                case 100024: return "Canceled(Not using Split Duty)";
                case 100025: return "Mission";
                default:
                    return "Unknown";
            }
        }
        [Route("api/roster/fdp/delete")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> DeleteFDP(dynamic dto)
        {
            var context = new Models.dbEntities();
            int fdpId = Convert.ToInt32(dto.Id);
            var fdp = await context.FDPs.FirstOrDefaultAsync(q => q.Id == fdpId);
            double total = 0;
            if (!string.IsNullOrEmpty(fdp.InitFlights))
            {
                var parts = fdp.InitFlights.Split('*').ToList();

                foreach (var x in parts)
                {
                    var _std = x.Split('_')[2];
                    var _sta = x.Split('_')[3];
                    var std = DateTime.ParseExact(_std, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                    var sta = DateTime.ParseExact(_sta, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                    total += (sta - std).TotalMinutes;
                }
                //this.context.Database.ExecuteSqlCommand("update employee set flightsum=isnull(flightsum,0)-" + total + ",FlightEarly=isnull(FlightEarly,0)+" + early + ",FlightLate=isnull(FlightLate,0)+" + late + "  where id=" + dto.crewId);
                context.Database.ExecuteSqlCommand("update employee set flightsum=isnull(flightsum,0)-" + total + "  where id=" + fdp.CrewId);

            }
            var related = await context.FDPs.Where(q => q.FDPId == fdpId).FirstOrDefaultAsync();
            if (related != null)
            {
                related.FDPId = null;
                related.FDPReportingTime = null;
                related.UPD = related.UPD != null ? related.UPD + 1 : 1;
            }

            if (fdp.FDPId != null)
            {
                var stby = await context.FDPs.FirstOrDefaultAsync(q => q.Id == fdp.FDPId);
                if (stby != null)
                {
                    stby.FDPId = null;
                    stby.FDPReportingTime = null;
                    stby.UPD = stby.UPD == null ? 1 : ((int)stby.UPD) + 1;
                }
            }


            var templateId = fdp.TemplateId;
            context.FDPs.Remove(fdp);
            context.Database.ExecuteSqlCommand("Delete from TableDutyFDP where FDPId=" + fdp.Id);
            context.Database.ExecuteSqlCommand("Delete from TableFlightFDP where FDPId=" + fdp.Id);

            var saveResult = await context.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(total);
        }


        [Route("api/update/fdp/flight/{id}")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> UpdateFDPByFlight(int id)
        {   //02-05

            //*********************
            //UPDATE FOR ALL DH
            //***********************
            double default_reporting = 75;
            var context = new Models.dbEntities();
            var utcdiff = Convert.ToInt32(ConfigurationManager.AppSettings["utcdiff"]);
            var fdps = await (from x in context.FDPItems
                              join y in context.FDPs on x.FDPId equals y.Id
                              where x.FlightId == id && y.CrewId != null
                              select y).ToListAsync();
            var fdpIds = fdps.Select(q => q.Id).ToList();
            var fdpitems = await context.FDPItems.Where(q => fdpIds.Contains(q.FDPId)).ToListAsync();
            var fltIds = fdpitems.Select(q => q.FlightId).ToList();
            var flts = await context.SchFlights.Where(q => fltIds.Contains(q.ID)).ToListAsync();

            foreach (var fdp in fdps)
            {
                var items = fdpitems.Where(q => q.FDPId == fdp.Id).ToList();
                var notSectorIds = items.Where(q => q.IsPositioning == true || q.IsSector == false || q.IsOff == true).Select(q => q.FlightId).ToList();
                var flt_ids = items.Select(q => q.FlightId).ToList();
                var sectors = items.Where(q => q.IsPositioning != true && q.IsSector != false && q.IsOff != true).Count();
                var flights = flts.Where(q => flt_ids.Contains(q.ID)).OrderBy(q => q.ChocksOut).ToList();
                var reporting = fdp.ReportingTime == null ? fdp.DateStart : fdp.ReportingTime;
                if (fdp.STD == flights.First().STD)
                {
                    //the reporting time does not change

                    reporting = ((DateTime)reporting).AddMinutes(utcdiff);


                }
                else
                {
                    //reporting time changes
                    reporting = ((DateTime)flights.First().ChocksOut).AddMinutes(-default_reporting);
                    fdp.ReportingTime = reporting;
                    fdp.DateStart = reporting;
                    fdp.InitStart = reporting;

                    reporting = ((DateTime)reporting).AddMinutes(utcdiff);
                }


                var maxFdp = GetMaxFDP2((DateTime)reporting, sectors);
                fdp.MaxFDP = maxFdp;
                var fdp_duration = ((DateTime)flights.Last().ChocksIn - (DateTime)reporting).TotalMinutes;
                if (fdp_duration > maxFdp)
                    fdp.IsOver = true;
                //initflts 5824,5825
                //initroute THR,KIH,THR
                var rts = flights.Select(q => q.FromAirportIATA).ToList();
                rts.Add(flights.Last().ToAirportIATA);
                fdp.InitRoute = string.Join(",", rts);
                //initfromiata
                //inittoiata
                fdp.InitFromIATA = flights.First().FromAirportId.ToString();
                fdp.InitToIATA = flights.Last().ToAirportId.ToString();
                //initno 5824_5825
                fdp.InitNo = string.Join("_", flights.Select(q => q.FlightNumber).ToList());
                fdp.InitFlights = string.Join("*", RosterFDPDto.getFlightsStrs(flights, items.Where(q => q.IsPositioning == true).Select(q => q.FlightId).ToList()));
                fdp.DateEnd = ((DateTime)flights.Last().ChocksIn).AddMinutes(30);
                fdp.InitEnd = ((DateTime)flights.Last().ChocksIn).AddMinutes(30);
                var rst = fdp.InitHomeBase != flights.Last().ToAirportId ? 10 : 12;
                fdp.InitRestTo = ((DateTime)flights.Last().ChocksIn).AddMinutes(30).AddHours(rst);
                //initflights 540302_0_202302051740_202302051930_5824_THR_KIH*540303_0_202302052030_202302052215_5825_KIH_THR



                fdp.STD = flights.First().STD;
                fdp.STA = flights.Last().STA;



                AddToCumDuty(fdp, context);
                AddToCumFlight(fdp, flights.Where(q => !notSectorIds.Contains(q.ID)).OrderBy(q => q.ChocksOut).ToList(), items, context);

            }


            // context.Database.ExecuteSqlCommand("Delete from TableFlightFDP where FDPId=" + fdp.Id);

            var saveResult = await context.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            return Ok(true);
        }

        [Route("api/duty/save")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> AddDuty(dynamic dto)
        {
            var context = new Models.dbEntities();
            var duty = new FDP();
            DateTime _date = Convert.ToDateTime(dto.DateStart);
            _date = _date.Date;
            duty.DateStart = Convert.ToDateTime(dto.DateStart);
            duty.DateEnd = Convert.ToDateTime(dto.DateEnd);
            duty.CityId = Convert.ToInt32(dto.CityId) == -1 ? Convert.ToInt32(dto.CityId) : null;
            duty.CrewId = Convert.ToInt32(dto.CrewId);
            duty.DutyType = Convert.ToInt32(dto.DutyType);
            duty.GUID = Guid.NewGuid();
            duty.IsTemplate = false;
            duty.Remark = dto.Remark != null ? Convert.ToString(dto.Remark) : "";
            duty.UPD = 1;

            duty.InitStart = duty.DateStart;
            duty.InitEnd = duty.DateEnd;
            //  var rest = new List<int>() { 1167, 1168, 1170, 5000, 5001, 100001, 100003, 300010 };
            //  duty.InitRestTo = rest.Contains(duty.DutyType) ? ((DateTime)duty.InitEnd).AddHours(12) : duty.DateEnd;
            duty.InitRestTo = 0;
            //  var _bl = Convert.ToInt32(dto.BL);
            //if (_bl != 0)
            //{
            //    duty.TableBlockTimes.Add(new TableBlockTime()
            //    {
            //        BlockTime = _bl,
            //        CDate = _date,
            //        CrewId = duty.CrewId,


            //    });
            //    duty.BL = _bl;
            //}
            //var _fx = Convert.ToInt32(dto.FX);
            //if (_fx != 0)
            //{
            //    duty.FX = _fx;
            //}
            //1166: Dayoff 100003:sim 5000:trn 5001:ofc 10025:mission 300009:rest
            var _interupted = await context.FDPs.FirstOrDefaultAsync(q =>

                                             q.Id != duty.Id && q.CrewId == duty.CrewId
                                             && (

                                                   (duty.InitStart >= q.InitStart && duty.InitRestTo <= q.InitRestTo)
                                                   || (q.InitStart >= duty.InitStart && q.InitRestTo <= duty.InitRestTo)
                                                   || (q.InitStart >= duty.InitStart && q.InitStart < duty.InitRestTo)
                                                   || (q.InitRestTo > duty.InitStart && q.InitRestTo <= duty.InitRestTo)
                                                 )
                                              );
            var _interupted_norest = await context.FDPs.FirstOrDefaultAsync(q =>

                                            q.Id != duty.Id && q.CrewId == duty.CrewId
                                            && (

                                                  (duty.InitStart >= q.InitStart && duty.InitRestTo <= q.InitEnd)
                                                  || (q.InitStart >= duty.InitStart && q.InitEnd <= duty.InitRestTo)
                                                  || (q.InitStart >= duty.InitStart && q.InitStart < duty.InitRestTo)
                                                  || (q.InitEnd > duty.InitStart && q.InitEnd <= duty.InitRestTo)
                                                )
                                             );

            switch (duty.DutyType)
            {
                case 100000: //ground

                case 100004: //exp lic
                case 100005: //exp med
                case 100006: //exp pass
                case 100007: //no flt
                    if (_interupted_norest!=null && 
                        (_interupted_norest.DutyType == 1165 
                        || _interupted_norest.DutyType == 1167 
                        || _interupted_norest.DutyType == 1170 
                        || _interupted_norest.DutyType == 1168 
                        || _interupted_norest.DutyType == 300010))//other airline stby
                        return new CustomActionResult(HttpStatusCode.OK, new
                        {
                            Code = 406,
                            message = "Interruption Error." + (_interupted.InitStart == null ? "" : ((DateTime)_interupted.InitStart).ToString("yyyy-MM-dd") + " " + _interupted.InitFlts + " " + _interupted.InitRoute)

                        });
                    break;
                case 100002: //sick
                case 100008: //req off
                case 1166:   //day off
                case 1169:   //vacation
                    if (_interupted_norest != null &&
                       (_interupted_norest.DutyType == 1165
                       || _interupted_norest.DutyType == 1167
                       || _interupted_norest.DutyType == 1170
                       || _interupted_norest.DutyType == 1168
                       || _interupted_norest.DutyType == 300010 //ostby
                       || _interupted_norest.DutyType == 5000
                       || _interupted_norest.DutyType == 5001
                       || _interupted_norest.DutyType == 100001 //meeting
                        || _interupted_norest.DutyType == 100025 //mission

                       ))//other airline stby
                        return new CustomActionResult(HttpStatusCode.OK, new
                        {
                            Code = 406,
                            message = "Interruption Error." + (_interupted.InitStart == null ? "" : ((DateTime)_interupted.InitStart).ToString("yyyy-MM-dd") + " " + _interupted.InitFlts + " " + _interupted.InitRoute)

                        });
                    break;
                case 5000://trn
                case 5001: //office
                case 100001: //meeting
                    if (_interupted  != null &&
                       (_interupted.DutyType == 1165
                       || _interupted.DutyType == 1167
                       || _interupted.DutyType == 1170
                       || _interupted.DutyType == 1168
                       || _interupted.DutyType == 300010 //ostby
                       || _interupted.DutyType == 5000
                       || _interupted.DutyType == 5001
                       || _interupted.DutyType == 100001 //meeting
                        || _interupted.DutyType == 100025 //mission
                        || _interupted.DutyType == 100002 //sick
                        || _interupted.DutyType == 100008  //req off
                        || _interupted.DutyType == 1166 //day off
                        || _interupted.DutyType == 1169  //vacation
                        || _interupted_norest.DutyType == 300010 //other airline stby
                       ))
                        return new CustomActionResult(HttpStatusCode.OK, new
                        {
                            Code = 406,
                            message = "Interruption Error." + (_interupted.InitStart == null ? "" : ((DateTime)_interupted.InitStart).ToString("yyyy-MM-dd") + " " + _interupted.InitFlts + " " + _interupted.InitRoute)

                        });
                    break;
                case 300010://other airline stby
                case 100025://mission
                    var types = new List<int>() {1165,1167,1168,1170,5000,5001,100001,100025,100002,100008,1166,1169 };
                    if (_interupted != null && types.IndexOf(_interupted.DutyType)!=-1)
                        return new CustomActionResult(HttpStatusCode.OK, new
                        {
                            Code = 406,
                            message = "Interruption Error." + (_interupted.InitStart == null ? "" : ((DateTime)_interupted.InitStart).ToString("yyyy-MM-dd") + " " + _interupted.InitFlts + " " + _interupted.InitRoute)

                        });
                    break;
                default:
                    break;
            }



            this.context.FDPs.Add(duty);
            return duty;
        }

        [Route("api/roster/stby/save")]
        [AcceptVerbs("POST")]

        public async Task<IHttpActionResult> PostRosterSTBYSave(dynamic dto)
        {
            var context = new Models.dbEntities();
            var sbam_start = 0 * 60;
            var sbam_durattion = 11 * 60 + 59;
            var sbpm_start = 12 * 60;
            var sbpm_duration = 11 * 60 + 59;
            var res_start = 4 * 60;
            var res_duration = 17 * 60 + 59;


            DateTime day = (Convert.ToDateTime(dto.date));
            var crewId = Convert.ToInt32(dto.crewId);
            var type = Convert.ToInt32(dto.type);

            //var start = day;
            //var end = day.AddHours(12);
            //if (type == 1167)
            //{
            //    start = day.AddHours(12);
            //    end = start.AddHours(12);
            //}
            //if (type == 1170)
            //{
            //    end = start.AddHours(23).AddMinutes(59).AddSeconds(59);
            //}

            //caspian
            var start = day.AddMinutes(sbam_start);
            var end = start.AddMinutes(sbam_durattion);
            if (type == 1167)
            {
                start = day.AddMinutes(sbpm_start);
                end = start.AddMinutes(sbpm_duration);
            }
            if (type == 1170)
            {
                start = day.AddMinutes(res_start);
                end = start.AddMinutes(res_duration);
            }

            var duty = new FDP();
            duty.DateStart = start;
            duty.DateEnd = end;
            var duration = (end - start).TotalMinutes;
            duty.CityId = Convert.ToInt32(dto.CityId); //Convert.ToInt32(dto.CityId) == -1 ? Convert.ToInt32(dto.CityId) : null;
            var homeBase = Convert.ToInt32(dto.HomeBase);
            var outOfBase = false;
            if (duty.CityId != -1 && homeBase != duty.CityId)
            {
                outOfBase = true;
            }
            var rest = duration >= 720 ? duration : (outOfBase ? 600 : 720);
            duty.CrewId = crewId;
            duty.DutyType = type;
            duty.GUID = Guid.NewGuid();
            duty.IsTemplate = false;
            //duty.Remark = dto.Remark != null ? Convert.ToString(dto.Remark) : "";
            duty.UPD = 1;

            duty.InitStart = duty.DateStart;
            duty.InitEnd = duty.DateEnd;
            // var rest = new List<int>() { 1167, 1168, 1170, 5000, 5001, 100001, 100003 };
            //duty.InitRestTo = rest.Contains(duty.DutyType) ? ((DateTime)duty.InitEnd).AddHours(12) : duty.DateEnd;
            if (duty.DutyType == 1167 || duty.DutyType == 1168)
                duty.InitRestTo = ((DateTime)duty.InitEnd).AddMinutes(rest);
            else
                duty.InitRestTo = duty.DateEnd;
            //porn
            var exc = new List<int>() { 100009, 100020, 100021, 100022, 100023, 1170 };
            var check = new List<int>() { 1165, 1166, 1167, 1168, 1169, 5000, 5001, 100000, 100002, 100003, 100004, 100005, 100006, 100008, 100025, 300008, 300009, 300010 };
            // var _interupted = await this.context.FDPs.FirstOrDefaultAsync(q => !exc.Contains(q.DutyType) && q.CrewId == duty.CrewId
            // && (
            //       (duty.InitStart >= q.InitStart && duty.InitStart <= q.InitRestTo)
            //    || (duty.InitEnd >= q.InitStart && duty.InitEnd <= q.InitRestTo)
            //    || (q.InitStart >= duty.InitStart && q.InitRestTo <= duty.InitRestTo)
            //   )
            //);
            var _interupted = await context.FDPs.FirstOrDefaultAsync(q =>
                                               check.Contains(q.DutyType) &&
                                               q.Id != duty.Id && q.CrewId == duty.CrewId
                                               && (

                                                     (duty.InitStart >= q.InitStart && duty.InitRestTo <= q.InitRestTo)
                                                     || (q.InitStart >= duty.InitStart && q.InitRestTo <= duty.InitRestTo)
                                                     || (q.InitStart >= duty.InitStart && q.InitStart < duty.InitRestTo)
                                                     || (q.InitRestTo > duty.InitStart && q.InitRestTo <= duty.InitRestTo)
                                                   )
                                                );

            if (_interupted != null)
            {
                //Rest/Interruption Error
                return new CustomActionResult(HttpStatusCode.OK, new
                {
                    Code = 406,
                    message = "Rest/Interruption Error." + (_interupted.InitStart == null ? "" : ((DateTime)_interupted.InitStart).ToString("yyyy-MM-dd") + " " + _interupted.InitFlts + " " + _interupted.InitRoute)

                });
                //return new CustomActionResult(HttpStatusCode.NotAcceptable, _interupted);
            }


            context.FDPs.Add(duty);
            var saveResult = await context.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            AddToCumDuty(duty);

            //2020-11-22 noreg
            var view = await context.ViewCrewDutyNoRegs.FirstOrDefaultAsync(q => q.Id == duty.Id);
            return new CustomActionResult(HttpStatusCode.OK, view);

        }

        [Route("api/roster/fdp/save")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> SaveFDP(RosterFDPDto dto)
        {
            if (dto.Id == -100)
            {
                dto.ids = new List<RosterFDPId>() { new RosterFDPId() { dh = 0, id = 539799 }, new RosterFDPId() { dh = 0, id = 539928 } };
                dto.extension = 0;
                dto.IsAdmin = 0;
                dto.UserName = "demo";
                dto.crewId = 4484;
                dto.from = 141866;
                dto.group = "TRE";
                dto.homeBase = 135502;
                dto.index = 1;
                dto.key = "539799_539928";
                dto.maxFDP = 780;
                dto.no = "909_910";
                dto.rank = "rank";
                dto.scheduleName = "scheduleName";
                dto.split = false;
                dto.to = 141866;


            }
            double default_reporting = 75;
            var context = new Models.dbEntities();
            try
            {
                var _x_fltids = dto.ids.Select(q => q.id).ToList();
                var _x_flights = context.SchFlights.Where(q => _x_fltids.Contains(q.ID)).OrderBy(q => q.STD).ToList();
                dto.IsAdmin = dto.IsAdmin == null ? 0 : (int)dto.IsAdmin;
                Stopwatch timer = new Stopwatch();
                timer.Start();
                dto.items = RosterFDPDto.getItemsX(_x_flights, dto.ids); //RosterFDPDto.getItems(dto.flights);
                dto.no = string.Join("_", _x_flights.Select(q => q.FlightNumber).ToList());
                dto.key = string.Join("_", _x_flights.Select(q => q.ID.ToString()).ToList());
                dto.from = (int)_x_flights.First().FromAirportId;
                dto.to = (int)_x_flights.Last().ToAirportId;
                dto.flights = RosterFDPDto.getFlightsStrs(_x_flights, dto.ids);



                bool alldh = dto.items.Where(q => q.dh == 0).Count() == 0;
                var fdpDuty = dto.getDuty(default_reporting);
                var fdpFlight = dto.getFlight();
                var stdday = dto.items[0].std.Date;
                //var dutyFlight = await this.context.ViewDayDutyFlights.FirstOrDefaultAsync(q => q.Date == stdday);
                //magu210



                var _d1 = stdday;
                var _d2 = stdday.AddDays(-6);
                var _df1 = stdday;
                var _df2 = stdday.AddDays(-27);
                var ncrewid = (Nullable<int>)dto.crewId;

                var ftlDateFrom = stdday.Date;
                var ftlDate7 = stdday.AddDays(6);
                var ftlDate14 = stdday.AddDays(13);
                var ftlDate28 = stdday.AddDays(27);
                var ftlDate12M = stdday.AddMonths(11);

                var ftlYearFrom = new DateTime(stdday.Year, 1, 1);
                var ftlYearTo = (new DateTime(stdday.Year + 1, 1, 1)).AddDays(-1);


                //var _d7 = await this.context.TableDutyFDPs.Where(q => q.CrewId == ncrewid && q.CDate >= _d2 && q.CDate <= _d1).Select(q => q.DurationLocal).SumAsync();
                var _d7 = await context.AppFTLs.Where(q => q.CrewId == ncrewid && q.CDate >= ftlDateFrom && q.CDate <= ftlDate7 && q.Duty7 > 60 * 60 - fdpDuty).FirstOrDefaultAsync();
                var _d14 = await context.AppFTLs.Where(q => q.CrewId == ncrewid && q.CDate >= ftlDateFrom && q.CDate <= ftlDate14 && q.Duty14 > 110 * 60 - fdpDuty).FirstOrDefaultAsync();
                var _d28 = await context.AppFTLs.Where(q => q.CrewId == ncrewid && q.CDate >= ftlDateFrom && q.CDate <= ftlDate28 && q.Duty28 > 190 * 60 - fdpDuty).FirstOrDefaultAsync();

                var _f28 = await context.AppFTLs.Where(q => q.CrewId == ncrewid && q.CDate >= ftlDateFrom && q.CDate <= ftlDate28 && q.Flight28 > 100 * 60 - fdpFlight).FirstOrDefaultAsync();
                var _fcy = await context.AppFTLs.Where(q => q.CrewId == ncrewid && q.CDate == stdday && q.FlightCYear > 900 * 60 - fdpFlight).FirstOrDefaultAsync();
                var _fy = await context.AppFTLs.Where(q => q.CrewId == ncrewid && q.CDate >= ftlDateFrom && q.CDate <= ftlDate12M && q.FlightYear > 1000 * 60 - fdpFlight).FirstOrDefaultAsync();

                double flight28 = 0;


                if (_d7 != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.OK, new
                    {
                        Code = 307,
                        message = "Flight Time/Duty Limitaion Error. "
                        + "7-Day Duty: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d7.Duty7 + fdpDuty))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d7.Duty7 + fdpDuty))) % 60)
                        + " on " + _d7.CDate.ToString("yyy-MMM-dd")
                    });
                }
                if (_d14 != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.OK, new
                    {
                        Code = 314,
                        message = "Flight Time/Duty Limitaion Error. "
                        + "14-Day Duty: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d14.Duty14 + fdpDuty))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d14.Duty14 + fdpDuty))) % 60)
                        + " on " + _d14.CDate.ToString("yyy-MMM-dd")
                    });
                }
                if (_d28 != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {

                    return new CustomActionResult(HttpStatusCode.OK, new
                    {
                        Code = 328,
                        message = "Flight Time/Duty Limitaion Error. "
                        + "28-Day Duty: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d28.Duty28 + fdpDuty))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d28.Duty28 + fdpDuty))) % 60)
                        + " on " + _d28.CDate.ToString("yyy-MMM-dd")
                    });
                }

                if (_f28 != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.OK, new
                    {
                        Code = 428,
                        message = "Flight Time/Duty Limitaion Error. "
                        + "28-Day Flight: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_f28.Flight28 + fdpFlight))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_f28.Flight28 + fdpFlight))) % 60)
                        + " on " + _f28.CDate.ToString("yyy-MMM-dd")
                    });
                }

                if (_fy != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.OK, new
                    {
                        Code = 412,
                        message = "Flight Time/Duty Limitaion Error. "
                        + "12-Month Flight: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_fy.FlightYear + fdpFlight))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_fy.FlightYear + fdpFlight))) % 60)
                        + " on " + _fy.CDate.ToString("yyy-MMM-dd")
                    });
                }

                if (_fcy != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.OK, new
                    {
                        Code = 413,
                        message = "Flight Time/Duty Limitaion Error. "
                        + "Current-Year Flight: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_fcy.FlightCYear + fdpFlight))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_fcy.FlightCYear + fdpFlight))) % 60)
                        + " on " + _fcy.CDate.ToString("yyy-MMM-dd")
                    });
                }
                //if (flight28 + fdpFlight > 100 * 60 && (dto.IsAdmin == null || dto.IsAdmin == 0))
                //{
                //    return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                //    {
                //        message = "Flight Time/Duty Limitaion Error. "
                //       + "28-Day Flights: " + FormatTwoDigits(Convert.ToInt32(Math.Floor(flight28 + fdpFlight)) / 60) + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(flight28 + fdpFlight)) % 60)
                //    });
                //}
                ////////////////////////////////////


                //(from x in this.context.ViewDayDutyFlights
                // where x.Date >= dateStart && x.Date <= dateEnd && crewIds.Contains(x.CrewId)
                // select x).ToList();



                dto.flts = string.Join(",", dto.items.Select(q => q.no + (q.dh == 1 ? "(dh)" : "")).ToList());
                var rts = dto.items.Select(q => q.from).ToList();
                //if (dto.items.Count > 1)
                rts.Add(dto.items.Last().to);
                dto.route = string.Join(",", rts);
                // if (dto.items.Count= 1)


                var keyParts = dto.items.Select(q => q.flightId + (q.dh == 1 ? "*1" : "*0")).ToList();
                var rst = dto.to == dto.homeBase ? 12 : 10;

                // if (dto.extension != null && dto.extension > 0)
                //     rst += 4;

                var fdp = new FDP()
                {
                    IsTemplate = false,
                    DutyType = 1165,
                    CrewId = dto.crewId,
                    GUID = Guid.NewGuid(),
                    JobGroupId = RosterFDPDto.getRank(dto.rank),
                    FirstFlightId = dto.items.First().flightId,
                    LastFlightId = dto.items.Last().flightId,
                    Key = string.Join("_", keyParts),
                    InitFlts = dto.flts,
                    InitRoute = dto.route,
                    InitFromIATA = dto.from.ToString(),
                    InitToIATA = dto.to.ToString(),
                    InitStart = !alldh ? dto.items.First().offblock.AddMinutes(-default_reporting) : dto.items.First().offblock,
                    InitEnd = !alldh ? dto.items.Last().onblock.AddMinutes(30) : dto.items.Last().onblock,
                    DateStart = !alldh ? dto.items.First().offblock.AddMinutes(-default_reporting) : dto.items.First().offblock,
                    DateEnd = !alldh ? dto.items.Last().onblock.AddMinutes(30) : dto.items.Last().onblock,
                    InitRestTo = !alldh ? dto.items.Last().onblock.AddMinutes(30).AddHours(rst) : dto.items.Last().onblock,
                    InitFlights = string.Join("*", dto.flights),
                    InitGroup = dto.group,
                    InitHomeBase = dto.homeBase,
                    InitIndex = dto.index,
                    InitKey = dto.key,
                    InitNo = dto.no,
                    InitRank = dto.rank,
                    InitScheduleName = dto.scheduleName,
                    Extension = dto.extension,
                    Split = 0,
                    UserName = dto.UserName,
                    MaxFDP = dto.maxFDP,
                    ReportingTime = !alldh ? dto.items.First().offblock.AddMinutes(-default_reporting) : dto.items.First().offblock,
                    IsOver = false,
                    STD = dto.items.First().std,
                    STA = dto.items.Last().sta,
                    OutOfHomeBase = dto.to != dto.homeBase,



                };
                fdp.ActualEnd = fdp.InitEnd;
                fdp.ActualStart = fdp.InitStart;
                fdp.ActualRestTo = fdp.InitRestTo;
                fdp.FDPExtras.Add(new FDPExtra() { MaxFDP = dto.maxFDP });
                //Check Extension //////////////////
                if (fdp.Extension != null && fdp.Extension > 0)
                {
                    var exd1 = (DateTime)fdp.InitStart;
                    var exd0 = exd1.AddHours(-168);
                    var extcnt = await context.ExtensionHistories.Where(q => q.CrewId == fdp.CrewId && (q.DutyStart >= exd0 && q.DutyStart <= exd1)).CountAsync();
                    if (extcnt >= 2)
                    {
                        return new CustomActionResult(HttpStatusCode.OK, new
                        {
                            Code = 110,
                            message = "You can't extend maximum daily FDP due to the crew extended fdps in 7 consecutive days."

                        });
                    }
                }
                //4-11
                //Check interuption/////////////////
                var exc = new List<int>() { 1166, 1169, /*10000, 10001, 100000, 100002, 100004, 100005, 100006,*/
                    100007, 100008, 100024, 100025, 100009, 100020, 100021, 100022, 100023, 200000, 200001, 200002, 200003, 200004, 200005
                ,300000
                ,300001
                ,300002
                ,300003
                ,300004
                ,300005
                ,300006
                ,300007
                ,300008

                ,300010
                ,300011
                ,300012
                ,300013
                ,300014


                ,1167
                ,1168
                ,1170
                ,5001
                };
                var stbys = new List<int>() { 1167, 1168, 1170 };
                //if (!alldh)
                {
                    FDP _interupted = null;

                    _interupted = await context.FDPs.FirstOrDefaultAsync(q =>
                                                !exc.Contains(q.DutyType) &&
                                                q.Id != fdp.Id && q.CrewId == fdp.CrewId
                                                && (
                                                      //(fdp.InitStart >= q.InitStart && fdp.InitStart < q.InitRestTo)
                                                      // || (fdp.InitEnd >= q.InitStart && fdp.InitEnd <= q.InitRestTo)
                                                      // || (q.InitStart >= fdp.InitStart && q.InitStart < fdp.InitRestTo)
                                                      // || (q.InitRestTo > fdp.InitStart && q.InitRestTo <= fdp.InitRestTo)
                                                      (fdp.InitStart >= q.InitStart && fdp.InitRestTo <= q.InitRestTo)
                                                      || (q.InitStart >= fdp.InitStart && q.InitRestTo <= fdp.InitRestTo)
                                                      || (q.InitStart >= fdp.InitStart && q.InitStart < fdp.InitRestTo)
                                                      || (q.InitRestTo > fdp.InitStart && q.InitRestTo <= fdp.InitRestTo)
                                                    )
                                                );
                    if (_interupted == null)
                    {
                        _interupted = await context.FDPs.FirstOrDefaultAsync(q => stbys.Contains(q.DutyType) && q.Id != fdp.Id && q.CrewId == fdp.CrewId
                          && (
                               ((q.InitStart > fdp.InitStart) && ((fdp.InitEnd > q.InitStart && fdp.InitEnd < q.InitRestTo) || (fdp.InitRestTo > q.InitStart && fdp.InitRestTo < q.InitRestTo)))
                               ||
                               ((q.InitRestTo > fdp.InitStart && q.InitRestTo < fdp.InitRestTo) && !(fdp.InitStart >= q.InitStart && fdp.InitStart < q.InitEnd))

                          )


                        );

                    }
                    bool _activeq = false;
                    if (_interupted == null)
                    {
                        _interupted = await context.FDPs.FirstOrDefaultAsync(q => stbys.Contains(q.DutyType) && q.Id != fdp.Id && q.CrewId == fdp.CrewId
                          && (fdp.InitStart >= q.InitStart && fdp.InitStart < q.InitEnd)

                        );
                        _activeq = _interupted != null;

                    }
                    //WHAT IS THIS?
                    bool _interuptedAcceptable = dto.DeletedFDPId == null ? true : dto.DeletedFDPId != _interupted.Id;

                    if (_interupted != null && _interuptedAcceptable /* && (dto.IsAdmin == null || dto.IsAdmin == 0)*/)
                    {
                        //Rest/Interruption Error
                        if ((dto.IsAdmin == null || dto.IsAdmin == 0)
                            && (_activeq && _interupted.DutyType != 1167 && _interupted.DutyType != 1168 && _interupted.DutyType != 1170)
                            || !(dto.items.First().std >= _interupted.DateStart && dto.items.First().std <= _interupted.DateEnd))
                        {
                            //if (false)
                            bool sendError = false;
                            switch (_interupted.DutyType)
                            {
                                case 10000:
                                case 100000:
                                case 100002:
                                case 100004:
                                case 100005:
                                case 100006:
                                    if ((new List<int>() { 10000, 10001, 1169, 100000, 100002, 100004, 100005, 100006, 100007, 100008, 100009, 100020, 100021, 100022, 100023, 100024, 200000, 200001, 200002, 200003, 200004, 200005 }).IndexOf(fdp.DutyType) == -1)
                                    {
                                        if ((fdp.InitStart >= _interupted.InitStart && fdp.InitStart <= _interupted.InitEnd)
                                            || (fdp.InitEnd >= _interupted.InitStart && fdp.InitEnd <= _interupted.InitEnd)
                                            || (_interupted.InitStart >= fdp.InitStart && _interupted.InitStart <= fdp.InitEnd)
                                            || (_interupted.InitEnd >= fdp.InitStart && _interupted.InitEnd <= fdp.InitEnd)
                                            )
                                            sendError = true;
                                    }
                                    break;
                                case 1165:
                                    if (alldh)
                                    {
                                        try
                                        {
                                            var intStart = ((DateTime)_interupted.InitStart).AddMinutes(60);
                                            var intEnd = ((DateTime)_interupted.InitEnd).AddMinutes(-30);
                                            if (fdp.InitStart > intStart && fdp.InitStart < intEnd)
                                                sendError = true;
                                            else if (fdp.InitEnd > intStart && fdp.InitEnd < intEnd)
                                                sendError = true;
                                            else if (fdp.InitStart == intStart && fdp.InitEnd == intEnd)
                                                sendError = true;
                                            else if (intStart >= fdp.InitStart && intStart <= fdp.InitEnd)
                                                sendError = true;
                                            else if (intEnd >= fdp.InitStart && intEnd <= fdp.InitEnd)
                                                sendError = true;
                                            else
                                                sendError = false;

                                        }
                                        catch (Exception exxx)
                                        {
                                            sendError = false;
                                        }

                                    }
                                    else
                                    {
                                        if (_interupted.InitNo.Contains("*1"))
                                        {
                                            var nodh = await context.FDPItems.Where(q => q.FDPId == _interupted.Id && q.IsPositioning != true).CountAsync();
                                            if (nodh > 0)
                                                sendError = true;
                                            else
                                            {
                                                var fdpStart = dto.items.First().std;
                                                var fdpEnd = dto.items.Last().sta;
                                                if (_interupted.InitStart >= fdpStart && _interupted.InitStart <= fdpEnd)
                                                    sendError = true;
                                                else if (_interupted.InitEnd >= fdpStart && _interupted.InitEnd <= fdpEnd)
                                                    sendError = true;
                                                else if (fdpStart >= _interupted.InitStart && fdpStart <= _interupted.InitEnd)
                                                    sendError = true;
                                                else if (fdpEnd >= _interupted.InitStart && fdpEnd <= _interupted.InitEnd)
                                                    sendError = true;
                                                else
                                                    sendError = false;
                                            }
                                        }
                                        else
                                            sendError = true;
                                    }

                                    break;
                                default:
                                    sendError = true;
                                    break;
                            }
                            if (sendError && (dto.IsAdmin == null || dto.IsAdmin == 0))
                                return new CustomActionResult(HttpStatusCode.OK, new
                                {
                                    Code = 406,
                                    message = "Rest/Interruption Error(" + _interupted.Id + "). " + (GetDutyTypeTitle(_interupted.DutyType)) + "   " + (_interupted.InitStart == null ? "" : ((DateTime)_interupted.InitStart).ToString("yyyy-MM-dd") + " " + _interupted.InitFlts + " " + _interupted.InitRoute)

                                });
                        }
                        else
                        {
                            if (_interupted.DutyType == 1167 || _interupted.DutyType == 1168)
                                return new CustomActionResult(HttpStatusCode.OK, new { Code = 501, data = _interupted });
                            else if (_interupted.DutyType == 5000)
                                return new CustomActionResult(HttpStatusCode.OK, new
                                {
                                    Code = 406,
                                    message = "Interruption Error (TRAINING)"

                                });
                            else
                            {
                                var _strt = ((DateTime)fdp.InitStart).AddMinutes(60);
                                var rdif = Math.Abs((DateTime.UtcNow - _strt).TotalMinutes);
                                if (rdif < 10 * 60)
                                    return new CustomActionResult(HttpStatusCode.OK, new { Code = 304, data = _interupted });
                            }
                        }
                        //return new CustomActionResult(HttpStatusCode.NotAcceptable, _interupted);
                    }
                }

                ///////////////////////////////
                if (dto.DeletedFDPId != null)
                {
                    //await DeleteFDP((int)dto.DeletedFDPId);
                    var dfdp = await context.FDPs.FirstOrDefaultAsync(q => q.Id == dto.DeletedFDPId);
                    double total = 0;
                    if (!string.IsNullOrEmpty(dfdp.InitFlights))
                    {
                        var parts = dfdp.InitFlights.Split('*').ToList();

                        foreach (var x in parts)
                        {
                            var _std = x.Split('_')[2];
                            var _sta = x.Split('_')[3];
                            var std = DateTime.ParseExact(_std, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                            var sta = DateTime.ParseExact(_sta, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                            total += (sta - std).TotalMinutes;
                        }
                        //this.context.Database.ExecuteSqlCommand("update employee set flightsum=isnull(flightsum,0)-" + total + ",FlightEarly=isnull(FlightEarly,0)+" + early + ",FlightLate=isnull(FlightLate,0)+" + late + "  where id=" + dto.crewId);
                        context.Database.ExecuteSqlCommand("update employee set flightsum=isnull(flightsum,0)-" + total + "  where id=" + dfdp.CrewId);

                    }
                    var related = await context.FDPs.Where(q => q.FDPId == dto.DeletedFDPId).FirstOrDefaultAsync();
                    if (related != null)
                    {
                        related.FDPId = null;
                        related.FDPReportingTime = null;
                        related.UPD = related.UPD != null ? related.UPD + 1 : 1;
                    }

                    if (dfdp.FDPId != null)
                    {
                        var stby = await context.FDPs.FirstOrDefaultAsync(q => q.Id == dfdp.FDPId);
                        if (stby != null)
                        {
                            stby.FDPId = null;
                            stby.FDPReportingTime = null;
                            stby.UPD = stby.UPD == null ? 1 : ((int)stby.UPD) + 1;
                        }
                    }


                    var templateId = dfdp.TemplateId;
                    context.FDPs.Remove(dfdp);


                }
                ////////////////////////////
                bool saveTemp = false;
                var tkey = string.Join("_", keyParts);
                var temp = await context.FDPs.FirstOrDefaultAsync(q => q.IsTemplate && q.Key == tkey);
                if (temp == null)
                {
                    saveTemp = true;
                    temp = new FDP()
                    {
                        IsTemplate = true,
                        DutyType = 1165,
                        IsMain = true,
                        GUID = Guid.NewGuid(),

                        FirstFlightId = dto.items.First().flightId,
                        LastFlightId = dto.items.Last().flightId,
                        Key = string.Join("_", keyParts),
                        InitFlts = dto.flts,
                        InitRoute = dto.route,
                        InitFromIATA = dto.from.ToString(),
                        InitToIATA = dto.to.ToString(),
                        InitStart = dto.items.First().offblock.AddMinutes(default_reporting),
                        InitEnd = dto.items.Last().onblock.AddMinutes(30),
                        Split = 0,
                        UserName = dto.UserName,
                        MaxFDP = dto.maxFDP,
                        IsOver = false,

                    };
                    temp.FDPExtras.Add(new FDPExtra() { MaxFDP = dto.maxFDP });
                    context.FDPs.Add(temp);
                }

                double flightSum = 0;
                var firststd = ((DateTime)dto.items.First().offblock).ToLocalTime();
                var laststa = ((DateTime)dto.items.Last().onblock).ToLocalTime();
                int early = firststd.Hour <= 6 ? 1 : 0;
                int late = laststa.Hour >= 22 ? 1 : 0;
                foreach (var x in dto.items)
                {
                    flightSum += ((DateTime)x.onblock - (DateTime)x.offblock).TotalMinutes;
                    fdp.FDPItems.Add(new FDPItem()
                    {
                        FlightId = x.flightId,
                        IsPositioning = x.dh == 1,
                        IsSector = x.dh != 1,
                        PositionId = RosterFDPDto.getRank(dto.rank),
                        RosterPositionId = dto.index,

                    });
                    if (saveTemp)
                        temp.FDPItems.Add(new FDPItem()
                        {
                            FlightId = x.flightId,
                            IsPositioning = x.dh == 1,
                            IsSector = x.dh != 1,


                        });

                }

                var breakGreaterThan10Hours = string.Empty;
                if (dto.items.Count > 1 && dto.split)
                {
                    for (int i = 1; i < dto.items.Count; i++)
                    {
                        var dt = (DateTime)dto.items[i].offblock - (DateTime)dto.items[i - 1].onblock;
                        var minuts = dt.TotalMinutes;
                        // – (0:30 + 0:15 + 0:45)
                        var brk = minuts - 30 - 60; //30:travel time, post flight duty:15, pre flight duty:30
                        if (brk >= 600)
                        {
                            //var tfi = tflights.FirstOrDefault(q => q.ID == flights[i].ID);
                            // var tfi1 = tflights.FirstOrDefault(q => q.ID == flights[i - 1].ID);
                            breakGreaterThan10Hours = "The break is greater than 10 hours.";
                        }

                        else
                        if (brk >= 180)
                        {
                            var fdpitem = fdp.FDPItems.FirstOrDefault(q => q.FlightId == dto.items[i].flightId);
                            fdpitem.SplitDuty = true;
                            var pair = fdp.FDPItems.FirstOrDefault(q => q.FlightId == dto.items[i - 1].flightId);
                            pair.SplitDuty = true;
                            fdpitem.SplitDutyPairId = pair.FlightId;
                            fdp.Split += 0.5 * (brk);
                            ////////////////////////////////////////////////////
                            if (saveTemp)
                            {
                                var fdpitemTemp = temp.FDPItems.FirstOrDefault(q => q.FlightId == dto.items[i].flightId);
                                fdpitemTemp.SplitDuty = true;
                                var pairTemp = temp.FDPItems.FirstOrDefault(q => q.FlightId == dto.items[i - 1].flightId);
                                pairTemp.SplitDuty = true;
                                fdpitemTemp.SplitDutyPairId = pair.FlightId;
                                temp.Split += 0.5 * (brk);
                            }


                            //////////////////////////////////

                        }
                    }
                }

                var saveResult = new CustomActionResult(HttpStatusCode.OK, null);

                if (saveTemp)
                {
                    saveResult = await context.SaveAsync();
                }

                if (saveResult.Code != HttpStatusCode.OK)
                    return saveResult;
                fdp.TemplateId = temp.Id;

                //????? stby.UPD = stby.UPD == null ? 1 : ((int)stby.UPD) + 1;
                context.FDPs.Add(fdp);
                if (fdp.Extension != null && fdp.Extension > 0)
                {
                    fdp.ExtensionHistories.Add(new ExtensionHistory()
                    {
                        CrewId = (int)fdp.CrewId,
                        DutyStart = fdp.InitStart,
                        Extension = fdp.Extension,
                        Sectors = dto.items.Count,
                    });
                }

                context.Database.ExecuteSqlCommand("update employee set flightsum=isnull(flightsum,0)+" + flightSum + ",FlightEarly=isnull(FlightEarly,0)+" + early + ",FlightLate=isnull(FlightLate,0)+" + late + "  where id=" + dto.crewId);


                saveResult = await context.SaveAsync();
                if (saveResult.Code != HttpStatusCode.OK)
                    return saveResult;
                // var vfdp = await this.context.ViewFDPRests.FirstOrDefaultAsync(q => q.Id == fdp.Id);
                //  return new CustomActionResult(HttpStatusCode.OK, vfdp);

                AddToCumDuty(fdp);
                AddToCumFlight(fdp, dto.items);

                timer.Stop();
                var _ms = timer.Elapsed;
                var fdp_result = new FDPResult()
                {
                    BL = fdp.BL,
                    BoxId = fdp.BoxId,
                    CanceledNo = fdp.CanceledNo,
                    CanceledRoute = fdp.CanceledRoute,
                    CityId = fdp.CityId,
                    ConfirmedBy = fdp.ConfirmedBy,
                    CP = fdp.CP,
                    CrewId = fdp.CrewId,
                    CustomerId = fdp.CustomerId,
                    DateConfirmed = fdp.DateConfirmed,
                    DateContact = fdp.DateContact,
                    DateEnd = fdp.DateEnd,
                    DateStart = fdp.DateStart,
                    DelayAmount = fdp.DelayAmount,
                    DelayedReportingTime = fdp.DelayedReportingTime,
                    DutyType = fdp.DutyType,
                    Extension = fdp.Extension,
                    FDPId = fdp.FDPId,
                    FDPReportingTime = fdp.FDPReportingTime,
                    FirstFlightId = fdp.FirstFlightId,
                    FirstNotification = fdp.FirstNotification,
                    FX = fdp.FX,
                    GUID = fdp.GUID,
                    Id = fdp.Id,
                    InitEnd = fdp.InitEnd,
                    InitFlights = fdp.InitFlights,
                    InitFlts = fdp.InitFlts,
                    InitFromIATA = fdp.InitFromIATA,
                    InitGroup = fdp.InitGroup,
                    InitHomeBase = fdp.InitHomeBase,
                    InitIndex = fdp.InitIndex,
                    InitKey = fdp.InitKey,
                    InitNo = fdp.InitNo,
                    InitRank = fdp.InitRank,
                    InitRestTo = fdp.InitRestTo,
                    InitRoute = fdp.InitRoute,
                    InitScheduleName = fdp.InitScheduleName,
                    InitStart = fdp.InitStart,
                    InitToIATA = fdp.InitToIATA,
                    IsMain = fdp.IsMain,
                    IsTemplate = fdp.IsTemplate,
                    JobGroupId = fdp.JobGroupId,
                    Key = fdp.Key,
                    LastFlightId = fdp.LastFlightId,
                    LocationId = fdp.LocationId,
                    MaxFDP = fdp.MaxFDP,
                    NextNotification = fdp.NextNotification,
                    Remark = fdp.Remark,
                    Remark2 = fdp.Remark2,
                    ReportingTime = fdp.ReportingTime,
                    RevisedDelayedReportingTime = fdp.RevisedDelayedReportingTime,
                    Split = fdp.Split,
                    TemplateId = fdp.TemplateId,
                    UPD = fdp.UPD,
                    UserName = fdp.UserName,
                };
                return new CustomActionResult(HttpStatusCode.OK, fdp_result);


            }
            catch (Exception ex)
            {
                int iiii = 0;
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += " IINER:" + ex.InnerException.Message;
                return new CustomActionResult(HttpStatusCode.OK, msg);
            }

            return Ok(true);
        }




        DateTime toDate(string dt, string t)
        {
            var year = Convert.ToInt32(dt.Substring(0, 4));
            var month = Convert.ToInt32(dt.Substring(4, 2));
            var day = Convert.ToInt32(dt.Substring(6, 2));
            var hour = Convert.ToInt32(t.Substring(0, 2));
            var minute = Convert.ToInt32(t.Substring(2, 2));

            return new DateTime(year, month, day, hour, minute, 0);
        }
        private void AddToCumFlight(FDP fdp, List<RosterFDPDtoItem> items)
        {
            try
            {

                var context = new Models.dbEntities();
                context.Database.ExecuteSqlCommand("Delete from TableFlightFDP where FDPId=" + fdp.Id);
                var utcdiff = Convert.ToInt32(ConfigurationManager.AppSettings["utcdiff"]);
                var fdpItems = fdp.FDPItems.ToList();
                foreach (var fdp_item in fdpItems)
                {
                    var item = items.FirstOrDefault(q => q.flightId == fdp_item.FlightId);
                    var offblock = ((DateTime)item.offblock).AddMinutes(utcdiff);
                    var onblock = ((DateTime)item.onblock).AddMinutes(utcdiff);
                    if (offblock.Date == onblock.Date)
                    {
                        var duration = (fdp_item.IsOff == true || fdp_item.IsPositioning == true || fdp_item.IsSector == false) ? 0 : (onblock - offblock).TotalMinutes;
                        context.TableFlightFDPs.Add(new TableFlightFDP()
                        {
                            CDate = offblock.Date,
                            CrewId = fdp.CrewId,
                            Duration = duration,
                            DurationLocal = duration,
                            DutyEnd = item.onblock,
                            DutyEndLocal = onblock,
                            DutyStart = item.offblock,
                            DutyStartLocal = offblock,
                            FDPItemId = fdp_item.Id,
                            FlightId = fdp_item.FlightId,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });
                    }
                    else
                    {
                        var duration1 = (fdp_item.IsOff == true || fdp_item.IsPositioning == true || fdp_item.IsSector == false)
                            ? 0 : (onblock.Date - offblock).TotalMinutes;
                        var duration2 = (fdp_item.IsOff == true || fdp_item.IsPositioning == true || fdp_item.IsSector == false)
                            ? 0 : (onblock - onblock.Date).TotalMinutes;
                        context.TableFlightFDPs.Add(new TableFlightFDP()
                        {
                            CDate = offblock.Date,
                            CrewId = fdp.CrewId,
                            Duration = duration1,
                            DurationLocal = duration1,
                            DutyEnd = item.onblock,
                            DutyEndLocal = onblock,
                            DutyStart = item.offblock,
                            DutyStartLocal = offblock,
                            FDPItemId = fdp_item.Id,
                            FlightId = fdp_item.FlightId,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });
                        context.TableFlightFDPs.Add(new TableFlightFDP()
                        {
                            CDate = onblock.Date,
                            CrewId = fdp.CrewId,
                            Duration = duration2,
                            DurationLocal = duration2,
                            DutyEnd = item.onblock,
                            DutyEndLocal = onblock,
                            DutyStart = item.offblock,
                            DutyStartLocal = offblock,
                            FDPItemId = fdp_item.Id,
                            FlightId = fdp_item.FlightId,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });


                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }


        }

        private void AddToCumFlight(FDP fdp, List<SchFlight> flights, List<FDPItem> fdpitems, Models.dbEntities context = null)
        {
            try
            {
                //02-05
                bool do_save = context == null;
                if (context == null)
                    context = new Models.dbEntities();
                context.Database.ExecuteSqlCommand("Delete from TableFlightFDP where FDPId=" + fdp.Id);
                var utcdiff = Convert.ToInt32(ConfigurationManager.AppSettings["utcdiff"]);
                // var fdpItems = fdp.FDPItems.ToList();
                foreach (var flight in flights)
                {
                    var fdp_item = fdpitems.FirstOrDefault(q => q.FlightId == flight.ID);
                    var offblock = ((DateTime)flight.ChocksOut).AddMinutes(utcdiff);
                    var onblock = ((DateTime)flight.ChocksIn).AddMinutes(utcdiff);
                    if (offblock.Date == onblock.Date)
                    {
                        var duration = (onblock - offblock).TotalMinutes;
                        context.TableFlightFDPs.Add(new TableFlightFDP()
                        {
                            CDate = offblock.Date,
                            CrewId = fdp.CrewId,
                            Duration = duration,
                            DurationLocal = duration,
                            DutyEnd = flight.ChocksIn,
                            DutyEndLocal = onblock,
                            DutyStart = flight.ChocksOut,
                            DutyStartLocal = offblock,
                            FDPItemId = fdp_item.Id,
                            FlightId = flight.ID,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });
                    }
                    else
                    {
                        var duration1 = (onblock.Date - offblock).TotalMinutes;
                        var duration2 = (onblock - onblock.Date).TotalMinutes;
                        context.TableFlightFDPs.Add(new TableFlightFDP()
                        {
                            CDate = offblock.Date,
                            CrewId = fdp.CrewId,
                            Duration = duration1,
                            DurationLocal = duration1,
                            DutyEnd = flight.ChocksIn,
                            DutyEndLocal = onblock,
                            DutyStart = flight.ChocksOut,
                            DutyStartLocal = offblock,
                            FDPItemId = fdp_item.Id,
                            FlightId = flight.ID,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });
                        context.TableFlightFDPs.Add(new TableFlightFDP()
                        {
                            CDate = onblock.Date,
                            CrewId = fdp.CrewId,
                            Duration = duration2,
                            DurationLocal = duration2,
                            DutyEnd = flight.ChocksIn,
                            DutyEndLocal = onblock,
                            DutyStart = flight.ChocksOut,
                            DutyStartLocal = offblock,
                            FDPItemId = fdp_item.Id,
                            FlightId = flight.ID,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });


                    }
                }
                if (do_save)
                    context.SaveChanges();
            }
            catch (Exception ex)
            {

            }


        }



        //this method is planningController too
        void AddToCumDuty(FDP fdp, Models.dbEntities context = null)
        {
            try
            {
                bool do_save = context == null;
                if (context == null)
                    context = new Models.dbEntities();
                context.Database.ExecuteSqlCommand("Delete from TableDutyFDP where FDPId=" + fdp.Id);
                var utcdiff = Convert.ToInt32(ConfigurationManager.AppSettings["utcdiff"]);

                if (fdp.DutyType == 1165)
                {
                    var startLocal = ((DateTime)fdp.DateStart).AddMinutes(utcdiff);
                    var endLocal = ((DateTime)fdp.DateEnd).AddMinutes(utcdiff);
                    var startDate = startLocal.Date;
                    var endDate = endLocal.Date;
                    if (startDate == endDate)
                    {
                        var duration = (endLocal - startLocal).TotalMinutes;
                        context.TableDutyFDPs.Add(new TableDutyFDP()
                        {
                            CDate = startDate,
                            CrewId = fdp.CrewId,
                            Duration = duration,
                            DurationLocal = duration,
                            DutyEnd = fdp.DateEnd,
                            DutyEndLocal = endLocal,
                            DutyStart = fdp.DateStart,
                            DutyStartLocal = fdp.DateStart,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });

                    }
                    else
                    {
                        var duration1 = (endDate - startLocal).TotalMinutes;
                        var duration2 = (endLocal - endDate).TotalMinutes;
                        context.TableDutyFDPs.Add(new TableDutyFDP()
                        {
                            CDate = startDate,
                            CrewId = fdp.CrewId,
                            Duration = duration1,
                            DurationLocal = duration1,
                            DutyEnd = fdp.DateEnd,
                            DutyEndLocal = endLocal,
                            DutyStart = fdp.DateStart,
                            DutyStartLocal = fdp.DateStart,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });
                        context.TableDutyFDPs.Add(new TableDutyFDP()
                        {
                            CDate = endDate,
                            CrewId = fdp.CrewId,
                            Duration = duration2,
                            DurationLocal = duration2,
                            DutyEnd = fdp.DateEnd,
                            DutyEndLocal = endLocal,
                            DutyStart = fdp.DateStart,
                            DutyStartLocal = fdp.DateStart,
                            FDPId = fdp.Id,
                            GUID = Guid.NewGuid()
                        });
                    }




                }
                else
                {
                    double coef = 0;
                    switch (fdp.DutyType)
                    {
                        case 5000:
                        case 5001:
                            coef = 1;
                            break;
                        case 100025:
                        case 100003:
                        case 1167:
                        case 1168:
                            coef = 0.25;
                            break;





                        default:
                            coef = 0;
                            break;
                    }
                    var startLocal = ((DateTime)fdp.DateStart).AddMinutes(utcdiff);
                    var endLocal = ((DateTime)fdp.DateEnd).AddMinutes(utcdiff);
                    var startDate = startLocal.Date;
                    var endDate = endLocal.Date;
                    var duration = (endLocal - startLocal).TotalMinutes * coef;
                    context.TableDutyFDPs.Add(new TableDutyFDP()
                    {
                        CDate = startDate,
                        CrewId = fdp.CrewId,
                        Duration = duration,
                        DurationLocal = duration,
                        DutyEnd = fdp.DateEnd,
                        DutyEndLocal = endLocal,
                        DutyStart = fdp.DateStart,
                        DutyStartLocal = fdp.DateStart,
                        FDPId = fdp.Id,
                        GUID = Guid.NewGuid()
                    });



                }
                if (do_save)
                    context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }


        static List<FDPMaxDaily> MaxTable = null;
        List<FDPMaxDaily> getMaxFDPTable()
        {
            if (MaxTable == null)
            {
                var context = new Models.dbEntities();
                MaxTable = context.FDPMaxDailies.ToList();
            }
            return MaxTable;
        }

        static List<Extension> ExtensionTable = null;
        List<Extension> getExtensionTable()
        {
            if (ExtensionTable == null)
            {
                var context = new Models.dbEntities();
                ExtensionTable = context.Extensions.ToList();
            }
            return ExtensionTable;
        }



        public int GetMaxFDP2(DateTime reporting, int sectors)
        {

            var MaxFDPTable = getMaxFDPTable();
            var maxfdp = getMaxFDP(reporting, sectors, MaxFDPTable);
            return maxfdp;
        }
        //magu2
        public async Task<dynamic> GetMaxFDPStats(List<int> ids, int? dh = null)
        {
            var context = new Models.dbEntities();
            var MaxFDPTable = getMaxFDPTable(); //await this.context.FDPMaxDailies.ToListAsync();
            //var flights = await this.context.ViewFlightABS.Where(q => ids.Contains(q.ID)).OrderBy(q => q.STDDay).ThenBy(q => q.STD).ThenBy(q => q.Register).ToListAsync();
            //2022-01-23
            var flights = await context.ViewLegTimes.Where(q => ids.Contains(q.ID)).OrderBy(q => q.STDDay).ThenBy(q => q.ChocksOut).ThenBy(q => q.Register).ToListAsync();
            foreach (var f in flights)
            {

                if (f.ChocksIn == null)
                    f.ChocksIn = f.STA;
                if (f.ChocksOut == null)
                    f.ChocksOut = f.STD;
            }
            //old 
            //var flights = await this.context.ViewLegTimes.Where(q => ids.Contains(q.ID)).OrderBy(q => q.STDDay).ThenBy(q => q.STD).ThenBy(q => q.Register).ToListAsync();
            MaxFDPStats stat = new MaxFDPStats();
            stat.ReportingTime = ((DateTime)flights.First().DepartureLocal).AddMinutes(-60);
            stat.Sectors = ids.Count - (dh == null ? 0 : (int)dh);
            stat.RestFrom = ((DateTime)flights.Last().ArrivalLocal).AddMinutes(30);
            var endDate = ((DateTime)flights.Last().ArrivalLocal);

            var _start = stat.ReportingTime.Hour + stat.ReportingTime.Minute * 1.0 / 60;
            var _end = endDate.Hour + endDate.Minute * 1.0 / 60;
            if (stat.ReportingTime.Day != endDate.Day)
                _start = 0;
            double wocl = 0;
            if (_start >= 2 && _start <= 6)
                wocl = Math.Min(6, _end) - _start;
            else if (_end >= 2 && _end <= 6)
                wocl = Math.Min(6, _end) - 2;
            else if (2 > _start && 2 < _end && 6 > _start && 6 < _end)
                wocl = 4;


            stat.RestTo = stat.RestFrom.AddHours(12);
            stat.MaxFDP = getMaxFDP(((DateTime)flights.First().DepartureLocal).AddMinutes(-60), stat.Sectors, MaxFDPTable);
            stat.WOCL = wocl * 60;
            stat.Extended = 0;
            stat.AllowedExtension = 0;
            bool allowExtension = true;
            stat.WOCLError = 0;
            if (flights.Count >= 5)
                allowExtension = false;
            else if (wocl > 0 && wocl <= 2 && flights.Count > 4)
            {
                allowExtension = false;
                stat.WOCLError = 1;
            }
            else if (wocl > 2 && flights.Count > 2)
            {
                allowExtension = false;
                stat.WOCLError = 1;
            }

            if (wocl > 0 && wocl <= 2 && flights.Count > 4)
            {

                stat.WOCLError = 1;
            }
            else if (wocl > 2 && flights.Count > 2)
            {

                stat.WOCLError = 1;
            }
            //allowExtension = true;
            if (allowExtension)
                for (int i = 1; i < flights.Count; i++)
                {
                    var dt = (DateTime)flights[i].ChocksOut - (DateTime)flights[i - 1].ChocksIn;
                    var minuts = dt.TotalMinutes;

                    var brk = minuts - 30 - 60;

                    if (brk >= 180 && brk < 600)
                    {
                        stat.Extended = 0.5 * brk;
                        break;
                    }
                }

            stat.Flight = flights.Select(q => ((DateTime)q.ChocksIn - (DateTime)q.ChocksOut).TotalMinutes).Sum();
            stat.Duration = ((DateTime)flights.Last().ArrivalLocal - stat.ReportingTime).TotalMinutes;
            if (stat.Duration > stat.MaxFDP && stat.Extended == 0)
            {
                var extTable = getExtensionTable();
                var extend = getExtension(((DateTime)flights.First().DepartureLocal).AddMinutes(-60), stat.Sectors, extTable);
                if (extend >= stat.Duration)
                    stat.AllowedExtension = getExtension(((DateTime)flights.First().DepartureLocal).AddMinutes(-60), stat.Sectors, extTable);


            }
            stat.MaxFDPExtended = stat.MaxFDP + stat.Extended;

            stat.Duty = stat.Duration + 30;


            return stat;
        }
        public int getMaxFDP(DateTime start, int sectors, List<FDPMaxDaily> rows)
        {
            var _start = new DateTime(1900, 1, 1, start.Hour, start.Minute, start.Second);
            var row = rows.FirstOrDefault(q => q.Sectors == sectors && _start >= q.DutyStart && _start <= q.DutyEnd);
            if (row != null)
                return (int)row.MaxFDP;
            else
                return 13 * 60;
        }
        public int getExtension(DateTime start, int sectors, List<Extension> rows)
        {
            var _start = new DateTime(1900, 1, 1, start.Hour, start.Minute, start.Second);
            var row = rows.FirstOrDefault(q => q.Sectors == sectors && _start >= q.DutyStart && _start <= q.DutyEnd);
            if (row != null)
                return (int)row.MaxFDP;
            else
                return 0;
        }


    }

    public class MaxFDPStats
    {
        public int Sectors { get; set; }
        public DateTime ReportingTime { get; set; }

        public DateTime RestTo { get; set; }

        public double MaxFDP { get; set; }
        public double WOCL { get; set; }
        public double WOCLError { get; set; }

        public double Extended { get; set; }

        public double MaxFDPExtended { get; set; }

        public double Flight { get; set; }

        public DateTime RestFrom { get; set; }

        public double Duration { get; set; }
        public double Duty { get; set; }

        public bool IsOver
        {
            get
            {
                return Duration > MaxFDPExtended;
            }
        }

        public int AllowedExtension { get; set; }
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
    public class FDPResult
    {
        public int Id { get; set; }
        public Nullable<int> CrewId { get; set; }
        public Nullable<System.DateTime> ReportingTime { get; set; }
        public Nullable<System.DateTime> DelayedReportingTime { get; set; }
        public Nullable<System.DateTime> RevisedDelayedReportingTime { get; set; }
        public Nullable<System.DateTime> FirstNotification { get; set; }
        public Nullable<System.DateTime> NextNotification { get; set; }
        public Nullable<int> DelayAmount { get; set; }
        public Nullable<int> BoxId { get; set; }
        public Nullable<int> JobGroupId { get; set; }
        public bool IsTemplate { get; set; }
        public int DutyType { get; set; }
        public Nullable<System.DateTime> DateContact { get; set; }
        public Nullable<int> FDPId { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> TemplateId { get; set; }
        public Nullable<System.DateTime> FDPReportingTime { get; set; }
        public Nullable<System.Guid> GUID { get; set; }
        public Nullable<int> FirstFlightId { get; set; }
        public Nullable<int> LastFlightId { get; set; }
        public Nullable<int> UPD { get; set; }
        public Nullable<bool> IsMain { get; set; }
        public string Key { get; set; }
        public Nullable<bool> CP { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Remark { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<System.DateTime> InitStart { get; set; }
        public Nullable<System.DateTime> InitEnd { get; set; }
        public Nullable<System.DateTime> InitRestTo { get; set; }
        public string InitFlts { get; set; }
        public string InitRoute { get; set; }
        public string InitFromIATA { get; set; }
        public string InitToIATA { get; set; }
        public string InitNo { get; set; }
        public string InitKey { get; set; }
        public Nullable<int> InitHomeBase { get; set; }
        public string InitRank { get; set; }
        public Nullable<int> InitIndex { get; set; }
        public string InitGroup { get; set; }
        public string InitScheduleName { get; set; }
        public string InitFlights { get; set; }
        public string Remark2 { get; set; }
        public string CanceledNo { get; set; }
        public string CanceledRoute { get; set; }
        public Nullable<int> Extension { get; set; }
        public Nullable<double> Split { get; set; }
        public Nullable<System.DateTime> DateConfirmed { get; set; }
        public string ConfirmedBy { get; set; }
        public string UserName { get; set; }
        public Nullable<decimal> MaxFDP { get; set; }
        public Nullable<int> BL { get; set; }
        public Nullable<int> FX { get; set; }


    }
}
