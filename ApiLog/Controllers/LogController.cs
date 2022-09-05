using ApiLog.Models;
using ApiLog.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiLog.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogController : ApiController
    {
        [Route("api/flight/log/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostFlightLog(ViewModels.FlightSaveDto dto)
        {
            if (dto.UpdateDelays == null)
                dto.UpdateDelays = 1;
            List<int> offCrewIds = new List<int>();
            //marmar
            // return new CustomActionResult(HttpStatusCode.OK, null);
            var context = new ppa_entities();
            var flight = await context.FlightInformations.FirstOrDefaultAsync(q => q.ID == dto.ID);
            var notifiedDelay = flight.NotifiedDelay;
            var leg = await context.ViewLegTimes.FirstOrDefaultAsync(q => q.ID == dto.ID);
            if (flight == null)
                return new CustomActionResult(HttpStatusCode.NotFound, "");

            var changeLog = new FlightChangeHistory()
            {
                Date = DateTime.Now,
                FlightId = flight.ID,
                User = dto.UserName,

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
            flight.GUID = Guid.NewGuid();
            flight.DateCreate = DateTime.Now.ToUniversalTime();
            flight.FlightStatusUserId = dto.UserId;
            flight.ChocksIn = dto.ChocksIn;
            flight.Landing = dto.Landing;
            flight.ChocksOut = dto.ChocksOut;
            flight.Takeoff = dto.Takeoff;
            flight.GWTO = dto.GWTO;

            flight.LTR = dto.LTR;
            
            flight.SerialNo = dto.SerialNo;
            flight.FuelDensity = dto.FuelDensity;
            //flight.FuelDeparture = dto.FuelDeparture;
            // flight.FuelArrival = dto.FuelArrival;
            flight.PaxAdult = dto.PaxAdult;
            flight.PaxInfant = dto.PaxInfant;
            flight.PaxChild = dto.PaxChild;
            flight.NightTime = dto.NightTime;
            flight.CargoWeight = dto.CargoWeight;
            flight.CargoUnitID = dto.CargoUnitID;
            flight.BaggageCount = dto.BaggageCount;
            flight.CargoCount = dto.CargoCount;
            flight.BaggageWeight = dto.BaggageWeight;
            //flight.FuelUnitID = dto.FuelUnitID;
            flight.DepartureRemark = dto.DepartureRemark;
            flight.FPFlightHH = dto.FPFlightHH;
            flight.FPFlightMM = dto.FPFlightMM;
            //flight.FPFuel = dto.FPFuel;
            flight.Defuel = dto.Defuel;
            // flight.UsedFuel = dto.UsedFuel;
            flight.JLBLHH = dto.JLBLHH;
            flight.JLBLMM = dto.JLBLMM;
            flight.PFLR = dto.PFLR;
            flight.ChrAdult = dto.ChrAdult;
            flight.ChrCapacity = dto.ChrCapacity;
            flight.ChrChild = dto.ChrChild;
            flight.ChrInfant = dto.ChrInfant;
            flight.ChrCode = dto.ChrCode;
            flight.ChrTitle = dto.ChrTitle;
            flight.ArrivalRemark = dto.ArrivalRemark;
            if (dto.FlightStatusID != null)
                flight.FlightStatusID = dto.FlightStatusID;
            if (flight.FlightStatusID == null)
                flight.FlightStatusID = 1;
            if (flight.FlightStatusID == 4)
            {
                var cnlMsn = await context.Ac_MSN.Where(q => q.Register == "CNL").Select(q => q.ID).FirstOrDefaultAsync();
                flight.RegisterID = cnlMsn;
                flight.CancelDate = dto.CancelDate;
                flight.CancelReasonId = dto.CancelReasonId;
            }
            //if (flight.FlightStatusID == 17)
            if (dto.RedirectReasonId != null)
            {

                flight.RedirectDate = dto.RedirectDate;
                flight.RedirectReasonId = dto.RedirectReasonId;
                flight.RedirectRemark = dto.RedirectRemark;
                if (flight.OSTA == null)
                {
                    var vflight = await context.ViewFlightInformations.FirstOrDefaultAsync(q => q.ID == flight.ID);
                    flight.OSTA = flight.STA;
                    flight.OToAirportId = vflight.ToAirport;
                    flight.OToAirportIATA = vflight.ToAirportIATA;
                }

                // var airport = await context.Airports.FirstOrDefaultAsync(q => q.Id == flight.OToAirportId);
                flight.ToAirportId = dto.ToAirportId;
                // if (airport != null)
                //    flight.OToAirportIATA = airport.IATA;
            }
            else
            {
                flight.RedirectDate = null;
                flight.RedirectReasonId = null;
                // if (flight.FlightPlanId != null)
                //    flight.ToAirportId = null;
                flight.OSTA = null;
                flight.OToAirportId = null;
                flight.OToAirportIATA = null;

            }
            if (flight.FlightStatusID == 9)
            {
                flight.RampDate = dto.RampDate;
                flight.RampReasonId = dto.RampReasonId;
            }

            if (flight.ChocksIn != null && flight.FlightStatusID == 15)
            {
                //var vflight = await context.ViewFlightInformations.FirstOrDefaultAsync(q => q.ID == flight.ID);

                //var flightCrewEmployee = await (from x in context.Employees
                //                                join y in context.ViewFlightCrewNews on x.Id equals y.CrewId
                //                                where y.FlightId == flight.ID
                //                                select x).ToListAsync();

                //foreach (var x in flightCrewEmployee)
                //    x.CurrentLocationAirport = vflight.ToAirport;
                var flightCrews = await (from x in context.Employees
                                         join z in context.FDPs on x.Id equals z.CrewId
                                         join y in context.FDPItems on z.Id equals y.FDPId
                                         where y.FlightId == flight.ID
                                         select x).ToListAsync();

                foreach (var x in flightCrews)
                    x.CurrentLocationAirport = flight.ToAirportId;
            }
            if (flight.FlightStatusID != null && /*dto.UserId != null*/ !string.IsNullOrEmpty(dto.UserName))
               context.FlightStatusLogs.Add(new FlightStatusLog()
                {
                    FlightID = dto.ID,

                    Date = DateTime.Now.ToUniversalTime(),
                    FlightStatusID = (int)flight.FlightStatusID,

                    UserId = dto.UserId != null ? (int)dto.UserId : 128000,
                    Remark = dto.UserName,
                });
            var result = UpdateDelays(dto,context);

            if (result.Code != HttpStatusCode.OK)
                return result;

          //  var result2 = await UpdateEstimatedDelays(dto);

            //if (result2.Code != HttpStatusCode.OK)
            //    return result2;

            if (flight.FlightStatusID == 4)
            {
                UpdateFirstLastFlights(flight.ID,context);
            }

           
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

            context.FlightChangeHistories.Add(changeLog);
            ////////////////////////////////////////

            bool sendNira = false;
            try
            {
                var xdelay = (int)(((DateTime)dto.ChocksOut) - ((DateTime)leg.STD)).TotalMinutes;
                if (xdelay > 30 && (flight.FlightStatusID == 1) && notifiedDelay != xdelay && ((DateTime)flight.STD - DateTime.UtcNow).TotalHours > 1)
                {
                    flight.NotifiedDelay = xdelay;
                    sendNira = true;
                }
                if (dto.FlightStatusID == 4)
                {
                   // offCrewIds = (from q in context.ViewFlightCrewNews
                   //               where q.FlightId == dto.ID
                   //               select q.CrewId).ToList();
                    offCrewIds = await (from x in context.Employees
                                             join z in context.FDPs on x.Id equals z.CrewId
                                             join y in context.FDPItems on z.Id equals y.FDPId
                                             where y.FlightId == flight.ID
                                             select x.Id).ToListAsync();



                }





            }
            catch (Exception ex)
            {

            }



            //return new CustomActionResult(HttpStatusCode.OK, new updateLogResult()
            //{
            //    sendNira = sendNira,
            //    flight = flight.ID,
            //    offIds = offCrewIds

            //});

            var saveResult = await context.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            if (offCrewIds != null && offCrewIds.Count > 0)
            {
                var disoffIds = offCrewIds.Distinct().ToList();
                foreach (var crewid in disoffIds)
                {
                    await  RemoveItemsFromFDP(flight.ID.ToString(), (int)crewid, 2, "Flight Cancellation - Removed by AirPocket.", 0, 0);
                }
            }



            //var fg = await unitOfWork.FlightRepository.GetViewFlightGantts().Where(q => q.ID == fresult.flight).ToListAsync();
            var fg = await context.ViewFlightsGantts.Where(q => q.ID == flight.ID).ToListAsync();
            ViewModels.ViewFlightsGanttDto odto = new ViewFlightsGanttDto();
            ViewModels.ViewFlightsGanttDto.FillDto(fg.First(), odto, 0, 1);


            var resgroups = from x in fg
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                           into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };
            var ressq = (from x in fg
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp

                         orderby getOrderIndex(grp.Key.Register, new List<string>())
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();

            odto.resourceId.Add((int)odto.RegisterID);


            var oresult = new
            {
                flight = odto,
                resgroups,
                ressq
            };
            //await unitOfWork.FlightRepository.CreateMVTMessage(dto.ID,dto.UserName);
            //6-28
           // unitOfWork.FlightRepository.CreateMVTMessage(dto.ID, dto.UserName);
            return Ok(oresult);





        }

        internal string UpdateFirstLastFlights(int flightId, ppa_entities context)
        //(int fdpId,int fdpItemId,bool off)
        {
           
            var fdps = (from x in  context.FDPs
                        join y in context.FDPItems on x.Id equals y.FDPId
                        where y.FlightId == flightId
                        select x).ToList();
            var fdpIds = fdps.Select(q => q.Id).Distinct().ToList();
            var viewFdpItems = (from x in  context.ViewFDPItems.AsNoTracking()
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

        public CustomActionResult UpdateDelays(ViewModels.FlightSaveDto dto, ppa_entities context)
        {
           // if (!string.IsNullOrEmpty(dto.UserName) && dto.UserName.ToLower().StartsWith("aps."))
           //     return new CustomActionResult(HttpStatusCode.OK, "");
           if (dto.UpdateDelays!=1)
                return new CustomActionResult(HttpStatusCode.OK, "");
            var currentDelays = context.FlightDelays.Where(q => q.FlightId == dto.ID);
            context.FlightDelays.RemoveRange(currentDelays);
            foreach (var x in dto.Delays)
            {
                context.FlightDelays.Add(new FlightDelay()
                {
                    DelayCodeId = x.DelayCodeId,
                    FlightId = dto.ID,
                    MM = x.MM,
                    HH = x.HH,
                    Remark = x.Remark,
                    UserId = x.UserId
                });
            }

            return new CustomActionResult(HttpStatusCode.OK, "");
        }


        internal async Task<CustomActionResult> RemoveItemsFromFDP(string strItems, int crewId, int reason, string remark, int notify, int noflight, string username = "")
        {
            ppa_entities context = new ppa_entities();
            //var _fdpItemIds = strItems.Split('*').Select(q => Convert.ToInt32(q)).Distinct().ToList();
            //var _fdpIds = strfdps.Split('*').Select(q => Convert.ToInt32(q)).Distinct().ToList();

            var flightIds = strItems.Split('*').Select(q => (Nullable<int>)Convert.ToInt32(q)).Distinct().ToList();
           // var X_fdpItemIds = await context.ViewFDPItem2.Where(q => q.CrewId == crewId && flightIds.Contains(q.FlightId)).OrderBy(q => q.STD).Select(q => q.Id).ToListAsync();

            var _fdpItemIds =await ( from x in context.FDPs
                              join y in context.FDPItems on x.Id equals y.FDPId
                              where flightIds.Contains(y.FlightId) && x.CrewId==crewId
                              select y.Id).ToListAsync();
            var allRemovedItems = await context.FDPItems.Where(q => _fdpItemIds.Contains(q.Id)).ToListAsync();
            var _fdpIds = allRemovedItems.Select(q => q.FDPId).ToList();
            var fdps = await context.FDPs.Where(q => _fdpIds.Contains(q.Id)).ToListAsync();
            var fdpItems = await context.FDPItems.Where(q => _fdpIds.Contains(q.FDPId)).ToListAsync();


            var allFlightIds = fdpItems.Select(q => q.FlightId).ToList();
            var allFlights = await context.ViewLegTimes.Where(q => allFlightIds.Contains(q.ID)).OrderBy(q => q.STD).ToListAsync();
            var crews = await context.ViewEmployeeLights.Where(q =>q.Id==crewId).ToListAsync();
            var allRemovedFlights = allFlights.Where(q => flightIds.Contains(q.ID)).OrderBy(q => q.STD).ToList();
            FDP offFDP = null;
            string offSMS = string.Empty;
            List<string> sms = new List<string>();
            List<string> nos = new List<string>();
            List<CrewPickupSM> csms = new List<CrewPickupSM>();
            if (reason != -1)
            {


                offFDP = new FDP()
                {
                    CrewId = crewId,
                    DateStart = allRemovedFlights.First().STD,
                    DateEnd = allRemovedFlights.Last().STA,
                    InitStart = allRemovedFlights.First().STD,
                    InitEnd = allRemovedFlights.Last().STA,

                    InitRestTo = allRemovedFlights.Last().STA,
                    InitKey = allRemovedFlights.First().ID.ToString(),
                    DutyType = 0,
                    GUID = Guid.NewGuid(),
                    IsTemplate = false,
                    Remark = remark,
                    UPD = 1,
                    UserName = username


                };
                offFDP.CanceledNo = string.Join(",", allRemovedFlights.Select(q => q.FlightNumber));
                offFDP.CanceledRoute = string.Join(",", allRemovedFlights.Select(q => q.FromAirportIATA)) + "," + allRemovedFlights.Last().ToAirportIATA;
                switch (reason)
                {
                    case 1:
                        offFDP.DutyType = 100009;
                        offFDP.Remark2 = "Refused by Crew";
                        break;
                    case 5:
                        offFDP.DutyType = 100020;
                        offFDP.Remark2 = "Cenceled due to Rescheduling";
                        break;
                    case 2:
                        offFDP.DutyType = 100021;
                        offFDP.Remark2 = "Cenceled due to Flight(s) Cancellation";
                        break;
                    case 3:
                        offFDP.DutyType = 100022;
                        offFDP.Remark2 = "Cenceled due to Change of A/C Type";
                        break;
                    case 4:
                        offFDP.DutyType = 100023;
                        offFDP.Remark2 = "Cenceled due to Flight/Duty Limitations";
                        break;
                    case 6:
                        offFDP.DutyType = 100024;
                        offFDP.Remark2 = "Cenceled due to Not using Split Duty";
                        break;


                    case 7:
                        offFDP.DutyType = 200000;
                        offFDP.Remark2 = "Refused-Not Home";
                        break;
                    case 8:
                        offFDP.DutyType = 200001;
                        offFDP.Remark2 = "Refused-Family Problem";
                        break;
                    case 9:
                        offFDP.DutyType = 200002;
                        offFDP.Remark2 = "Canceled - Training";
                        break;
                    case 10:
                        offFDP.DutyType = 200003;
                        offFDP.Remark2 = "Ground - Operation";
                        break;
                    case 11:
                        offFDP.DutyType = 200004;
                        offFDP.Remark2 = "Ground - Expired License";
                        break;
                    case 12:
                        offFDP.DutyType = 200005;
                        offFDP.Remark2 = "Ground - Medical";
                        break;
                    default:
                        break;
                }
                foreach (var x in allRemovedFlights)
                {
                    var _ofdpitem = fdpItems.FirstOrDefault(q => q.FlightId == x.ID);
                    string _oremark = string.Empty;
                    if (_ofdpitem != null)
                    {
                        var _ofdp = fdps.Where(q => q.Id == _ofdpitem.FDPId).FirstOrDefault();
                        if (_ofdp != null)
                            _oremark = _ofdp.InitRank;
                    }
                    offFDP.OffItems.Add(new OffItem() { FDP = offFDP, FlightId = x.ID, Remark = _oremark });
                }

                context.FDPs.Add(offFDP);



                var strs = new List<string>();
                strs.Add(ConfigurationManager.AppSettings["airline"] + " Airlines");
                strs.Add("Dear " + crews.FirstOrDefault(q => q.Id == crewId).Name + ", ");
                strs.Add("Canceling Notification");
                var day = ((DateTime)allRemovedFlights.First().STDLocal).Date;
                var dayStr = day.ToString("ddd") + " " + day.Year + "-" + day.Month + "-" + day.Day;
                strs.Add(dayStr);
                strs.Add(offFDP.CanceledNo);
                strs.Add(offFDP.CanceledRoute);
                strs.Add(offFDP.Remark2);
                strs.Add(remark);
                strs.Add("Date sent: " + DateTime.Now.ToLocalTime().ToString("yyyy/MM/dd HH:mm"));
                strs.Add("Crew Scheduling Department");
                offSMS = String.Join("\n", strs);
                sms.Add(offSMS);
                nos.Add(crews.FirstOrDefault(q => q.Id == crewId).Mobile);

                var csm = new CrewPickupSM()
                {
                    CrewId = (int)crewId,
                    DateSent = DateTime.Now,
                    DateStatus = DateTime.Now,
                    FlightId = -1,
                    Message = offSMS,
                    Pickup = null,
                    RefId = "",
                    Status = "",
                    Type = offFDP.DutyType,
                    FDP = offFDP,
                    DutyType = offFDP.Remark2,
                    DutyDate = ((DateTime)offFDP.InitStart).ToLocalTime().Date,
                    Flts = offFDP.CanceledNo,
                    Routes = offFDP.CanceledRoute
                };
                csms.Add(csm);
                if (notify == 1)
                    context.CrewPickupSMS.Add(csm);
            }

          

            foreach (var x in allRemovedItems)
            {
                var xfdp = fdps.FirstOrDefault(q => q.Id == x.FDPId);
                var xcrew = crews.FirstOrDefault(q => q.Id == xfdp.CrewId);
                var xleg = allFlights.FirstOrDefault(q => q.ID == x.FlightId);


            }

            var updatedIds = new List<int>();
            var updated = new List<FDP>();
            var removed = new List<int>();


            //  List<FDP> deleted = new List<FDP>();
            foreach (var fdp in fdps)
            {
                fdp.Split = 0;
                var allitems = fdpItems.Where(q => q.FDPId == fdp.Id).ToList();
                var removedItems = allitems.Where(q => _fdpItemIds.Contains(q.Id)).ToList();
                var remainItems = allitems.Where(q => !_fdpItemIds.Contains(q.Id)).ToList();
                var remainFlightIds = remainItems.Select(q => q.FlightId).ToList();
                if (allitems.Count == removedItems.Count)
                {
                    removed.Add(fdp.Id);
                    context.FDPItems.RemoveRange(removedItems);

                    context.FDPs.Remove(fdp);
                }
                else
                {
                    //Update FDP

                    context.FDPItems.RemoveRange(removedItems);
                    var items = (from x in remainItems
                                 join y in allFlights on x.FlightId equals y.ID
                                 orderby y.STD
                                 select new { fi = x, flt = y }).ToList();
                    fdp.FirstFlightId = items.First().flt.ID;
                    fdp.LastFlightId = items.Last().flt.ID;
                    fdp.InitStart = ((DateTime)items.First().flt.STD).AddMinutes(-60);
                    fdp.InitEnd = ((DateTime)items.Last().flt.STA).AddMinutes(30);

                    fdp.DateStart = ((DateTime)items.First().flt.STD).AddMinutes(-60);
                    fdp.DateEnd = ((DateTime)items.Last().flt.STA).AddMinutes(30);

                    var rst = 12;
                    if (fdp.InitHomeBase != null && fdp.InitHomeBase != items.Last().flt.ToAirport)
                        rst = 10;
                    fdp.InitRestTo = ((DateTime)items.Last().flt.STA).AddMinutes(30).AddHours(rst);
                    fdp.InitFlts = string.Join(",", items.Select(q => q.flt).Select(q => q.FlightNumber).ToList());
                    fdp.InitRoute = string.Join(",", items.Select(q => q.flt).Select(q => q.FromAirportIATA).ToList());
                    fdp.InitRoute += "," + items.Last().flt.ToAirportIATA;
                    fdp.InitFromIATA = items.First().flt.FromAirport.ToString();
                    fdp.InitToIATA = items.Last().flt.ToAirport.ToString();
                    fdp.InitNo = string.Join("-", items.Select(q => q.flt).Select(q => q.FlightNumber).ToList());
                    fdp.InitKey = string.Join("-", items.Select(q => q.flt).Select(q => q.ID).ToList());
                    fdp.InitFlights = string.Join("*", items.Select(q => q.flt.ID + "_" + (q.fi.IsPositioning == true ? "1" : "0") + "_" + ((DateTime)q.flt.STDLocal).ToString("yyyyMMddHHmm")
                      + "_" + ((DateTime)q.flt.STALocal).ToString("yyyyMMddHHmm")
                      + "_" + q.flt.FlightNumber + "_" + q.flt.FromAirportIATA + "_" + q.flt.ToAirportIATA).ToList()
                    );

                    var keyParts = new List<string>();
                    keyParts.Add(items[0].flt.ID + "*" + (items[0].fi.IsPositioning == true ? "1" : "0"));
                    var breakGreaterThan10Hours = string.Empty;
                    for (int i = 1; i < items.Count; i++)
                    {

                        keyParts.Add(items[i].flt.ID + "*" + (items[i].fi.IsPositioning == true ? "1" : "0"));
                        var dt = (DateTime)items[i].flt.STD - (DateTime)items[i - 1].flt.STA;
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
                            var xfdpitem = allitems.FirstOrDefault(q => q.Id == items[i].fi.Id);
                            xfdpitem.SplitDuty = true;
                            var pair = allitems.FirstOrDefault(q => q.Id == items[i - 1].fi.Id);
                            pair.SplitDuty = true;
                            xfdpitem.SplitDutyPairId = pair.FlightId;
                            fdp.Split += 0.5 * (brk);

                        }

                    }
                    fdp.UPD = fdp.UPD == null ? 1 : ((int)fdp.UPD) + 1;
                    fdp.Key = string.Join("_", keyParts);
                    fdp.UserName = username;
                    //var flights = allFlights.Where(q => remainFlightIds.Contains(q.ID)).OrderBy(q=>q.STD).ToList();
                    updatedIds.Add(fdp.Id);
                    updated.Add(fdp);

                }
            }





            var saveResult = await context.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;

            var fdpsIds = fdps.Select(q => q.Id).ToList();
            var maxfdps = await context.HelperMaxFDPs.Where(q => fdpsIds.Contains(q.Id)).ToListAsync();
            var fdpExtras = await context.FDPExtras.Where(q => fdpsIds.Contains(q.FDPId)).ToListAsync();
            context.FDPExtras.RemoveRange(fdpExtras);
            foreach (var x in maxfdps)
            {
                context.FDPExtras.Add(new FDPExtra()
                {
                    FDPId = x.Id,
                    MaxFDP = Convert.ToDecimal(x.MaxFDPExtended),
                });
            }
            saveResult = await context.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;
            //if (notify == 1)
            //{
            //    Magfa m = new Magfa();
            //    int c = 0;
            //    foreach (var x in sms)
            //    {
            //        var txt = sms[c];
            //        var no = nos[c];

            //        var smsResult = m.enqueue(1, no, txt)[0];
            //        c++;

            //    }
            //}

            //updated = await context.ViewFDPKeys.Where(q => updatedIds.Contains(q.Id)).ToListAsync();

            var result = new
            {
                removed,
                updatedId = updated.Select(q => q.Id).ToList(),
                //updated = getRosterFDPDtos(updated)
            };

            return new CustomActionResult(HttpStatusCode.OK, result);
        }


        public string getOrderIndex(string reg, List<string> grounds)
        {
            var str = "";
            //orderby grp.Key.Register.Contains("CNL") ? "ZZZZ" :( grp.Key.Register[grp.Key.Register.Length - 1].ToString())
            if (reg.Contains("CNL"))
                str = "ZZZZZZ";
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



        [Route("api/flights/gantt/utc/customer/{cid}/{from}/{to}/{tzoffset}")]
        [AcceptVerbs("POST", "GET")]
        public async Task<IHttpActionResult> GetFlightsGanttByCustomerIdUTC(int cid, string from, string to, int tzoffset)
        {
            DateTime dateFrom =Helper.BuildDateTimeFromYAFormat(from);
            DateTime dateTo = Helper.BuildDateTimeFromYAFormat(to);

            // var result = await unitOfWork.FlightRepository.GetFlightGanttFleet(cid, dateFrom, dateTo, tzoffset, null, null, 1);
            //return Ok(result);
            var context = new ppa_entities();
           
            var flightsQuery = context.ViewFlightsGantts.Where(q => /*q.CustomerId == cid &&*/ q.RegisterID != null &&
            (
            (q.STDLocal >= dateFrom && q.STDLocal <= dateTo) || (q.DepartureLocal >= dateFrom && q.DepartureLocal <= dateTo)
            || (q.STALocal >= dateFrom && q.STALocal <= dateTo) || (q.ArrivalLocal >= dateFrom && q.ArrivalLocal <= dateTo)
            )
            );
            int utc = 1;
            int? doUtc = utc;
            if (cid != -1)
                flightsQuery = flightsQuery.Where(q => q.CustomerId == cid);
            
           
            
            var flights = await flightsQuery.ToListAsync();

            
            
            var grounds = (from x in context.ViewRegisterGrounds
                           where x.CustomerId == cid &&
                           (
                            (dateFrom >= x.DateFrom && dateTo <= x.DateEnd) ||
                            (x.DateFrom >= dateFrom && x.DateEnd <= dateTo) ||

                            (x.DateFrom >= dateFrom && x.DateFrom <= dateTo) ||
                            (x.DateEnd >= dateFrom && x.DateEnd <= dateTo)
                           )
                           select x).ToList();
            
            flights = flights.OrderBy(q => q.STD).ToList();


            var groundRegs = new List<string>();

            var flightsdto = new List<ViewModels.ViewFlightsGanttDto>();
            foreach (var x in flights)
            {
                ViewModels.ViewFlightsGanttDto dto = new ViewFlightsGanttDto();
                ViewModels.ViewFlightsGanttDto.FillDto(x, dto, tzoffset, doUtc);
                flightsdto.Add(dto);
            }

            var resgroups = from x in flights
                            group x by new { x.AircraftType, AircraftTypeId = x.TypeId }
                            into grp
                            select new { groupId = grp.Key.AircraftTypeId, Title = grp.Key.AircraftType };


            //change other method
            var ressq = (from x in flights
                         group x by new { x.RegisterID, x.Register, x.TypeId }
                     into grp
                         //orderby grp.Key.TypeId, grp.Key.Register
                         // orderby grp.Key.Register.Contains("CNL")?"ZZZZ": grp.Key.Register[grp.Key.Register.Length-1].ToString()
                         orderby getOrderIndex(grp.Key.Register, groundRegs)
                         select new { resourceId = grp.Key.RegisterID, resourceName = grp.Key.Register, groupId = grp.Key.TypeId }).ToList();
            //var ress = ressq.OrderBy(q => q.TypeId).Select((q, i) => new { resourceName = q.Register, groupId = q.TypeId, resourceId = (q.RegisterID >= 0 ? q.RegisterID : -1 * (i + 1)) }).ToList();

            foreach (var x in flightsdto)
            {
                //if (x.RegisterID >= 0)
                x.resourceId.Add((int)x.RegisterID);
                // else
                //    x.resourceId.Add((int)ress.First(q => q.resourceName == x.Register).resourceId);
            }

           
            var fromAirport = (from x in flights
                               group x by new { x.FromAirport, x.FromAirportIATA, x.FromAirportName } into g
                               select new BaseSummary()
                               {
                                   BaseId = g.Key.FromAirport,
                                   BaseIATA = g.Key.FromAirportIATA,
                                   BaseName = g.Key.FromAirportName,
                                   Total = g.Count(),
                                   TakeOff = g.Where(q => q.Takeoff != null).Count(),
                                   Landing = 0, //g.Where(q => q.Landing != null).Count(),
                                   Canceled = g.Where(q => q.FlightStatusID == 4).Count(),
                                   Redirected = g.Where(q => q.FlightStatusID == 17).Count(),
                                   Diverted = g.Where(q => q.FlightStatusID == 7).Count(),
                                   TotalDelays = g.Where(q => q.ChocksOut != null).Sum(q => q.DelayOffBlock),
                                   DepartedPax = g.Where(q => q.Takeoff != null).Sum(q => q.TotalPax),
                                   ArrivedPax = 0,// g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

                               }).ToList();
            var toAirport = (from x in flights
                             group x by new { x.ToAirport, x.ToAirportIATA, x.ToAirportName } into g
                             select new BaseSummary()
                             {
                                 BaseId = g.Key.ToAirport,
                                 BaseIATA = g.Key.ToAirportIATA,
                                 BaseName = g.Key.ToAirportName,
                                 Total = g.Count(),
                                 TakeOff = 0,//g.Where(q => q.Takeoff != null).Count(),
                                 Landing = g.Where(q => q.Landing != null).Count(),
                                 Canceled = 0,//g.Where(q => q.FlightStatusID == 4).Count(),
                                 Redirected = 0,// g.Where(q => q.FlightStatusID == 17).Count(),
                                 Diverted = 0,// g.Where(q => q.FlightStatusID == 7).Count(),
                                 TotalDelays = 0,// g.Where(q => q.ChocksOut != null).Sum(q => q.DelayOffBlock),
                                 DepartedPax = 0,// g.Where(q => q.Takeoff != null).Sum(q => q.TotalPax),
                                 ArrivedPax = g.Where(q => q.Landing != null).Sum(q => q.TotalPax),

                             }).ToList();

            var baseSum = new List<BaseSummary>();
            foreach (var x in fromAirport)
            {
                var _to = toAirport.FirstOrDefault(q => q.BaseId == x.BaseId);
                if (_to != null)
                {
                    x.ArrivedPax += _to.ArrivedPax;
                    x.Canceled += _to.Canceled;
                    x.DepartedPax += _to.DepartedPax;
                    x.Diverted += _to.Diverted;
                    x.Landing += _to.Landing;
                    x.Redirected += _to.Redirected;
                    x.TakeOff += _to.TakeOff;
                    x.Total += _to.Total;
                    x.TotalDelays += _to.TotalDelays;

                }

                baseSum.Add(x);
            }

          

          
            var result = new
            {
                flights = flightsdto,
                resourceGroups = resgroups.ToList(),
                resources = ressq,
                baseSummary = baseSum,
                grounds,
                // fltgroups,
                baseDate = DateTime.UtcNow,
            };
            return Ok( result);


            ///////////////////////
            ///////////////////////
            //////////////////////
        }



    } 
}
