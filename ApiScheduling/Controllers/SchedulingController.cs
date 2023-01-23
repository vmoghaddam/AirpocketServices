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


        [Route("api/roster/fdp/save")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> SaveFDP(RosterFDPDto dto)
        {
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
                    return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                    {
                        message = "Flight Time/Duty Limitaion Error. "
                        + "7-Day Duty: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d7.Duty7 + fdpDuty))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d7.Duty7 + fdpDuty))) % 60)
                        + " on " + _d7.CDate.ToString("yyy-MMM-dd")
                    });
                }
                if (_d14 != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                    {
                        message = "Flight Time/Duty Limitaion Error. "
                        + "14-Day Duty: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d14.Duty14 + fdpDuty))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d14.Duty14 + fdpDuty))) % 60)
                        + " on " + _d14.CDate.ToString("yyy-MMM-dd")
                    });
                }
                if (_d28 != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                    {
                        message = "Flight Time/Duty Limitaion Error. "
                        + "28-Day Duty: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d28.Duty28 + fdpDuty))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_d28.Duty28 + fdpDuty))) % 60)
                        + " on " + _d28.CDate.ToString("yyy-MMM-dd")
                    });
                }

                if (_f28 != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                    {
                        message = "Flight Time/Duty Limitaion Error. "
                        + "28-Day Flight: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_f28.Flight28 + fdpFlight))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_f28.Flight28 + fdpFlight))) % 60)
                        + " on " + _f28.CDate.ToString("yyy-MMM-dd")
                    });
                }

                if (_fy != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                    {
                        message = "Flight Time/Duty Limitaion Error. "
                        + "12-Month Flight: "
                        + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_fy.FlightYear + fdpFlight))) / 60)
                        + ":" + FormatTwoDigits(Convert.ToInt32(Math.Floor(Convert.ToDecimal(_fy.FlightYear + fdpFlight))) % 60)
                        + " on " + _fy.CDate.ToString("yyy-MMM-dd")
                    });
                }

                if (_fcy != null && (dto.IsAdmin == null || dto.IsAdmin == 0))
                {
                    return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                    {
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



                };
                fdp.FDPExtras.Add(new FDPExtra() { MaxFDP = dto.maxFDP });
                //Check Extension //////////////////
                if (fdp.Extension != null && fdp.Extension > 0)
                {
                    var exd1 = (DateTime)fdp.InitStart;
                    var exd0 = exd1.AddHours(-168);
                    var extcnt = await context.ExtensionHistories.Where(q => q.CrewId == fdp.CrewId && (q.DutyStart >= exd0 && q.DutyStart <= exd1)).CountAsync();
                    if (extcnt >= 2)
                    {
                        return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                        {

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
                                                      (fdp.InitStart >= q.InitStart && fdp.InitStart <= q.InitRestTo)
                                                       || (fdp.InitEnd >= q.InitStart && fdp.InitEnd <= q.InitRestTo)
                                                       || (q.InitStart >= fdp.InitStart && q.InitStart <= fdp.InitRestTo)
                                                       || (q.InitRestTo >= fdp.InitStart && q.InitRestTo <= fdp.InitRestTo)
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
                        if ((dto.IsAdmin == null || dto.IsAdmin == 0) && (_activeq && _interupted.DutyType != 1167 && _interupted.DutyType != 1168 && _interupted.DutyType != 1170) || !(dto.items.First().std >= _interupted.DateStart && dto.items.First().std <= _interupted.DateEnd))
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
                                return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                                {

                                    message = "Rest/Interruption Error(" + _interupted.Id + "). " + (GetDutyTypeTitle(_interupted.DutyType)) + "   " + (_interupted.InitStart == null ? "" : ((DateTime)_interupted.InitStart).ToString("yyyy-MM-dd") + " " + _interupted.InitFlts + " " + _interupted.InitRoute)

                                });
                        }
                        else
                        {
                            if (_interupted.DutyType == 1167 || _interupted.DutyType == 1168)
                                return new CustomActionResult(HttpStatusCode.NotImplemented, _interupted);
                            else if (_interupted.DutyType == 5000)
                                return new CustomActionResult(HttpStatusCode.NotAcceptable, new
                                {

                                    message = "Interruption Error (TRAINING)"

                                });
                            else
                            {
                                var _strt = ((DateTime)fdp.InitStart).AddMinutes(60);
                                var rdif = Math.Abs((DateTime.UtcNow - _strt).TotalMinutes);
                                if (rdif < 10 * 60)
                                    return new CustomActionResult(HttpStatusCode.NotModified, _interupted);
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



                timer.Stop();
                var _ms = timer.Elapsed;
                return new CustomActionResult(HttpStatusCode.OK, fdp);


            }
            catch (Exception ex)
            {
                int iiii = 0;
                return new CustomActionResult(HttpStatusCode.OK, null);
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
