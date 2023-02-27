using ApiCAO.Models;
using ApiCAO.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiCAO.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogController : ApiController
    {
        public class mvtDelayObj
        {
            public int amount { get; set; }
            public string reasonCode { get; set; }
        }
        public class mvtFlightNumberObj
        {
            public string carrier { get; set; }
            public int number { get; set; }
            public string postFix { get; set; }
        }
        public class mvtPassengerObj
        {
            public int male { get; set; }
            public int female { get; set; }
            public int child { get; set; }
            public int infant { get; set; }
            public int adult { get; set; }
        }
        public class mvtObj
        {
            public string acRegister { get; set; }
            public string destination { get; set; }
            public Int64 flightDate { get; set; }
            public Int64 landingDate { get; set; }
            public string messageType { get; set; }
            public Int64 offBlockDate { get; set; }
            public Int64 onBlockDate { get; set; }
            public string origin { get; set; }
            public Int64 takeOffDate { get; set; }
            public List<mvtDelayObj> mvtDelays { get; set; }
            public mvtFlightNumberObj mvtFlightNumber { get; set; }
            public mvtPassengerObj mvtPassenger { get; set; }
        }



        [Route("api/cao/mvt/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCAOMVT(int id)
        {
            try
            {
                string iata = ConfigurationManager.AppSettings["iata"];
                string icao = ConfigurationManager.AppSettings["icao"];
                ppa_entities context = new ppa_entities();
                var flt = context.ViewLegTimes.Where(q => q.ID == id).FirstOrDefault();
                if (flt.FlightStatusID != 2 && !(flt.FlightStatusID == 15 || flt.FlightStatusID == 3))
                    return Ok();
                CaoMVTLog log = new CaoMVTLog()
                {
                    DateCreate = DateTime.Now,
                    FlightId = id,
                };
                context.CaoMVTLogs.Add(log);
                var delays = context.ViewFlightDelayCodes.Where(q => q.FlightId == id).OrderBy(q => q.Code).Select(q => new { q.Code, q.HH, q.MM }).ToList();

                var _type = flt.FlightStatusID == 2 ? "DEPARTURE" : "ARRIVAL";
                log.MessageType = _type;

                var mvt = new mvtObj()
                {
                    acRegister = flt.Register,
                    destination = flt.ToAirportIATA,
                    flightDate = Convert.ToInt64(((DateTimeOffset)flt.STDDay).ToUnixTimeSeconds()),
                    landingDate = Convert.ToInt64(((DateTimeOffset)flt.Landing).ToUnixTimeSeconds()),
                    messageType = _type,
                    offBlockDate = Convert.ToInt64(((DateTimeOffset)flt.ChocksOut).ToUnixTimeSeconds()),
                    onBlockDate = Convert.ToInt64(((DateTimeOffset)flt.ChocksIn).ToUnixTimeSeconds()),
                    takeOffDate = Convert.ToInt64(((DateTimeOffset)flt.Takeoff).ToUnixTimeSeconds()),
                    origin = flt.FromAirportIATA,
                    mvtDelays = new List<mvtDelayObj>(),
                    mvtFlightNumber = new mvtFlightNumberObj()
                    {
                        carrier = iata,
                        number = Convert.ToInt32(flt.FlightNumber.ToLower().Replace("a", "").Replace("b", "")),
                        postFix = icao,

                    },
                    mvtPassenger = new mvtPassengerObj()
                    {
                        child = flt.PaxChild == null ? 0 : (int)flt.PaxChild,
                        female = 0,
                        male = flt.PaxAdult == null ? 0 : (int)flt.PaxAdult,
                        infant = flt.PaxInfant == null ? 0 : (int)flt.PaxInfant,
                    }


                };
                if (delays.Count > 0)
                {

                    foreach (var x in delays)
                    {
                        mvt.mvtDelays.Add(new mvtDelayObj()
                        {
                            amount = (x.HH ?? 0) * 60 + (x.MM ?? 0),
                            reasonCode = x.Code
                        });
                    }


                }
                var mvts = new List<mvtObj>();
                mvts.Add(mvt);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://cao.raman-it.com/mvt");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = new JavaScriptSerializer().Serialize(new
                    //{
                    //    user = "Foo",
                    //    password = "Baz"
                    //});
                    string json = JsonConvert.SerializeObject(mvts);
                    log.Message = json;
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    log.Response = result;
                }
                context.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "   INNER: " + ex.InnerException.Message;
                return Ok(msg);
            }



        }
        public class AuthInfo
        {
            public string userName { get; set; }
            public string password { get; set; }
        }
        [Route("api/flight/status/{username}/{password}/{date}/{no}")]
        [AcceptVerbs("GET")]
        ///<summary>
        ///Get Status Of Flight
        ///</summary>
        ///<remarks>


        ///</remarks>
        public async Task<IHttpActionResult> PostFlightStatus(string username,string password, string date, string no)
        {
            try
            {
                if (!(username == "pnlairpocket" &&  password == "Pnl1234Za"))
                    return BadRequest("Authentication Failed");

                no = no.PadLeft(4, '0');
                List<int> prts = new List<int>();
                try
                {
                    prts = date.Split('-').Select(q => Convert.ToInt32(q)).ToList();
                }
                catch (Exception ex)
                {
                    return BadRequest("Incorrect Date");
                }

                if (prts.Count != 3)
                    return BadRequest("Incorrect Date");
                if (prts[0] < 1300)
                    return BadRequest("Incorrect Date (Year)");
                //if (prts[1] < 1 || prts[1]>12)
                //    return BadRequest("Incorrect Date (Month)");
                //if (prts[2] < 1 || prts[1] > 31)
                //    return BadRequest("Incorrect Date (Day)");

                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                var gd = (pc.ToDateTime(prts[0], prts[1], prts[2], 0, 0, 0, 0)).Date;
                var context = new ppa_entities();
                var flight = await context.ExpFlights.Where(q => q.DepartureDay == gd && q.FlightNo == no).FirstOrDefaultAsync();
                if (flight == null)
                    return BadRequest("Flight Not Found");
                var delay = (((DateTime)flight.Departure) - ((DateTime)flight.STD)).TotalMinutes;
                if (delay < 0)
                    delay = 0;
                var result = new
                {
                    flightId = flight.Id,
                    flightNo = flight.FlightNo,
                    date = flight.DepartureDay,
                    departure = flight.DepartureLocal,
                    arrival = flight.ArrivalLocal,
                    departureUTC = flight.Departure,
                    arrivalUTC = flight.Arrival,
                    status = flight.FlightStatus,
                    statusId = flight.FlightStatusId,
                    origin = flight.Origin,
                    destination = flight.Destination,
                    aircraftType = flight.AircraftType,
                    register = flight.Register,
                    isDelayed = delay > 0,
                    delay

                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "    Inner Exception:" + ex.InnerException.Message;
                return BadRequest(msg);
            }








        }

    }
}
