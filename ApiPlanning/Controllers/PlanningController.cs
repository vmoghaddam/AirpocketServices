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
using ApiPlanning.ViewModels;

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

        public DateTime parseDate(string str)
        {
            var prts = str.Split('-').Select(q => Convert.ToInt32(q)).ToList();
            return new DateTime(prts[0], prts[1], prts[2], prts[3], prts[4], 0);

        }
        public List<DateTime> GetInvervalDates(int type, DateTime start, DateTime end, List<int> days = null)
        {
            List<DateTime> result = new List<DateTime>();
            var minDate = start.Date;
            var maxDate = end.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            while (minDate <= maxDate)
            {
                switch (type)
                {
                    case 1:
                        result.Add(minDate);
                        break;
                    case 2:
                        var d = (int)minDate.DayOfWeek;
                        if (days.IndexOf(d) != -1)
                            result.Add(minDate);
                        break;
                    default:
                        break;
                }
                minDate = minDate.AddDays(1);
            }
            return result;
        }

        [Route("api/plan/group/save/utc")]
        //kakoli9
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightGroupUTC(ViewModels.FlightDto dto)
        {
            var _context = new Models.dbEntities();
            bool isUtc = true;

             

            //var validate = unitOfWork.FlightRepository.ValidateFlight(dto);
            //if (validate.Code != HttpStatusCode.OK)
            //    return validate;
            var nowOffset = isUtc ? 0 : TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;
            dto.STD = parseDate(dto.STDRAW);
            dto.STA = parseDate(dto.STARAW);
            dto.RefDate = parseDate(dto.RefDateRAW);
            dto.IntervalFrom = parseDate(dto.IntervalFromRAW);
            dto.IntervalTo = parseDate(dto.IntervalToRAW);
            dto.Days = isUtc ? dto.DaysUTC.ToList() : dto.Days;




            var stdOffset = isUtc ? 0 : TimeZoneInfo.Local.GetUtcOffset((DateTime)dto.STD).TotalMinutes;
            var localSTD = ((DateTime)dto.STD).AddMinutes(stdOffset);
            var _addDay = localSTD.Day == ((DateTime)dto.STD).Day ? 0 : 1;

            var stdHours = ((DateTime)dto.STD).Hour;
            var stdMinutes = ((DateTime)dto.STD).Minute;
            var staHours = ((DateTime)dto.STA).Hour;
            var staMinutes = ((DateTime)dto.STA).Minute;
            var duration = (((DateTime)dto.STA) - ((DateTime)dto.STD)).TotalMinutes;
           

            var intervalDays = GetInvervalDates((int)dto.Interval, (DateTime)dto.IntervalFrom, (DateTime)dto.IntervalTo, dto.Days);


            FlightInformation entity = null;
           // FlightChangeHistory changeLog = null;

            List<FlightInformation> flights = new List<FlightInformation>();
            var str = DateTime.Now.ToString("MMddmmss");
            var flightGroup = Convert.ToInt32(str);
            foreach (var dt in intervalDays)
            {
                entity = new FlightInformation();
                _context.FlightInformations.Add(entity);
                flights.Add(entity);
                if (entity.STD != null)
                {
                    var oldSTD = ((DateTime)entity.STD).AddMinutes(270).Date;
                    var newSTD = ((DateTime)dto.STD).AddMinutes(270).Date;
                    if (oldSTD != newSTD)
                    {
                        entity.FlightDate = oldSTD;
                    }
                }


                ViewModels.FlightDto.Fill(entity, dto);
                var _fltDate = new DateTime(dt.Year, dt.Month, dt.Day, 1, 0, 0);
                var fltOffset = isUtc ? 0 : -1 * TimeZoneInfo.Local.GetUtcOffset(_fltDate).TotalMinutes;
                entity.FlightGroupID = flightGroup;
                var _std = new DateTime(dt.Year, dt.Month, dt.Day, (int)dto.STDHH, (int)dto.STDMM, 0);
                _std = _std.AddDays(_addDay).AddMinutes(fltOffset);
                entity.STD = _std;

                entity.STA = ((DateTime)entity.STD).AddMinutes(duration);
                entity.ChocksOut = entity.STD;
                entity.ChocksIn = entity.STA;
                entity.Takeoff = entity.STD;
                entity.Landing = entity.STA;
                entity.BoxId = dto.BoxId;
            }


            var saveResult = await _context.SaveChangesAsync();
           // if (saveResult.Code != HttpStatusCode.OK)
           //     return saveResult;

            //if (dto.SMSNira == 1)
           // {
           //     foreach (var x in flights)
           //         await unitOfWork.FlightRepository.NotifyNira(x.ID, dto.UserName);
           // }

            //bip
            var flightIds = flights.Select(q => q.ID).ToList();
            var beginDate = ((DateTime)dto.RefDate).Date;
            var endDate = ((DateTime)dto.RefDate).Date.AddDays((int)dto.RefDays).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var fg=await _context.ViewFlightsGantts.Where(q => flightIds.Contains(q.ID)
            && q.STDDay >= beginDate && q.STDDay <= endDate
            ).ToListAsync();
            


            var odtos = new List<ViewFlightsGanttDto>();
            foreach (var f in fg)
            {
                ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();

                ViewModels.ViewFlightsGanttDto.FillDto(f, odto, 0, 1);
                odto.resourceId.Add((int)odto.RegisterID);
                odtos.Add(odto);
            }

            var resgroups = from x in fg
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                           into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var ressq = (from x in fg
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp

                         orderby getOrderIndex(grp.Key.Register, new List<string>())
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();




            var oresult = new
            {
                flights = odtos,
                resgroups,
                ressq
            };

            return Ok(/*entity*/oresult);

        }

        [Route("api/plan/group/update/utc")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightGroupUpdate(ViewModels.FlightDto dto)
        {
            try
            {
                var _context = new Models.dbEntities();

                //var validate = unitOfWork.FlightRepository.ValidateFlight(dto);
                //if (validate.Code != HttpStatusCode.OK)
                //    return validate;
                bool isUtc = true;
                var nowOffset = isUtc ? 0 : TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;
                dto.STD = parseDate(dto.STDRAW);
                dto.STA = parseDate(dto.STARAW);
                dto.RefDate = parseDate(dto.RefDateRAW);
                dto.IntervalFrom = parseDate(dto.IntervalFromRAW);
                dto.IntervalTo = parseDate(dto.IntervalToRAW);
                dto.Days = isUtc ? dto.DaysUTC.ToList() : dto.Days;

                var intervalDays =  GetInvervalDates((int)dto.Interval, (DateTime)dto.IntervalFrom, (DateTime)dto.IntervalTo, dto.Days).Select(q => (Nullable<DateTime>)q).ToList();


                var baseFlight = await _context.FlightInformations.Where(q => q.ID == dto.ID).FirstOrDefaultAsync();
                if (baseFlight == null)
                    return BadRequest();

                var stdHoursBF = ((DateTime)baseFlight.STD).Hour;
                var stdMinutesBF = ((DateTime)baseFlight.STD).Minute;
                var staHoursBF = ((DateTime)baseFlight.STA).Hour;
                var staMinutesBF = ((DateTime)baseFlight.STA).Minute;

                var flightIds = await (from x in _context.ViewLegTimes
                                       where x.FlightNumber == baseFlight.FlightNumber && x.FlightPlanId==baseFlight.FlightGroupID && intervalDays.Contains(x.STDDay)
                                       select x.ID).ToListAsync();
                //var flights = await unitOfWork.FlightRepository.GetFlights().Where(q => flightIds.Contains(q.ID)).ToListAsync();
                List<FlightInformation> flights = new List<FlightInformation>();
                if ((int)dto.CheckTime == 0)
                    flights = await _context.FlightInformations.Where(q => flightIds.Contains(q.ID)).OrderBy(q => q.STD).ToListAsync();
                else
                    flights = await _context.FlightInformations.Where(q => flightIds.Contains(q.ID)

                    ).OrderBy(q => q.STD).ToListAsync();
                
                var stdOffset = isUtc ? 0 : TimeZoneInfo.Local.GetUtcOffset((DateTime)dto.STD).TotalMinutes;
                var localSTD = ((DateTime)dto.STD).AddMinutes(stdOffset);
                var _addDay = localSTD.Day == ((DateTime)dto.STD).Day ? 0 : 1;

                var stdHours = ((DateTime)dto.STD).Hour;
                var stdMinutes = ((DateTime)dto.STD).Minute;
                var staHours = ((DateTime)dto.STA).Hour;
                var staMinutes = ((DateTime)dto.STA).Minute;
                var duration = (((DateTime)dto.STA) - ((DateTime)dto.STD)).TotalMinutes;

                int utcDiff = 0;
                if (flights.Count > 0)
                {
                    var firstFlight = flights.First();
                    var __d1 = ((DateTime)firstFlight.STD).Date;
                    var __d2 = ((DateTime)dto.STD).Date;
                    if (__d1 > __d2)
                        utcDiff = -1;
                    if (((DateTime)firstFlight.STD).Date < ((DateTime)dto.STD).Date)
                        utcDiff = 1;

                }

                foreach (var entity in flights)
                {

                    var flt_stdHours = ((DateTime)entity.STD).Hour;
                    var flt_stdMinutes = ((DateTime)entity.STD).Minute;
                    var flt_staHours = ((DateTime)entity.STA).Hour;
                    var flt_staMinutes = ((DateTime)entity.STA).Minute;
                    bool exec = true;
                    if (((int)dto.CheckTime) == 1)
                    {
                        exec = flt_stdHours == stdHoursBF && flt_stdMinutes == stdMinutesBF && flt_staHours == staHoursBF && flt_staMinutes == staMinutesBF;
                    }
                    if (entity.FlightStatusID == 1 && exec)
                    {

                        var changeLog = new FlightChangeHistory()
                        {
                            Date = DateTime.Now,
                            FlightId = entity.ID,

                        };
                        
                        _context.FlightChangeHistories.Add(changeLog);
                        changeLog.OldFlightNumer = entity.FlightNumber;
                        changeLog.OldFromAirportId = entity.FromAirportId;
                        changeLog.OldToAirportId = entity.ToAirportId;
                        changeLog.OldSTD = entity.STD;
                        changeLog.OldSTA = entity.STA;
                        changeLog.OldStatusId = entity.FlightStatusID;
                        changeLog.OldRegister = entity.RegisterID;
                        changeLog.OldOffBlock = entity.ChocksOut;
                        changeLog.OldOnBlock = entity.ChocksIn;
                        changeLog.OldTakeOff = entity.Takeoff;
                        changeLog.OldLanding = entity.Landing;
                        changeLog.User = dto.UserName;

                        var oldSTD = (DateTime)entity.STD;



                        var newSTD = new DateTime(oldSTD.Year, oldSTD.Month, oldSTD.Day, stdHours, stdMinutes, 0);
                        var newSTA = newSTD.AddMinutes(duration);
                        if (oldSTD.AddMinutes(270).Date != newSTD.AddMinutes(270).Date)
                            entity.FlightDate = oldSTD.AddDays(utcDiff);
                        ViewModels.FlightDto.FillForGroupUpdate(entity, dto);
                        

                        var _fltDate = new DateTime(oldSTD.Year, oldSTD.Month, oldSTD.Day, 1, 0, 0);
                        
                       
                        var fltOffset = isUtc ? 0 : -1 * TimeZoneInfo.Local.GetUtcOffset(_fltDate).TotalMinutes;
                        var _std = new DateTime(oldSTD.Year, oldSTD.Month, oldSTD.Day, (int)dto.STDHH, (int)dto.STDMM, 0);

                        // _std = _std.AddMinutes(fltOffset);
                        _std = _std.AddDays(_addDay).AddDays(utcDiff).AddMinutes(fltOffset);


                        entity.STD = _std; //newSTD;
                        entity.STA = _std.AddMinutes(duration); //newSTA;
                        entity.ChocksOut = entity.STD;
                        entity.ChocksIn = entity.STA;
                        entity.Takeoff = entity.STD;
                        entity.Landing = entity.STA;
                        entity.BoxId = dto.BoxId;

                        changeLog.NewFlightNumber = entity.FlightNumber;
                        changeLog.NewFromAirportId = entity.FromAirportId;
                        changeLog.NewToAirportId = entity.ToAirportId;
                        changeLog.NewSTD = entity.STD;
                        changeLog.NewSTA = entity.STA;
                        changeLog.NewStatusId = entity.FlightStatusID;
                        changeLog.NewRegister = entity.RegisterID;
                        changeLog.NewOffBlock = entity.ChocksOut;
                        changeLog.NewOnBlock = entity.ChocksIn;
                        changeLog.NewTakeOff = entity.Takeoff;
                        changeLog.NewLanding = entity.Landing;

                        //var state = unitOfWork.context.Entry(entity).State; //= EntityState.Modified;

                    }
                    else if (exec)
                    {
                        entity.ChrCode = dto.ChrCode;
                        entity.ChrTitle = dto.ChrTitle;
                    }


                }



                var saveResult = await _context.SaveChangesAsync();
                

                //if (dto.SMSNira == 1)
                //{
                //    foreach (var x in flights)
                //        await unitOfWork.FlightRepository.NotifyNira(x.ID, dto.UserName);
                //}

                //bip
                // var flightIds = flights.Select(q => q.ID).ToList();
                var beginDate = ((DateTime)dto.RefDate).Date;
                var endDate = ((DateTime)dto.RefDate).Date.AddDays((int)dto.RefDays).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                var fg = await _context.ViewFlightsGantts .Where(q => flightIds.Contains(q.ID)
                   && q.STDDay >= beginDate && q.STDDay <= endDate
                ).ToListAsync();


                var odtos = new List<ViewFlightsGanttDto>();
                foreach (var f in fg)
                {
                    ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();

                    ViewModels.ViewFlightsGanttDto.FillDto(f, odto, 0, 1);
                    odto.resourceId.Add((int)odto.RegisterID);
                    odtos.Add(odto);
                }

                var resgroups = from x in fg
                                group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                               into grp
                                select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
                var ressq = (from x in fg
                             group x by new { x.RegisterID, x.Register, x.TypeId }
                         into grp

                             orderby getOrderIndex(grp.Key.Register, new List<string>())
                             select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();


                var oflight = odtos.FirstOrDefault(q => q.ID == dto.ID);

                var oresult = new
                {
                    flights = odtos,
                    flight = oflight,
                    resgroups,
                    ressq
                };

                return Ok(/*entity*/oresult);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "  " + ex.InnerException.Message;
                return Ok(msg);
            }
        }
 
        [Route("api/plan/delete/group")]

        [AcceptVerbs("POST")] 
        public async Task<IHttpActionResult> DeleteFlightGroup(dynamic dto)
        {
            var _context = new Models.dbEntities();
            int interval = Convert.ToInt32(dto.Interval);
            DateTime intervalFrom = Convert.ToDateTime(dto.IntervalFrom);
            DateTime intervalTo = Convert.ToDateTime(dto.IntervalTo);
            string _days = Convert.ToString(dto.Days);
            var days = _days.Split('_').Select(q => Convert.ToInt32(q)).ToList();
            int checkTime = (int)dto.CheckTime;

            int flightId = Convert.ToInt32(dto.Id);

            //var result = await unitOfWork.FlightRepository.DeleteFlightGroup(intervalFrom, intervalTo, days, flightId, interval, checkTime);
            /////////////////////////////////
            List<int> result = new List<int>();
            var intervalDays = GetInvervalDates(interval, intervalFrom, intervalTo, days).Select(q=>(Nullable<DateTime>)q).ToList();
            var baseFlight = await _context.FlightInformations.Where(q => q.ID == flightId).FirstOrDefaultAsync();

            var stdHoursBF = ((DateTime)baseFlight.STD).Hour;
            var stdMinutesBF = ((DateTime)baseFlight.STD).Minute;
            var staHoursBF = ((DateTime)baseFlight.STA).Hour;
            var staMinutesBF = ((DateTime)baseFlight.STA).Minute;

            var flightIds = await (from x in _context.ViewLegTimes
                                   where 
                                     x.FlightNumber == baseFlight.FlightNumber 
                                     &&  x.FlightPlanId==baseFlight.FlightGroupID
                                     && intervalDays.Contains(x.STDDay)
                                   select x.ID).ToListAsync();
            var nflts = flightIds.Select(q => (Nullable<int>)q).ToList();
            var fdpitems = await _context.FDPItems.Where(q => nflts.Contains(q.FlightId)).Select(q => q.FlightId).ToListAsync();

            var finalIds = nflts.Where(q => !fdpitems.Contains(q)).Select(q => (int)q).Distinct().ToList();
            var finalFlights = await _context.FlightInformations.Where(q => finalIds.Contains(q.ID)).ToListAsync();
            foreach (var entity in finalFlights)
            {
                if (entity.FlightStatusID == 1 || entity.FlightStatusID == 4)
                {
                    var flt_stdHours = ((DateTime)entity.STD).Hour;
                    var flt_stdMinutes = ((DateTime)entity.STD).Minute;
                    var flt_staHours = ((DateTime)entity.STA).Hour;
                    var flt_staMinutes = ((DateTime)entity.STA).Minute;
                    bool exec = true;
                    if (checkTime == 1)
                    {
                        exec = flt_stdHours == stdHoursBF && flt_stdMinutes == stdMinutesBF && flt_staHours == staHoursBF && flt_staMinutes == staMinutesBF;
                    }
                    if (exec)
                    {
                        result.Add(entity.ID);
                        _context.FlightInformations.Remove(entity);
                    }

                }
            }


            ///////////////////////////////////



            var saveResult = await _context.SaveChangesAsync();
           


            return Ok(result);
        }
        public class ShiftFlightsDto
        {
            public List<int> ids { get; set; }
            public int hour { get; set; }
            public int minute { get; set; }
            public int register { get; set; }
            public string userName { get; set; }
            public int nira { get; set; }
            public int sign { get; set; }
        }
       
        [Route("api/plan/shift")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightsShift(ShiftFlightsDto dto)
        {

            var _context = new Models.dbEntities();
            var flights = await _context.FlightInformations.Where(q => dto.ids.Contains(q.ID)).ToListAsync();
            var legs = await _context.ViewLegTimes.Where(q => dto.ids.Contains(q.ID)).ToListAsync();
            foreach (var flight in flights)
            {
                var leg = legs.FirstOrDefault(q => q.ID == flight.ID);
                if (leg != null && leg.FlightStatusID==1)
                {
                    var changeLog = new FlightChangeHistory()
                    {
                        Date = DateTime.Now,
                        FlightId = flight.ID,

                    };
                    changeLog.User = dto.userName + " " + (dto.nira == 1 ? "1" : "0");
                    changeLog.OldFlightNumer = leg.FlightNumber;
                    changeLog.OldFromAirportId = leg.FromAirport;
                    changeLog.OldToAirportId = leg.ToAirport;
                    changeLog.OldSTD = leg.STD;
                    changeLog.OldSTA = leg.STA;
                    changeLog.OldStatusId = leg.FlightStatusID;
                    changeLog.OldRegister = leg.RegisterID;
                    changeLog.OldOffBlock = leg.ChocksOut;
                    changeLog.OldOnBlock = leg.ChocksIn;
                    changeLog.OldTakeOff = leg.Takeoff;
                    changeLog.OldLanding = leg.Landing;

                    var mm = (dto.sign) * (dto.hour * 60 + dto.minute);
                    var dtoSTD = ((DateTime)flight.STD).AddMinutes(mm);
                    var dtoSTA = ((DateTime)flight.STA).AddMinutes(mm);
                    var tzoffset = Helper.GetTimeOffset((DateTime)dtoSTD);
                    var oldSTD = ((DateTime)flight.STD).AddMinutes(tzoffset).Date;
                    var newSTD = ((DateTime)dtoSTD).AddMinutes(tzoffset).Date;
                    if (oldSTD != newSTD)
                    {
                        flight.FlightDate = oldSTD;
                    }
                    flight.STD = dtoSTD;
                    flight.STA = dtoSTA;
                    flight.ChocksIn = dtoSTA;
                    flight.ChocksOut = dtoSTD;
                    flight.Takeoff = dtoSTD;
                    flight.Landing = dtoSTA;
                    changeLog.NewFlightNumber = flight.FlightNumber;
                    changeLog.NewFromAirportId = flight.FromAirportId;
                    changeLog.NewToAirportId = flight.ToAirportId;
                    changeLog.NewSTD = flight.STD;
                    changeLog.NewSTA = flight.STA;
                    changeLog.NewStatusId = flight.FlightStatusID;
                    changeLog.NewRegister = flight.RegisterID;
                    changeLog.NewOffBlock = flight.ChocksOut;
                    changeLog.NewOnBlock = flight.ChocksIn;
                    changeLog.NewTakeOff = flight.Takeoff;
                    changeLog.NewLanding = flight.Landing;

                    _context.FlightChangeHistories.Add(changeLog);


                }

            }

            var saveResult = await _context.SaveChangesAsync();
            //////////////////
            /////////////////
            

            if (dto.nira == 1)
            {
               // foreach (var id in dto.ids)
                //    await unitOfWork.FlightRepository.NotifyNira(id, obj.userName);
            }

            /////////////////////
           // var fg = await unitOfWork.FlightRepository.GetViewFlightGantts().Where(q => obj.ids.Contains(q.ID)).ToListAsync();
            var fg = await _context.ViewFlightsGantts.Where(q => dto.ids.Contains(q.ID)).ToListAsync();
            var xflights = new List<ViewFlightsGanttDto>();
            foreach (var x in fg)
            {
                ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();
                ViewModels.ViewFlightsGanttDto.FillDto(x, odto, 0, 1);
                xflights.Add(odto);
            }



            var resgroups = from x in fg
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                               into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var ressq = (from x in fg
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp

                         orderby getOrderIndex(grp.Key.Register, new List<string>())
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();

            //odto.resourceId.Add((int)odto.RegisterID);


            var oresult = new
            {
                flights=xflights,
                resgroups,
                ressq
            };
            //////////////////////
            return Ok(oresult);

        }


        public class cnlregs
        {
            public List<int> fids { get; set; }
            public int reason { get; set; }
            public string userName { get; set; }
            public int? userId { get; set; }
            public string remark { get; set; }
            public int? reg { get; set; }
            public DateTime? intervalFrom { get; set; }
            public DateTime? intervalTo { get; set; }
            public List<int> days { get; set; }
            public int? interval { get; set; }
            public DateTime? RefDate { get; set; }
            public int? RefDays { get; set; }
        }
        public class updateLogResult
        {
            public bool sendNira { get; set; }
            public int flight { get; set; }
            public List<int?> offIds { get; set; }
            public List<int> fltIds { get; set; }
            public List<offcrew> offcrews { get; set; }
        }
        public class offcrew
        {
            public int? flightId { get; set; }
            public List<int?> crews { get; set; }
        }

        [Route("api/plan/active/groupx")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightActiveX(cnlregs dto)
        {
            var _context = new Models.dbEntities();


            ///////////////////////////////////
            //var result = await unitOfWork.FlightRepository.ActiveFlights(dto);
            var flights = await _context.FlightInformations.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            var legs = await _context.ViewLegTimes.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            foreach (var fid in dto.fids)
            {
                var flight = flights.FirstOrDefault(q => q.ID == fid);
                var leg = legs.FirstOrDefault(q => q.ID == fid);

                var changeLog = new FlightChangeHistory()
                {
                    Date = DateTime.Now,
                    FlightId = flight.ID,
                    User = dto.userName,

                };
                changeLog.OldFlightNumer = leg.FlightNumber;
                changeLog.OldFromAirportId = leg.FromAirport;
                changeLog.OldToAirportId = leg.ToAirport;
                changeLog.OldSTD = flight.STD;
                changeLog.OldSTA = flight.STA;
                changeLog.OldStatusId = flight.FlightStatusID;
                changeLog.OldRegister = leg.RegisterID;
                changeLog.OldOffBlock = flight.ChocksOut;
                changeLog.OldOnBlock = flight.ChocksIn;
                changeLog.OldTakeOff = flight.Takeoff;
                changeLog.OldLanding = flight.Landing;

                //////////////////////////////////////////////////////////////

                flight.DateCreate = DateTime.Now.ToUniversalTime();
                flight.FlightStatusUserId = dto.userId;


                flight.FlightStatusID = 1;

                //var cnlMsn = await this.context.Ac_MSN.Where(q => q.Register == "CNL").Select(q => q.ID).FirstOrDefaultAsync();
                flight.RegisterID = dto.reg;
                flight.CancelDate = null;
                flight.CancelReasonId = null;
                flight.DepartureRemark += (!string.IsNullOrEmpty(flight.DepartureRemark) ? "\r\n" : "") + dto.remark + "(ACTV REMARK BY:" + dto.userName + ")";
                //2020-11-24




                if (flight.FlightStatusID != null && /*dto.UserId != null*/ !string.IsNullOrEmpty(dto.userName))
                    _context.FlightStatusLogs.Add(new FlightStatusLog()
                    {
                        FlightID = flight.ID,

                        Date = DateTime.Now.ToUniversalTime(),
                        FlightStatusID = (int)flight.FlightStatusID,

                        UserId = dto.userId != null ? (int)dto.userId : 128000,
                        Remark = dto.userName,
                    });

                //kak4

                ////////////////////////////////////////
                changeLog.NewFlightNumber = leg.FlightNumber;
                changeLog.NewFromAirportId = leg.FromAirport;
                changeLog.NewToAirportId = flight.ToAirportId;
                changeLog.NewSTD = flight.STD;
                changeLog.NewSTA = flight.STA;
                changeLog.NewStatusId = flight.FlightStatusID;
                changeLog.NewRegister = leg.RegisterID;
                changeLog.NewOffBlock = flight.ChocksOut;
                changeLog.NewOnBlock = flight.ChocksIn;
                changeLog.NewTakeOff = flight.Takeoff;
                changeLog.NewLanding = flight.Landing;

                _context.FlightChangeHistories.Add(changeLog);
            }




            bool sendNira = true;
            var nullfids = dto.fids.Select(q => (Nullable<int>)q).ToList();




            var result = new updateLogResult()
            {
                sendNira = sendNira,
                flight = -1, //flight.ID,
                             //offIds = offCrewIds
                offIds = nullfids


            };



            //////////////////////////////////////////////////////




            var saveResult = await _context.SaveChangesAsync();
          

            var fresult = result  as updateLogResult;
            //if (fresult.sendNira)
            //{
            //    foreach (var x in fresult.offIds)
            //        await unitOfWork.FlightRepository.NotifyNira((int)x, dto.userName);
            //}



            var fg = await _context.ViewFlightsGantts.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            var xflights = new List<ViewFlightsGanttDto>();
            foreach (var x in fg)
            {
                ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();
                ViewModels.ViewFlightsGanttDto.FillDto(x, odto, 0, 1);
                xflights.Add(odto);
            }



            var resgroups = from x in fg
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                           into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var ressq = (from x in fg
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp

                         orderby  getOrderIndex(grp.Key.Register, new List<string>())
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();

            //odto.resourceId.Add((int)odto.RegisterID);


            var oresult = new
            {
                flights=xflights,
                resgroups,
                ressq
            };
            //////////////////////
            return Ok(oresult);
        }


        [Route("api/plan/active/group")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightActive(cnlregs dto)
        {
            var _context = new Models.dbEntities();


            var intervalDays = GetInvervalDates((int)dto.interval, (DateTime)dto.intervalFrom, (DateTime)dto.intervalTo, dto.days).Select(q => (Nullable<DateTime>)q).ToList();
            var baseFlights =await  _context.FlightInformations.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            var fltNumbers = baseFlights.Select(q => q.FlightNumber).ToList();
            var fltIds = new List<int>();
            fltIds = baseFlights.Select(q => q.ID).ToList();
            var groupIds = baseFlights.Select(q => q.FlightGroupID).ToList();

            var _flightIds = await (from x in _context.ViewLegTimes
                                    where fltNumbers.Contains(x.FlightNumber) && groupIds.Contains(x.FlightPlanId) && intervalDays.Contains(x.STDDay)
                                    select x.ID).ToListAsync();
            fltIds = fltIds.Concat(_flightIds).Distinct().ToList();

            var flights = await _context.FlightInformations.Where(q => fltIds.Contains(q.ID)).ToListAsync();
            var legs = await _context.ViewLegTimes.Where(q => fltIds.Contains(q.ID)).ToListAsync();
            /////////////////////////////////
            //var flights = await this.context.FlightInformations.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            // var legs = await this.context.ViewLegTimes.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            foreach (var fid in fltIds)
            {
                var flight = flights.FirstOrDefault(q => q.ID == fid);
                var leg = legs.FirstOrDefault(q => q.ID == fid);

                var changeLog = new FlightChangeHistory()
                {
                    Date = DateTime.Now,
                    FlightId = flight.ID,
                    User = dto.userName,

                };
                changeLog.OldFlightNumer = leg.FlightNumber;
                changeLog.OldFromAirportId = leg.FromAirport;
                changeLog.OldToAirportId = leg.ToAirport;
                changeLog.OldSTD = flight.STD;
                changeLog.OldSTA = flight.STA;
                changeLog.OldStatusId = flight.FlightStatusID;
                changeLog.OldRegister = leg.RegisterID;
                changeLog.OldOffBlock = flight.ChocksOut;
                changeLog.OldOnBlock = flight.ChocksIn;
                changeLog.OldTakeOff = flight.Takeoff;
                changeLog.OldLanding = flight.Landing;

                //////////////////////////////////////////////////////////////

                flight.DateCreate = DateTime.Now.ToUniversalTime();
                flight.FlightStatusUserId = dto.userId;


                flight.FlightStatusID = 1;

                //var cnlMsn = await this.context.Ac_MSN.Where(q => q.Register == "CNL").Select(q => q.ID).FirstOrDefaultAsync();
                flight.RegisterID = dto.reg;
                flight.CancelDate = null;
                flight.CancelReasonId = null;
                flight.DepartureRemark += (!string.IsNullOrEmpty(flight.DepartureRemark) ? "\r\n" : "") + dto.remark + "(ACTV REMARK BY:" + dto.userName + ")";
                //2020-11-24




                if (flight.FlightStatusID != null && /*dto.UserId != null*/ !string.IsNullOrEmpty(dto.userName))
                    _context.FlightStatusLogs.Add(new FlightStatusLog()
                    {
                        FlightID = flight.ID,

                        Date = DateTime.Now.ToUniversalTime(),
                        FlightStatusID = (int)flight.FlightStatusID,

                        UserId = dto.userId != null ? (int)dto.userId : 128000,
                        Remark = dto.userName,
                    });

                //kak4

                ////////////////////////////////////////
                changeLog.NewFlightNumber = leg.FlightNumber;
                changeLog.NewFromAirportId = leg.FromAirport;
                changeLog.NewToAirportId = flight.ToAirportId;
                changeLog.NewSTD = flight.STD;
                changeLog.NewSTA = flight.STA;
                changeLog.NewStatusId = flight.FlightStatusID;
                changeLog.NewRegister = leg.RegisterID;
                changeLog.NewOffBlock = flight.ChocksOut;
                changeLog.NewOnBlock = flight.ChocksIn;
                changeLog.NewTakeOff = flight.Takeoff;
                changeLog.NewLanding = flight.Landing;

                _context.FlightChangeHistories.Add(changeLog);
            }




            bool sendNira = true;
            //var nullfids = dto.fids.Select(q => (Nullable<int>)q).ToList();

            var nullfids = fltIds.Select(q => (Nullable<int>)q).ToList();


            //hoda

            var result=   new updateLogResult()
            {
                sendNira = sendNira,
                flight = -1, //flight.ID,
                             //offIds = offCrewIds
                offIds = nullfids,
                 fltIds = fltIds

            } ;



            //////////////////////////////////////////////////////




            var saveResult = await _context.SaveChangesAsync();


            var fresult = result as updateLogResult;
            //if (fresult.sendNira)
            //{
            //    foreach (var x in fresult.offIds)
            //        await unitOfWork.FlightRepository.NotifyNira((int)x, dto.userName);
            //}



            // var fg = await _context.ViewFlightsGantts.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            var beginDate = ((DateTime)dto.RefDate).Date;
            var endDate = ((DateTime)dto.RefDate).Date.AddDays((int)dto.RefDays).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var fg = await _context.ViewFlightsGantts.Where(q => fresult.fltIds.Contains(q.ID)
             && q.STDDay >= beginDate && q.STDDay <= endDate
            ).ToListAsync();
            var xflights = new List<ViewFlightsGanttDto>();
            foreach (var x in fg)
            {
                ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();
                ViewModels.ViewFlightsGanttDto.FillDto(x, odto, 0, 1);
                xflights.Add(odto);
            }



            var resgroups = from x in fg
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                           into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var ressq = (from x in fg
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp

                         orderby getOrderIndex(grp.Key.Register, new List<string>())
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();

            //odto.resourceId.Add((int)odto.RegisterID);


            var oresult = new
            {
                flights = xflights,
                resgroups,
                ressq
            };
            //////////////////////
            return Ok(oresult);
        }



        [Route("api/plan/cancel/group")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightCancelGroup(cnlregs dto)
        {
            var _context = new Models.dbEntities();
             
            ///////////
            ///////////
            var intervalDays = GetInvervalDates((int)dto.interval, (DateTime)dto.intervalFrom, (DateTime)dto.intervalTo, dto.days).Select(q => (Nullable<DateTime>)q).ToList();
            var baseFlights = await _context.FlightInformations.Where(q => dto.fids.Contains(q.ID)).ToListAsync();
            var fltNumbers = baseFlights.Select(q => q.FlightNumber).ToList();
            var groupIds = baseFlights.Select(q => q.FlightGroupID).ToList();
            var fltIds = new List<int>();
            fltIds = baseFlights.Select(q => q.ID).ToList();

            var _flightIds = await (from x in _context.ViewLegTimes
                                    where fltNumbers.Contains(x.FlightNumber) && groupIds.Contains(x.FlightPlanId) && intervalDays.Contains(x.STDDay)
                                    select x.ID).ToListAsync();
            fltIds = fltIds.Concat(_flightIds).Distinct().ToList();

            var flights = await _context.FlightInformations.Where(q => fltIds.Contains(q.ID)).ToListAsync();
            var legs = await _context.ViewLegTimes.Where(q => fltIds.Contains(q.ID)).ToListAsync();
            foreach (var fid in fltIds)
            {
                var flight = flights.FirstOrDefault(q => q.ID == fid);
                var leg = legs.FirstOrDefault(q => q.ID == fid);

                var changeLog = new FlightChangeHistory()
                {
                    Date = DateTime.Now,
                    FlightId = flight.ID,
                    User = dto.userName,

                };
                changeLog.OldFlightNumer = leg.FlightNumber;
                changeLog.OldFromAirportId = leg.FromAirport;
                changeLog.OldToAirportId = leg.ToAirport;
                changeLog.OldSTD = flight.STD;
                changeLog.OldSTA = flight.STA;
                changeLog.OldStatusId = flight.FlightStatusID;
                changeLog.OldRegister = leg.RegisterID;
                changeLog.OldOffBlock = flight.ChocksOut;
                changeLog.OldOnBlock = flight.ChocksIn;
                changeLog.OldTakeOff = flight.Takeoff;
                changeLog.OldLanding = flight.Landing;

                //////////////////////////////////////////////////////////////

                flight.DateCreate = DateTime.Now.ToUniversalTime();
                flight.FlightStatusUserId = dto.userId;


                flight.FlightStatusID = 4;

                var cnlMsn = 84; //await _context .Ac_MSN.Where(q => q.Register == "CNL").Select(q => q.ID).FirstOrDefaultAsync();
                // flight.JLBLHH = flight.RegisterID;
                flight.CPRegister = leg.Register;
                flight.RegisterID = cnlMsn;
                flight.CancelDate = DateTime.Now;
                flight.CancelReasonId = dto.reason;
                flight.DepartureRemark += (!string.IsNullOrEmpty(flight.DepartureRemark) ? "\r\n" : "") + dto.remark + "(CNL REMARK BY:" + dto.userName + ")";
                //2020-11-24




                if (flight.FlightStatusID != null && /*dto.UserId != null*/ !string.IsNullOrEmpty(dto.userName))
                    _context.FlightStatusLogs.Add(new FlightStatusLog()
                    {
                        FlightID = flight.ID,

                        Date = DateTime.Now.ToUniversalTime(),
                        FlightStatusID = (int)flight.FlightStatusID,

                        UserId = dto.userId != null ? (int)dto.userId : 128000,
                        Remark = dto.userName,
                    });


                UpdateFirstLastFlights(flight.ID,_context);



                ////////////////////////////////////////
                changeLog.NewFlightNumber = leg.FlightNumber;
                changeLog.NewFromAirportId = leg.FromAirport;
                changeLog.NewToAirportId = flight.ToAirportId;
                changeLog.NewSTD = flight.STD;
                changeLog.NewSTA = flight.STA;
                changeLog.NewStatusId = flight.FlightStatusID;
                changeLog.NewRegister = leg.RegisterID;
                changeLog.NewOffBlock = flight.ChocksOut;
                changeLog.NewOnBlock = flight.ChocksIn;
                changeLog.NewTakeOff = flight.Takeoff;
                changeLog.NewLanding = flight.Landing;

                _context.FlightChangeHistories.Add(changeLog);
            }


            

            bool sendNira = false;
            var nullfids = dto.fids.Select(q => (Nullable<int>)q).ToList();

            var offCrewIds = (from q in _context.ViewFlightCrewNews
                              where nullfids.Contains(q.FlightId)
                              group q by q.FlightId into grp
                              select new offcrew() { flightId = grp.Key, crews = grp.Select(w => w.CrewId).ToList() }

                             ).ToList();
            var result = new updateLogResult()
            {
                sendNira = sendNira,
                flight = -1, //flight.ID,
                //offIds = offCrewIds
                offcrews = offCrewIds,
                fltIds = fltIds

            };

            /////
            ///********************
            ///*******************
            // var result = await unitOfWork.FlightRepository.CancelFlightsGroup(dto);

            ////////////////////
            ///////////////////
            ///




            var saveResult = await _context.SaveChangesAsync();

             

             var fresult = result  as updateLogResult;
            //if (fresult.offcrews != null && fresult.offcrews.Count > 0)
            //{
            //    foreach (var rec in fresult.offcrews)
            //    {
            //        foreach (var crewid in rec.crews)
            //        {
            //            await unitOfWork.FlightRepository.RemoveItemsFromFDP(rec.flightId.ToString(), (int)crewid, 2, "Flight Cancellation - Removed by AirPocket.", 0, 0);
            //        }
            //    }

            //}


            var beginDate = ((DateTime)dto.RefDate).Date;
            var endDate = ((DateTime)dto.RefDate).Date.AddDays((int)dto.RefDays).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var fg = await _context.ViewFlightsGantts.Where(q => fresult.fltIds.Contains(q.ID)
             && q.STDDay >= beginDate && q.STDDay <= endDate
            ).ToListAsync();
            var xflights = new List<ViewFlightsGanttDto>();
            foreach (var x in fg)
            {
                ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();
                ViewModels.ViewFlightsGanttDto.FillDto(x, odto, 0, 1);
                xflights.Add(odto);
            }



            var resgroups = from x in fg
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                           into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var ressq = (from x in fg
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp

                         orderby getOrderIndex(grp.Key.Register, new List<string>())
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();

            //odto.resourceId.Add((int)odto.RegisterID);


            var oresult = new
            {
                flights=xflights,
                resgroups,
                ressq
            };
            //////////////////////
            return Ok(oresult);

        }



        internal string UpdateFirstLastFlights(int flightId,dbEntities _context)
        //(int fdpId,int fdpItemId,bool off)
        {
            //var viewFdpItems = this.context.ViewFDPItems.AsNoTracking().Where(q => q.FlightId == flightId).ToList();
            // var fdpItems = this.context.FDPItems.Where(q => q.FlightId == flightId).ToList();
            //var fdpIds = viewFdpItems.Select(q => q.FDPId).Distinct().ToList();
            var fdps = (from x in _context.FDPs
                        join y in _context.FDPItems on x.Id equals y.FDPId
                        where y.FlightId == flightId
                        select x).ToList();
            var fdpIds = fdps.Select(q => q.Id).Distinct().ToList();
            var viewFdpItems = (from x in _context.ViewFDPItems.AsNoTracking()
                                where fdpIds.Contains(x.FDPId)
                                select x).ToList();


            foreach (var f in fdps)
            {
                var fdpId = f.Id;
                var viewItems = viewFdpItems.Where(q => q.FDPId == fdpId).ToList();
                var firstItem = viewFdpItems.Where(q => (q.IsOff == null || q.IsOff == false) && q.FlightId != flightId).OrderBy(q => q.STD).FirstOrDefault();
                var lastItem = viewFdpItems.Where(q => (q.IsOff == null || q.IsOff == false) && q.FlightId != flightId).OrderByDescending(q => q.STD).FirstOrDefault();
                if (firstItem != null)
                    f.FirstFlightId = firstItem.FlightId;
                if (lastItem != null)
                    f.LastFlightId = lastItem.FlightId;
                if (f.UPD == null)
                    f.UPD = 1;
                else
                    f.UPD++;
            }


            return string.Empty;


        }


        //[Route("api/plan/flight/save")]

        //[AcceptVerbs("POST")]
        //public async Task<IHttpActionResult> PostFlight(ViewModels.FlightDto dto)
        //{

        //    var _context = new Models.dbEntities();
        //    //var validate = unitOfWork.FlightRepository.ValidateFlight(dto);
        //    //if (validate.Code != HttpStatusCode.OK)
        //    //    return validate;

        //    FlightInformation entity = null;
        //    FlightChangeHistory changeLog = null;
        //    if (dto.ID == -1)
        //    {
        //        entity = new FlightInformation();
        //        _context.FlightInformations.Add(entity);
        //    }

        //    else
        //    {
        //        entity = await unitOfWork.FlightRepository.GetFlightById(dto.ID);
        //        if (entity == null)
        //            return Exceptions.getNotFoundException();
        //        unitOfWork.FlightRepository.RemoveFlightLink(dto.ID);

        //        changeLog = new FlightChangeHistory()
        //        {
        //            Date = DateTime.Now,
        //            FlightId = entity.ID,

        //        };
        //        unitOfWork.FlightRepository.Insert(changeLog);
        //        changeLog.OldFlightNumer = entity.FlightNumber;
        //        changeLog.OldFromAirportId = entity.FromAirportId;
        //        changeLog.OldToAirportId = entity.ToAirportId;
        //        changeLog.OldSTD = entity.STD;
        //        changeLog.OldSTA = entity.STA;
        //        changeLog.OldStatusId = entity.FlightStatusID;
        //        changeLog.OldRegister = entity.RegisterID;
        //        changeLog.OldOffBlock = entity.ChocksOut;
        //        changeLog.OldOnBlock = entity.ChocksIn;
        //        changeLog.OldTakeOff = entity.Takeoff;
        //        changeLog.OldLanding = entity.Landing;
        //        changeLog.User = dto.UserName;

        //    }


        //    if (entity.STD != null)
        //    {
        //        var oldSTD = ((DateTime)entity.STD).AddMinutes(270).Date;
        //        var newSTD = ((DateTime)dto.STD).AddMinutes(270).Date;
        //        if (oldSTD != newSTD)
        //        {
        //            entity.FlightDate = oldSTD;
        //        }
        //    }


        //    ViewModels.FlightDto.Fill(entity, dto);
        //    if (dto.ID != -1 && changeLog != null)
        //    {
        //        entity.RegisterID = changeLog.OldRegister;
        //        changeLog.NewFlightNumber = entity.FlightNumber;
        //        changeLog.NewFromAirportId = entity.FromAirportId;
        //        changeLog.NewToAirportId = entity.ToAirportId;
        //        changeLog.NewSTD = entity.STD;
        //        changeLog.NewSTA = entity.STA;
        //        changeLog.NewStatusId = entity.FlightStatusID;
        //        changeLog.NewRegister = entity.RegisterID;
        //        changeLog.NewOffBlock = entity.ChocksOut;
        //        changeLog.NewOnBlock = entity.ChocksIn;
        //        changeLog.NewTakeOff = entity.Takeoff;
        //        changeLog.NewLanding = entity.Landing;
        //    }
        //    entity.BoxId = dto.BoxId;

        //    if (dto.LinkedFlight != null)
        //    {
        //        var link = new FlightLink()
        //        {
        //            FlightInformation = entity,
        //            Flight2Id = (int)dto.LinkedFlight,
        //            ReasonId = (int)dto.LinkedReason,
        //            Remark = dto.LinkedRemark

        //        };
        //        unitOfWork.FlightRepository.Insert(link);
        //    }

        //    var saveResult = await unitOfWork.SaveAsync();
        //    if (saveResult.Code != HttpStatusCode.OK)
        //        return saveResult;

        //    if (dto.SMSNira == 1)
        //    {
        //        await unitOfWork.FlightRepository.NotifyNira(entity.ID, dto.UserName);
        //    }

        //    //bip
        //    var fg = await unitOfWork.FlightRepository.GetViewFlightGantts().Where(q => q.ID == entity.ID).ToListAsync();
        //    ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();
        //    ViewModels.ViewFlightsGanttDto.FillDto(fg.First(), odto, 0, 1);


        //    var resgroups = from x in fg
        //                    group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
        //                   into grp
        //                    select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
        //    var ressq = (from x in fg
        //                 group x by new { x.RegisterID, x.Register, x.TypeId }
        //             into grp

        //                 orderby unitOfWork.FlightRepository.getOrderIndex(grp.Key.Register, new List<string>())
        //                 select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();

        //    odto.resourceId.Add((int)odto.RegisterID);


        //    var oresult = new
        //    {
        //        flight = odto,
        //        resgroups,
        //        ressq
        //    };

        //    return Ok(/*entity*/oresult);

        //}

        public string getOrderIndex(string reg, List<string> grounds)
        {
            var str = "";
            //orderby grp.Key.Register.Contains("CNL") ? "ZZZZ" :( grp.Key.Register[grp.Key.Register.Length - 1].ToString())
            if (reg.Contains("CNL"))
                str = "ZZZZZZ";
            else if (reg.Contains("RBC"))
                str = "ZZZZZY";
            else
           //str = 1000000;
           // if (grounds.Contains(reg))
           if (reg.Contains("."))
            {
                str = "ZZZZ" + reg[reg.Length - 2];
                //str = 100000;
            }
            // str= reg[reg.Length - 1].ToString();
            else
                str = reg[reg.Length - 1].ToString();

            return str;

        }



    }
    public class SimpleDto
    {
        public string username { get; set; }
        public List<int> ids { get; set; }
    }
}
