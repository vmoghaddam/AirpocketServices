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
using XAPI.Models;
using System.Net.Http.Headers;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;

namespace XAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        [Route("api/flt")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlt(DateTime dt, string origin, string destination, string no, string key)
        {
            if (key != "taban@1359A")
                return BadRequest("Not Authorized");
            var ctx = new PPAEntities();
            var _dt = dt.Date;
            var _de = _dt.AddDays(1);
            var query = from x in ctx.ViewLegTimes
                        where x.STDLocal >= _dt && x.STDLocal < _de
                        select x;

            if (!string.IsNullOrEmpty(origin))
                query = query.Where(q => q.FromAirportIATA == origin);

            if (!string.IsNullOrEmpty(destination))
                query = query.Where(q => q.ToAirportIATA == destination);

            if (!string.IsNullOrEmpty(no))
                query = query.Where(q => q.FlightNumber == no);

            var result = query.ToList().OrderBy(q => q.STD).Select(q => new
            {
                FlightId = q.ID,
                Date = ((DateTime)q.STDLocal).Date,
                FltNo = q.FlightNumber,
                STD = q.STDLocal,
                STA = q.STALocal,
                STDUtc = q.STD,
                STAUtc = q.STA,
                DateUtc = ((DateTime)q.STD).Date,
                Dep = q.FromAirportIATA,
                Arr = q.ToAirportIATA,
                Departure = q.DepartureLocal,
                Arrival = q.ArrivalLocal,
                DepartureUtc = q.Departure,
                ArrivalUtc = q.Arrival,
                q.Register,
                q.FlightStatus



            }).ToList();

            return Ok(result);

        }


        public class fltQry
        {
            public DateTime? date { get; set; }
            public string key { get; set; }
        }
        [Route("api/flts")]
        [AcceptVerbs("POST")]
        public IHttpActionResult GetFlts(/*DateTime dt, string key*/fltQry dto)
        {
            if (dto.date == null)
                return Ok("Date not found.");
            if (string.IsNullOrEmpty(dto.key))
                return Ok("Authorization key not found.");

            if (dto.key != "taban@1359A")
                return Ok("Authorization key is wrong.");

            var ctx = new PPAEntities();
            var _dt = ((DateTime)dto.date).Date;
            var _de = _dt.AddDays(1);
            var query = from x in ctx.ViewLegTimes
                        where x.STD >= _dt && x.STD < _de
                        select x;



            var result = query.ToList().OrderBy(q => q.STD).Select(q => new
            {
                FlightId = q.ID,
                Date = ((DateTime)q.STDLocal).Date,
                FltNo = q.FlightNumber,
                STD = q.STDLocal,
                STA = q.STALocal,
                STDUtc = q.STD,
                STAUtc = q.STA,
                DateUtc = ((DateTime)q.STD).Date,
                Dep = q.FromAirportIATA,
                Arr = q.ToAirportIATA,
                Departure = q.DepartureLocal,
                Arrival = q.ArrivalLocal,
                DepartureUtc = q.Departure,
                ArrivalUtc = q.Arrival,
                q.Register,
                q.FlightStatus



            }).ToList();

            return Ok(result);

        }


        [Route("api/flt/flown")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlownFlt(DateTime Start, DateTime Finish, string Pass)
        {
            if (Pass != "taban@1359A")
                return BadRequest("Not Authorized");
            var ctx = new PPAEntities();
            var _dt = Start.Date;
            var _de = Finish.Date.AddDays(1).Date;
            var query = from x in ctx.ViewLegTimes
                        where x.STALocal >= _dt && x.STALocal < _de //&& (x.FlightStatusID == 15)
                        select x;

            //    Baggage,PAXADL,PAXCHD,PAXINF,TotalPAX,Status
            //2022 - 05 - 14,CH 8756,THR,AWZ,M82,CBH,2022 - 05 - 14 10:30:00,2022 - 05 - 14 11:40:00,2022 - 05 - 14 14:00:00,2022 - 05 - 14 15:10:00,0,123,1,0,124,F

            var result = query.ToList().OrderBy(q => q.STD).Select(q => new
            {

                Date = ((DateTime)q.STDLocal).Date,
                FltNo = q.FlightNumber,
                DepStn = q.FromAirportIATA,
                ArrStn = q.ToAirportIATA,
                ACType = q.AircraftType,
                ACReg = q.Register,
                DepTimeLCL = q.DepartureLocal,
                ArrTimeLCL = q.ArrivalLocal,
                DepTime = q.Departure,
                ArrTime = q.Arrival,
                Baggage = q.BaggageCount,
                PAXADL = q.PaxAdult != null ? q.PaxAdult : 0,
                PAXCHD = q.PaxChild != null ? q.PaxChild : 0,
                PAXINF = q.PaxInfant != null ? q.PaxInfant : 0,
                TotalPAX = (q.PaxAdult != null ? q.PaxAdult : 0) + (q.PaxChild != null ? q.PaxChild : 0) + (q.PaxInfant != null ? q.PaxInfant : 0),
                
                Status=q.FlightStatus





            }).ToList();

            return Ok(result);

        }


        [Route("api/mail")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetMail()
        {

            var mailRepository = new MailRepository("outlook.office365.com", 993, true, "flightcard.varesh@outlook.com", "Atrina1359");
            var allEmails = mailRepository.GetAllMails();

            //foreach (var email in allEmails)
            //{
            //    Console.WriteLine(email);
            //}

            //Assert.IsTrue(allEmails.ToList().Any());
            return Ok(allEmails);

        }
        [Route("api/mail/{flt}/{dt}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetMailByFlight(string flt, string dt)
        {

            var mailRepository = new MailRepository("outlook.office365.com", 993, true, "flightcard.varesh@outlook.com", "Atrina1359");
            var allEmails = mailRepository.GetAllMailsByFlight(flt, dt);

            //foreach (var email in allEmails)
            //{
            //    Console.WriteLine(email);
            //}

            //Assert.IsTrue(allEmails.ToList().Any());
            return Ok(allEmails);

        }


        [Route("api/fc/{flt}/{dt}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightCardByFlight(string flt, string dt)
        {
            var ctx = new PPAEntities();
            var key = "FlightCard_" + dt + "_" + flt + ".pdf";
            var fc = ctx.FlightCards.Where(q => q.Key == key).OrderByDescending(q => q.DateCreate).FirstOrDefault();
            if (fc != null)
            {
                return Ok(new List<string>() { key });
            }
            else
            {
                var mailRepository = new MailRepository("outlook.office365.com", 993, true, "flightcard.varesh@outlook.com", "Atrina1359");
                var allEmails = mailRepository.GetAllMailsByFlight(flt, dt);

                //foreach (var email in allEmails)
                //{
                //    Console.WriteLine(email);
                //}

                //Assert.IsTrue(allEmails.ToList().Any());
                return Ok(allEmails);
            }


        }

        [Route("api/nira/chrs")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightCardByFlight(string flt, DateTime dt)
        {
            var _dt = dt.ToString("yyyy-MM-dd");
            var url = "http://iv.nirasoft.ir:882/NRSCWS.jsp?ModuleType=SP&ModuleName=CharterOfficesCapacity&DepartureDateFrom=" + _dt + "&DepartureDateTo=" + _dt + "&FlightNo=" + flt + "&OfficeUser=Thr003.airpocket&OfficePass=nira123";
            WebRequest request = WebRequest.Create(url);

            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse(); ;

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseFromServer);

            return Ok(result);

        }

        [Route("api/skyputer")]
        [AcceptVerbs("POST")]
        public  IHttpActionResult PostSkyputer(skyputer dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.key))
                    return Ok("Authorization key not found.");
                if (string.IsNullOrEmpty(dto.plan))
                    return Ok("Plan cannot be empty.");
                if (dto.key != "Skyputer@1359#")
                    return Ok("Authorization key is wrong.");

                if (dto.plan.Contains("FlyPersia"))
                {
                    var entity = new OFPSkyPuter()
                    {
                        OFP = dto.plan,
                        DateCreate = DateTime.Now,
                        UploadStatus = 0,


                    };
                    var ctx = new PPAEntities();
                    ctx.Database.CommandTimeout = 1000;
                    ctx.OFPSkyPuters.Add(entity);
                    ctx.SaveChanges();

                    //HttpClient client = new HttpClient();
                    //var values = new Dictionary<string, string>
                    //             {
                    //                  { "key", dto.key },
                    //                  { "plan", dto.plan }
                    //             };

                    //var content = new FormUrlEncodedContent(values);

                    //var response =  await client.PostAsync ("https://fleet.flypersia.aero/xpi/api/skyputer", content);

                    //var responseString = await response.Content.ReadAsStringAsync();
                    using (WebClient client = new WebClient())
                    {
                        var reqparm = new System.Collections.Specialized.NameValueCollection();
                        reqparm.Add("key", dto.key);
                        reqparm.Add("plan", dto.plan);
                        byte[] responsebytes = client.UploadValues("https://fleet.flypersia.aero/xpi/api/skyputer/flypersia", "POST", reqparm);
                        string responsebody = Encoding.UTF8.GetString(responsebytes);
                    }
                    return Ok(true);
                }
                else
                {
                    var entity = new OFPSkyPuter()
                    {
                        OFP = dto.plan,
                        DateCreate = DateTime.Now,
                        UploadStatus = 0,


                    };
                    var ctx = new PPAEntities();
                    ctx.Database.CommandTimeout = 1000;
                    ctx.OFPSkyPuters.Add(entity);
                    ctx.SaveChanges();
                    new Thread(async () =>
                    {
                        GetSkyputerImport(entity.Id);
                    }).Start();
                    return Ok(true);
                }


            }
            catch (Exception ex)
            {
                return Ok(true);
            }

        }


        [Route("api/skyputer/flypersia")]
        [AcceptVerbs("POST")]
        public IHttpActionResult PostSkyputerFlyPersia(skyputer dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.key))
                    return Ok("Authorization key not found.");
                if (string.IsNullOrEmpty(dto.plan))
                    return Ok("Plan cannot be empty.");
                if (dto.key != "Skyputer@1359#")
                    return Ok("Authorization key is wrong.");

                
               
                    var entity = new OFPSkyPuter()
                    {
                        OFP = dto.plan,
                        DateCreate = DateTime.Now,
                        UploadStatus = 0,


                    };
                    var ctx = new PPAEntities();
                    ctx.Database.CommandTimeout = 1000;
                    ctx.OFPSkyPuters.Add(entity);
                    ctx.SaveChanges();
                    new Thread(async () =>
                    {
                        GetSkyputerImport(entity.Id);
                    }).Start();
                    return Ok(true);
                


            }
            catch (Exception ex)
            {
                return Ok(true);
            }

        }

        [Route("api/php")]
        [AcceptVerbs("POST")]
        public IHttpActionResult PostPHP(skyputer dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.key))
                    return Ok("Authorization key not found.");
                if (string.IsNullOrEmpty(dto.fltno))
                    return Ok("Flight No. cannot be empty.");
                if (string.IsNullOrEmpty(dto.date))
                    return Ok("Flight Date cannot be empty.");
                if (string.IsNullOrEmpty(dto.plan))
                    return Ok("Plan cannot be empty.");
                if (dto.key != "Php@2022#")
                    return Ok("Authorization key is wrong.");
                var dtparts = dto.date.Split('-').Select(q => Convert.ToInt32(q)).ToList();
                var fltDate = new DateTime(dtparts[0], dtparts[1], dtparts[2]);
                var fltno = dto.fltno.ToUpper().Replace("TBN", "").Replace(" ", "");
                var entity = new OFPSkyPuter()
                {
                    OFP = dto.plan,
                    DateCreate = DateTime.Now,
                    UploadStatus = 0,
                    FlightDate = fltDate,
                    FlightNumber = fltno,


                };
                var ctx = new PPAEntities();
                ctx.Database.CommandTimeout = 1000;
                var flight = ctx.ViewLegTimes.Where(q => q.STDDay == fltDate && q.FlightNumber == fltno).FirstOrDefault();
                if (flight == null)
                    return Ok("Flight not found.");
                if (flight.FlightStatusID == 15 || flight.FlightStatusID == 3)
                    return Ok("Flight Status is not valid.");
                entity.FlightId = flight.ID;

                ctx.OFPSkyPuters.Add(entity);
                ctx.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += " INNER: " + ex.InnerException.Message;
                return Ok(msg);
            }

        }


        [Route("api/skyputer/import/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetSkyputerImport(int id)
        {
            var context = new PPAEntities();
            context.Database.CommandTimeout = 5000;
            var dto = context.OFPSkyPuters.Where(q => q.Id == id).FirstOrDefault();
            if (dto == null)
                return BadRequest("not found");
            try
            {
                List<string> props = new List<string>();

                // var ofpSky = context.OFPSkyPuters.FirstOrDefault();
                var rawText = dto.OFP;
                // var mpln = rawText.Split(new string[] { "mpln:|" }, StringSplitOptions.None).ToList()[1];
                var parts = rawText.Split(new string[] { "||" }, StringSplitOptions.None).ToList();

                var info = parts.FirstOrDefault(q => q.StartsWith("binfo:|")).Replace("binfo:|", "");
                var infoRows = info.Split(';').ToList();
                //binfo:|OPT=VARESH AIRLINE;FLN=VAR5820;DTE=6/24/2022 12:00:00 AM;ETD=02:35;REG=;MCI=78;FLL=330;DOW=43742
                var opt = infoRows.FirstOrDefault(q => q.StartsWith("OPT")).Split('=')[1];
                var fln = infoRows.FirstOrDefault(q => q.StartsWith("FLN")).Split('=')[1];
                var dte = infoRows.FirstOrDefault(q => q.StartsWith("DTE")).Split('=')[1];
                var etd = infoRows.FirstOrDefault(q => q.StartsWith("ETD")).Split('=')[1];
                var reg = infoRows.FirstOrDefault(q => q.StartsWith("REG")).Split('=')[1];
                var mci = infoRows.FirstOrDefault(q => q.StartsWith("MCI")).Split('=')[1];
                var fll = infoRows.FirstOrDefault(q => q.StartsWith("FLL")).Split('=')[1];
                var dow = infoRows.FirstOrDefault(q => q.StartsWith("DOW")).Split('=')[1];

                var flightDate = DateTime.Parse(dte);
                var no =fln.Contains(" ")? fln.Substring(4) : fln.Substring(3);

                var flight = context.ViewLegTimes.Where(q => q.STDDay == flightDate && q.FlightNumber == no).FirstOrDefault();
                var fltobj = context.FlightInformations.Where(q => q.ID == flight.ID).FirstOrDefault();
                var cplan = context.OFPImports.FirstOrDefault(q => q.FlightId == flight.ID);
                if (cplan != null)
                    context.OFPImports.Remove(cplan);

                var plan = new OFPImport()
                {
                    DateCreate = DateTime.Now,
                    DateFlight = flight.STDDay,
                    FileName = "",
                    FlightNo = flight.FlightNumber,
                    Origin = flight.FromAirportICAO,
                    Destination = flight.ToAirportICAO,
                    User = "airpocket",
                    Text = rawText,



                };
                if (!string.IsNullOrEmpty(dow))
                    plan.DOW = Convert.ToDecimal(dow);
                if (!string.IsNullOrEmpty(fll))
                    plan.FLL = Convert.ToDecimal(fll);
                if (!string.IsNullOrEmpty(mci))
                    plan.MCI = Convert.ToDecimal(mci);
                plan.Source = "SkyPuter";

                if (flight != null)
                    plan.FlightId = flight.ID;
                context.OFPImports.Add(plan);

                var mpln = parts.FirstOrDefault(q => q.StartsWith("mpln:|")).Replace("mpln:|", "");
                var mplnRows = mpln.Split('|').ToList();
                //WAP=OIKB;COR=N27° 13' 06" ,  E056;FRE= ;VIA=ASMU1A;ALT=CLB;MEA=0;GMR=131;DIS=0;TDS=0;WID=;TRK=;TMP=;TME=00:00:00.0000000;TTM=00:00:00.0000000;FRE=-200;FUS=200;TAS=361;GSP=0
                List<JObject> mplnpJson = new List<JObject>();
                var idx = 0;
                foreach (var r in mplnRows)
                {
                    var procStr = "";
                    var _r = r.Replace("=;", "= ;");
                    var prts = _r.Split(';');
                    foreach (var x in prts)
                    {
                        var str = x.Replace("\"", "^").Replace("'", "#");
                        var substr = str.Split('=')[0] + ":'" + str.Split('=')[1] + "'";

                        procStr += substr;
                        if (x != prts.Last())
                            procStr += ",";
                    }
                    procStr = "{" + procStr + "}";

                    var jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    var _key = ("mpln_WAP_" + jsonObj.GetValue("WAP").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    props.Add("prop_" + _key + "_eta_" + idx);
                    props.Add("prop_" + _key + "_ata_" + idx);
                    props.Add("prop_" + _key + "_rem_" + idx);
                    props.Add("prop_" + _key + "_usd_" + idx);
                    mplnpJson.Add(jsonObj);
                    idx++;

                }
                //   var _r0 ="{"+ mplnRows.First().Replace("=;", ":''").Replace("= ;", ":''").Replace("=", ":").Replace(";", ",")+"}";
                // var jsonObj = JsonConvert.DeserializeObject<JObject>(_r0);


                var apln1 = parts.FirstOrDefault(q => q.StartsWith("apln:|"));
                if (apln1 != null)
                {
                    apln1 = apln1.Replace("apln:|", "");
                    var apln1Rows = apln1.Split('|').ToList();
                    List<JObject> apln1Json = new List<JObject>();
                    idx = 0;
                    foreach (var r in apln1Rows)
                    {
                        var procStr = "";
                        var _r = r.Replace("=;", "= ;");
                        var prts = _r.Split(';');
                        foreach (var x in prts)
                        {
                            var str = x.Replace("\"", "^").Replace("'", "#");
                            var substr = str.Split('=')[0] + ":'" + str.Split('=')[1] + "'";

                            procStr += substr;
                            if (x != prts.Last())
                                procStr += ",";
                        }
                        procStr = "{" + procStr + "}";

                        var jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                        var _key = ("apln_WAP_" + jsonObj.GetValue("WAP").ToString()).Replace(" ", "").ToLower();
                        jsonObj.Add("_key", _key);
                        props.Add("prop_" + _key + "_eta_" + idx);
                        props.Add("prop_" + _key + "_ata_" + idx);
                        props.Add("prop_" + _key + "_rem_" + idx);
                        props.Add("prop_" + _key + "_usd_" + idx);
                        apln1Json.Add(jsonObj);
                        idx++;

                    }

                    plan.JAPlan1 = "[" + string.Join(",", apln1Json) + "]";
                }



                var cstbl = parts.FirstOrDefault(q => q.StartsWith("cstbl:|"));
                if (cstbl != null)
                {
                    cstbl = cstbl.Replace("cstbl:|", "");
                    var cstblRows = cstbl.Split('|').ToList();
                    List<JObject> cstblJson = new List<JObject>();
                    idx = 0;
                    foreach (var r in cstblRows)
                    {
                        var procStr = "";
                        var _r = r.Replace("=;", "= ;");
                        var prts = _r.Split(';');
                        foreach (var x in prts)
                        {
                            var str = x.Replace("\"", "^").Replace("'", "#");
                            var substr = str.Split('=')[0] + ":'" + str.Split('=')[1] + "'";

                            procStr += substr;
                            if (x != prts.Last())
                                procStr += ",";
                        }
                        procStr = "{" + procStr + "}";

                        var jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                        var _key = ("cstbl_ETN_" + jsonObj.GetValue("ETN").ToString()).Replace(" ", "").ToLower();
                        jsonObj.Add("_key", _key);
                        // props.Add("prop_" + _key + "_eta_" + idx);
                        // props.Add("prop_" + _key + "_ata_" + idx);
                        // props.Add("prop_" + _key + "_rem_" + idx);
                        // props.Add("prop_" + _key + "_usd_" + idx);
                        cstblJson.Add(jsonObj);
                        idx++;

                    }
                    plan.JCSTBL = "[" + string.Join(",", cstblJson) + "]";
                }

                var aldrf = parts.FirstOrDefault(q => q.StartsWith("aldrf:|"));
                if (aldrf != null)
                {
                    aldrf = aldrf.Replace("aldrf:|", "");
                    var aldrfRows = aldrf.Split('/').Where(q=>!string.IsNullOrEmpty(q)).ToList();
                    List<JObject> aldrfJson = new List<JObject>();
                    idx = 0;

                    foreach (var r in aldrfRows)
                    {
                        var procStr = "";
                        var _r = r.Replace("=;", "= ;");
                        var prts = _r.Split(new string[] { "  " }, StringSplitOptions.None).Where(q=>!string.IsNullOrEmpty(q)).ToList();
                      //  var prts2 = _r.Split(new string[] { " " }, StringSplitOptions.None);
                        //foreach (var x in prts)
                        //{
                        //    var str = x.Replace("\"", "^").Replace("'", "#");
                        //    var substr = str.Split('=')[0] + ":'" + str.Split('=')[1] + "'";

                        //    procStr += substr;
                        //    if (x != prts.Last())
                        //        procStr += ",";
                        //}
                        procStr += "FL:'"+prts[0].Replace(" ","").Replace("|","")+"'";
                        procStr += ",WIND:'" + prts[1].Replace(" ", "") + "'";
                        procStr += ",FUEL:'" + prts[2].Replace(" ", "") + "'";
                        procStr += ",T:'" + prts[3].Replace(" ", "") + "'";
                        procStr += ",SH1:'" + prts[4].Replace(" ", "") + "'";
                        procStr += ",SH2:'" + prts[5].Replace(" ", "") + "'";
                        procStr += ",DEV:'" + prts[6].Replace(" ", "") + "'";
                        procStr = "{" + procStr + "}";

                        var jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                        var _key = ("aldrf_FL_" + jsonObj.GetValue("FL").ToString()).Replace(" ", "").ToLower();
                        jsonObj.Add("_key", _key);

                        aldrfJson.Add(jsonObj);
                        idx++;

                    }
                    plan.JALDRF = "[" + string.Join(",", aldrfJson) + "]";
                }

                var wtdrf = parts.FirstOrDefault(q => q.StartsWith("wtdrf:|"));
                if (wtdrf != null)
                {
                    wtdrf = wtdrf.Replace("wtdrf:|", "");
                    var aldrfRows = wtdrf.Split(new string[] { "  " }, StringSplitOptions.None).Where(q => !string.IsNullOrEmpty(q)).ToList();
                    List<JObject> wtdrfJson = new List<JObject>();
                    idx = 0;

                    var procStr = "{IDX:'1',X:'-8',FUEL:'" + aldrfRows[1].Replace(" ", "")+"'}";
                    var jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    var _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);


                    procStr = "{IDX:'2',X:'-6',FUEL:'" + aldrfRows[3].Replace(" ", "") + "'}";
                    jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);

                    procStr = "{IDX:'3',X:'-4',FUEL:'" + aldrfRows[5].Replace(" ", "") + "'}";
                    jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);


                    procStr = "{IDX:'4',X:'-2',FUEL:'" + aldrfRows[7].Replace(" ", "") + "'}";
                    jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);

                    procStr = "{IDX:'5',X:'+2',FUEL:'" + aldrfRows[9].Replace(" ", "") + "'}";
                    jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);


                    procStr = "{IDX:'6',X:'+4',FUEL:'" + aldrfRows[11].Replace(" ", "") + "'}";
                    jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);

                    procStr = "{IDX:'7',X:'+6',FUEL:'" + aldrfRows[13].Replace(" ", "") + "'}";
                    jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);

                    procStr = "{IDX:'8',X:'+8',FUEL:'" + aldrfRows[15].Replace(" ", "") + "'}";
                    jsonObj = JsonConvert.DeserializeObject<JObject>(procStr);
                    _key = ("wtdrf_IDX_" + jsonObj.GetValue("IDX").ToString()).Replace(" ", "").ToLower();
                    jsonObj.Add("_key", _key);
                    wtdrfJson.Add(jsonObj);

                    plan.JWTDRF = "[" + string.Join(",", wtdrfJson) + "]";

                }


                var futbl = parts.FirstOrDefault(q => q.StartsWith("futbl:|")).Replace("futbl:|", "");
                var prmParts = futbl.Split('|');
                var fuel = new List<fuelPrm>();
                idx = 0;
                foreach (var x in prmParts)
                {
                    var _prts = x.Split(';');
                    //PRM=TRIP FUEL;TIM=17:26:00.00000;VAL=99960|
                    var prm = _prts[0].Split('=')[1];
                    var tim = _prts[1].Split('=')[1];
                    var val = _prts[2].Split('=')[1];
                    var _key = "fuel_" + (prm != "CONT[5%]" ? prm.Replace(" ", "").ToLower() : "cont05");
                    fuel.Add(new fuelPrm()
                    {
                        prm = prm,
                        time = tim,
                        value = val,
                        _key = _key,
                    });

                    if (prm == "TRIP FUEL")
                    {
                        plan.FPTripFuel = Convert.ToDecimal(val);
                        fltobj.FPTripFuel = plan.FPTripFuel;
                    }
                    if (prm == "TOF")
                    {

                        fltobj.FPFuel = Convert.ToDecimal(val);
                    }


                    props.Add("prop_" + _key);

                    idx++;
                }
                fuel.Add(new fuelPrm()
                {
                    prm = "REQ",
                    _key = "fuel_" + "req"
                }); ;
                props.Add("prop_fuel_" + "req");

                var other = new List<fuelPrm>();
                other.Add(new fuelPrm() { prm = "MACH", value = mci });
                props.Add("prop_mach");
                other.Add(new fuelPrm() { prm = "FL", value = fll });
                props.Add("prop_fl");
                other.Add(new fuelPrm() { prm = "DOW", value = dow });
                props.Add("prop_dow");
                other.Add(new fuelPrm() { prm = "PLD", value = "" });
                props.Add("prop_pld");
                other.Add(new fuelPrm() { prm = "EZFW", value = "" });
                props.Add("prop_ezfw");
                other.Add(new fuelPrm() { prm = "ETOW", value = "" });
                props.Add("prop_etow");
                other.Add(new fuelPrm() { prm = "ELDW", value = "" });
                props.Add("prop_eldw");
                other.Add(new fuelPrm() { prm = "CREW1", value = "2" });
                props.Add("prop_crew1");
                other.Add(new fuelPrm() { prm = "CREW2", value = "4" });
                props.Add("prop_crew2");
                other.Add(new fuelPrm() { prm = "CREW3", value = "" });
                props.Add("prop_crew3");

                other.Add(new fuelPrm() { prm = "PAX_ADULT", value = "" });
                props.Add("prop_pax_adult");
                other.Add(new fuelPrm() { prm = "PAX_CHILD", value = "" });
                props.Add("prop_pax_child");
                other.Add(new fuelPrm() { prm = "PAX_INFANT", value = "" });
                props.Add("prop_pax_infant");



                other.Add(new fuelPrm() { prm = "SOB", value = "" });
                props.Add("prop_sob");
                other.Add(new fuelPrm() { prm = "CLEARANCE", value = "" });
                props.Add("prop_clearance");

                other.Add(new fuelPrm() { prm = "ATIS1", value = "" });
                props.Add("prop_atis1");
                other.Add(new fuelPrm() { prm = "ATIS2", value = "" });
                props.Add("prop_atis2");
                other.Add(new fuelPrm() { prm = "ATIS3", value = "" });
                props.Add("prop_atis3");
                other.Add(new fuelPrm() { prm = "ATIS4", value = "" });
                props.Add("prop_atis4");

                other.Add(new fuelPrm() { prm = "RVSM_FLT_L", value = "" });
                props.Add("prop_rvsm_flt_l");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_STBY", value = "" });
                props.Add("prop_rvsm_flt_stby");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_R", value = "" });
                props.Add("prop_rvsm_flt_r");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_TIME", value = "" });
                props.Add("prop_rvsm_flt_time");


                other.Add(new fuelPrm() { prm = "RVSM_FLT_L2", value = "" });
                props.Add("prop_rvsm_flt_l2");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_STBY2", value = "" });
                props.Add("prop_rvsm_flt_stby2");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_R2", value = "" });
                props.Add("prop_rvsm_flt_r2");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_TIME2", value = "" });
                props.Add("prop_rvsm_flt_time2");

                other.Add(new fuelPrm() { prm = "RVSM_FLT_L3", value = "" });
                props.Add("prop_rvsm_flt_l3");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_STBY3", value = "" });
                props.Add("prop_rvsm_flt_stby3");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_R3", value = "" });
                props.Add("prop_rvsm_flt_r3");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_TIME3", value = "" });
                props.Add("prop_rvsm_flt_time3");


                other.Add(new fuelPrm() { prm = "RVSM_FLT_L4", value = "" });
                props.Add("prop_rvsm_flt_l4");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_STBY4", value = "" });
                props.Add("prop_rvsm_flt_stby4");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_R4", value = "" });
                props.Add("prop_rvsm_flt_r4");
                other.Add(new fuelPrm() { prm = "RVSM_FLT_TIME4", value = "" });
                props.Add("prop_rvsm_flt_time4");



                other.Add(new fuelPrm() { prm = "RVSM_GND_L", value = "" });
                props.Add("prop_rvsm_gnd_l");
                other.Add(new fuelPrm() { prm = "RVSM_GND_STBY", value = "" });
                props.Add("prop_rvsm_gnd_stby");
                other.Add(new fuelPrm() { prm = "RVSM_GND_R", value = "" });
                props.Add("prop_rvsm_gnd_r");
                other.Add(new fuelPrm() { prm = "RVSM_GND_TIME", value = "" });
                props.Add("prop_rvsm_gnd_time");
               
                
                other.Add(new fuelPrm() { prm = "RVSM_FLT_LVL", value = "" });
                props.Add("prop_rvsm_flt_lvl");

                other.Add(new fuelPrm() { prm = "RVSM_FLT_LVL2", value = "" });
                props.Add("prop_rvsm_flt_lvl2");


                other.Add(new fuelPrm() { prm = "RVSM_FLT_LVL3", value = "" });
                props.Add("prop_rvsm_flt_lvl3");

                other.Add(new fuelPrm() { prm = "RVSM_FLT_LVL4", value = "" });
                props.Add("prop_rvsm_flt_lvl4");



                other.Add(new fuelPrm() { prm = "FILLED_CPT", value = "" });
                props.Add("prop_filled_cpt");
                other.Add(new fuelPrm() { prm = "FILLED_FO", value = "" });
                props.Add("prop_filled_fo");

                //prop_offblock
                other.Add(new fuelPrm() { prm = "OFFBLOCK", value = "" });
                props.Add("prop_offblock");
                //prop_takeoff
                other.Add(new fuelPrm() { prm = "TAKEOFF", value = "" });
                props.Add("prop_takeoff");
                //prop_landing
                other.Add(new fuelPrm() { prm = "LANDING", value = "" });
                props.Add("prop_landing");
                //prop_onblock
                other.Add(new fuelPrm() { prm = "ONBLOCK", value = "" });
                props.Add("prop_onblock");
                //prop_fuel_onblock
                other.Add(new fuelPrm() { prm = "FUEL_ONBLOCK", value = "" });
                props.Add("prop_fuel_onblock");

                other.Add(new fuelPrm() { prm = "ARR_ATIS", value = "" });
                props.Add("prop_arr_atis");
                other.Add(new fuelPrm() { prm = "DEP_ATIS1", value = "" });
                props.Add("prop_dep_atis1");
                other.Add(new fuelPrm() { prm = "DEP_ATIS2", value = "" });
                props.Add("prop_dep_atis2");

                other.Add(new fuelPrm() { prm = "ARR_QNH", value = "" });
                props.Add("prop_arr_qnh");
                other.Add(new fuelPrm() { prm = "DEP_QNH1", value = "" });
                props.Add("prop_dep_qnh1");
                other.Add(new fuelPrm() { prm = "DEP_QNH2", value = "" });
                props.Add("prop_dep_qnh2");


                other.Add(new fuelPrm() { prm = "TO_V1", value = "" });
                props.Add("prop_to_v1");
                other.Add(new fuelPrm() { prm = "TO_VR", value = "" });
                props.Add("prop_to_vr");
                other.Add(new fuelPrm() { prm = "TO_V2", value = "" });
                props.Add("prop_to_v2");
                other.Add(new fuelPrm() { prm = "TO_CONF", value = "" });
                props.Add("prop_to_conf");
                other.Add(new fuelPrm() { prm = "TO_ASMD", value = "" });
                props.Add("prop_to_asmd");
                other.Add(new fuelPrm() { prm = "TO_RWY", value = "" });
                props.Add("prop_to_rwy");
                other.Add(new fuelPrm() { prm = "TO_COND", value = "" });
                props.Add("prop_to_cond");



                other.Add(new fuelPrm() { prm = "LND_STAR", value = "" });
                props.Add("prop_lnd_star");
                other.Add(new fuelPrm() { prm = "LND_APP", value = "" });
                props.Add("prop_lnd_app");
                other.Add(new fuelPrm() { prm = "LND_VREF", value = "" });
                props.Add("prop_lnd_vref");
                other.Add(new fuelPrm() { prm = "LND_CONF", value = "" });
                props.Add("prop_lnd_conf");
                other.Add(new fuelPrm() { prm = "LND_LDA", value = "" });
                props.Add("prop_lnd_lda");
                other.Add(new fuelPrm() { prm = "LND_RWY", value = "" });
                props.Add("prop_lnd_rwy");
                other.Add(new fuelPrm() { prm = "LND_COND", value = "" });
                props.Add("prop_lnd_cond");



                var dtupd = DateTime.UtcNow.ToString("yyyyMMddHHmm");


                foreach (var prop in props)
                    plan.OFPImportProps.Add(new OFPImportProp()
                    {
                        DateUpdate = dtupd,
                        PropName = prop,
                        PropValue = "",
                        User = "airpocket",

                    });
                var _fuel = JsonConvert.SerializeObject(fuel);
                plan.JFuel = _fuel; //"["+string.Join(",", fuel)+"]";
                plan.JPlan = "[" + string.Join(",", mplnpJson) + "]";








                plan.TextOutput = JsonConvert.SerializeObject(other);

                dto.DateUpload = DateTime.Now;
                dto.UploadStatus = 1;
                dto.UploadMessage = "OK";

                context.SaveChanges();





                return Ok(true);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message += "  INNER: " + ex.InnerException.Message;

                dto.DateUpload = DateTime.Now;
                dto.UploadStatus = -1;
                dto.UploadMessage = message;

                context.SaveChanges();

                return Ok("Not Uploaded");
            }






        }



        [Route("api/appleg/ofp/{flightId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetOPF(int flightId)
        {


            var context = new PPAEntities();
            var ofp = context.OFPImports.FirstOrDefault(q => q.FlightId == flightId);
            if (ofp == null)
                return Ok(new { Id = -1 });
            else
            {
                var dr = context.EFBDSPReleases.FirstOrDefault(q => q.FlightId == flightId);
                var _props = context.OFPImportProps.Where(q => q.OFPId == ofp.Id).ToList();
                if (dr != null && dr.MinFuelRequiredPilotReq != null)
                {
                    var fprop = _props.FirstOrDefault(q => q.PropName == "prop_reqfuel");
                    if (fprop != null)
                    {
                        fprop.PropValue = dr.MinFuelRequiredPilotReq.ToString();
                    }
                }
                var props = _props.Select(q =>
                  new
                  {
                      q.Id,
                      q.OFPId,
                      q.PropName,
                      q.PropType,
                      q.PropValue,
                      q.User,
                      q.DateUpdate,

                  }
                    ).ToList();

                return Ok(new
                {
                    ofp.Id,
                    ofp.FlightId,
                    ofp.TextOutput,
                    ofp.User,
                    ofp.DateCreate,
                    ofp.PIC,
                    ofp.PICId,
                    ofp.JLSignedBy,
                    ofp.JLDatePICApproved,
                    props

                });
            }



        }



        public class fuelPrm
        {
            public string prm { get; set; }
            public string time { get; set; }
            public string value { get; set; }

            public string _key { get; set; }
        }


        [Route("api/skyputer/get")]
        [AcceptVerbs("GET")]
        public IHttpActionResult PostSkyputer2(skyputer dto)
        {

            if (string.IsNullOrEmpty(dto.key))
                return Ok("Authorization key not found.");
            if (string.IsNullOrEmpty(dto.plan))
                return Ok("Plan cannot be empty.");
            if (dto.key != "Skyputer@1359#")
                return Ok("Authorization key is wrong.");
            var entity = new OFPSkyPuter()
            {
                OFP = dto.plan,

            };
            var ctx = new PPAEntities();
            ctx.OFPSkyPuters.Add(entity);
            ctx.SaveChanges();
            return Ok(true);


        }

        [Route("api/fdp/ext/get/{clr}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFMISASSIGN(int clr, DateTime from)
        {
            var upd = "'upd" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + "'";
            var dt = from.Date; //DateTime.Now.Date;
            var fmis_cnn_string = "Data Source=VA.FMIS.IR,2019;Initial Catalog=CrewVA;User ID=WinUsers;Password=Crew1018!)!*";
            var ap_cnn_string = "Data Source=65.21.14.236;Initial Catalog=ppa_varesh;User ID=Vahid;Password=Atrina1359@aA";
            //185.141.132.14
            //var ap_cnn_string = "Data Source=185.141.132.14;Initial Catalog=x_varesh;User ID=Vahid;Password=Atrina1359@aA";

            SqlConnection cnnAP = new SqlConnection(ap_cnn_string);
            cnnAP.Open();
            SqlConnection cnnFMIS = new SqlConnection(fmis_cnn_string);
            cnnFMIS.Open();
            //if (clr==0)
            {
                SqlCommand cmd1 = new SqlCommand("DELETE FROM FMISLEG", cnnAP);
                cmd1.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand("DELETE FROM FMISLEGASSIGN", cnnAP);
                cmd2.ExecuteNonQuery();



                var fmisSql = "SELECT   DateUTC ,FltNo ,DepStn ,ArrStn ,DepTime ,ArrTime ,DepTimeLCL ,ArrTimeLCL ,STD ,STA ,STC ,ACType ,ACReg ,Flt ,UpdateFlag ,ScheduleGroup ,TurnType ,RouteType ,LegDesc ,Change ,Importance ,LastUpdateTime ,LastUpdateScher ,Comment ,FltGroup ,StandardTime ,Holyday ,HolydayName ,NormalCorrectedTime ,ChangedCorrectedTime FROM dbo.Leg "
                             + " WHERE DateUTC='" + dt.ToString("yyyy-MM-dd") + "'";
                SqlDataAdapter da = new SqlDataAdapter(fmisSql, cnnFMIS);
                DataSet ds = new DataSet();
                da.FillError += new FillErrorEventHandler(FillError);
                da.Fill(ds);
                var tbl = ds.Tables[0];
                var columns = tbl.Columns;
                var rows = tbl.Select();

                SqlBulkCopy sqlbc = new SqlBulkCopy(cnnAP);
                sqlbc.BulkCopyTimeout = 6000;
                sqlbc.NotifyAfter = 100000;
                //sqlbc.BatchSize = 1;
                var xxxx = sqlbc.BatchSize;

                sqlbc.DestinationTableName = "FMISLEG";
                sqlbc.WriteToServer(rows);

                //update _xleg set [key]=cast(dateutc as varchar(500))+'_'+REPLACE(FltNo, 'VA ', '')+'_'+DepStn+'_'+ArrStn
                using (SqlCommand command = new SqlCommand("update FMISLEG set [key]=convert(varchar, cast(dateutc as date), 23)+'_'+FltNo+'_'+DepStn+'_'+ArrStn", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }
                //UPDATE T SET T.Id = TT.ROW_ID  FROM _xleg AS T INNER JOIN (SELECT ROW_NUMBER() OVER (ORDER BY cast(std as datetime)) AS ROW_ID  ,[key] FROM _xleg) AS TT ON T.[key] = TT.[key]
                using (SqlCommand command = new SqlCommand("UPDATE T SET T.Id = TT.ROW_ID  FROM FMISLEG AS T INNER JOIN (SELECT ROW_NUMBER() OVER (ORDER BY cast(std as datetime)) AS ROW_ID  ,[key] FROM _xleg) AS TT ON T.[key] = TT.[key]", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(" update FMISLEG set flightnumber = REPLACE(fltno, 'VA ', '')", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }


                using (SqlCommand command = new SqlCommand("update FMISLEG set depstn = 'GSM' where DepStn = 'QSM'", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }


                using (SqlCommand command = new SqlCommand("update FMISLEG set arrstn = 'GSM' where arrstn = 'QSM'", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }



                using (SqlCommand command = new SqlCommand("update FMISLEG set fromairportid = (select id from airport where iata = DepStn)", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }




                using (SqlCommand command = new SqlCommand("update FMISLEG set toairportid = (select id from airport where iata = ArrStn)", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }




                using (SqlCommand command = new SqlCommand("update FMISLEG set regid = (select id from Ac_MSN where REGISTER = acreg)", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }

                //update _xlegassign set initindex=cast(dbo.UDF_ExtractNumbers(pos) as int)

                //////////////////////////////////
                var fmisSql2 = "SELECT   Route ,Crew ,Pos ,Rank ,TurnType ,ScheduleGroup ,Scheduler ,DateUTC ,FltNo ,DepStn ,ArrStn ,DepTime ,ArrTime ,ACType ,ACReg ,Flt ,RouteType ,JobType ,DepTimeLCL ,ArrTimeLCL ,Change ,StandardTime ,Status ,Expr1 ,OffBlock ,OnBlock FROM dbo.LegAssign "
                            + " WHERE DateUTC='" + dt.ToString("yyyy-MM-dd") + "'";
                SqlDataAdapter da2 = new SqlDataAdapter(fmisSql2, cnnFMIS);
                DataSet ds2 = new DataSet();
                da2.FillError += new FillErrorEventHandler(FillError);
                da2.Fill(ds2);
                var tbl2 = ds2.Tables[0];
                var columns2 = tbl2.Columns;
                var rows2 = tbl2.Select();

                SqlBulkCopy sqlbc2 = new SqlBulkCopy(cnnAP);
                sqlbc2.BulkCopyTimeout = 6000;
                sqlbc2.NotifyAfter = 100000;
                //sqlbc.BatchSize = 1;


                sqlbc2.DestinationTableName = "FMISLEGASSIGN";
                sqlbc2.WriteToServer(rows2);


                using (SqlCommand command = new SqlCommand("update FMISLEGASSIGN set initindex=cast(dbo.UDF_ExtractNumbers(pos) as int)", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }
                //update FMISLEGASSIGN set crewid=(select id from viewcrew where viewcrew.code=crew)
                using (SqlCommand command = new SqlCommand("update FMISLEGASSIGN set crewid=(select id from viewcrew where viewcrew.code=crew)", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }

                var _cmd1 = "update FMISLEGASSIGN 	set HomeBase=(select baseairportid from viewcrew where viewcrew.id=crewid) "
            + ", schedulename = (select schedulename from viewcrew where viewcrew.id = crewid) "
            + ",jobgroupid = (select groupid from viewcrew where viewcrew.id = crewid) "
            + ",InitGroup = (select jobgroup from viewcrew where viewcrew.id = crewid) "
            + " where crewid is not null";
                using (SqlCommand command = new SqlCommand(_cmd1, cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }

                //update _xlegassign set initindex=2 where pos like 'AGFO%'
                using (SqlCommand command = new SqlCommand("update FMISLEGASSIGN set initindex=2 where pos like 'AGFO%'", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }

                var _cmd2 = "update FMISLEGASSIGN "
    + "		set initrank=( "
    + "		   CASE dbo.UDF_ExtractChars(pos) "
    + "	     WHEN 'IP' THEN 'IP' "
    + "	     when 'CPT' then 'P1' "
    + "			 when 'FA' then 'CCM' "
    + "			 when 'FP' then 'SCCM' "
    + "			 when 'IFP' then 'ISCCM' "
    + "			 when 'FO' then 'P2' "
    + "			 when 'OB' then 'OBS' "
    + "			 when 'OBS' then 'OBS' "
    + "			 when 'SAFETY' then 'SAFETY' "
    + "			 else 'OBS' "
    + "      END "

    + "		) "

    + "		where crewid is not null ";
                using (SqlCommand command = new SqlCommand(_cmd2, cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }
                //update _xlegassign set [key]=cast(dateutc as varchar(500))+'_'+REPLACE(FltNo, 'VA ', '')+'_'+DepStn+'_'+ArrStn
                using (SqlCommand command = new SqlCommand("update FMISLEGASSIGN set [key]=convert(varchar, cast(dateutc as date), 23)+'_'+FltNo+'_'+DepStn+'_'+ArrStn", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }

                //update _xlegassign set flightid = (select id from FlightInformation where departureremark =[key])
                using (SqlCommand command = new SqlCommand("update FMISLEGASSIGN set flightid = (select id from FlightInformation where ALT5 =[key])", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand("update FMISLEG set flightid = (select id from FlightInformation where ALT5 =[key])", cnnAP))
                {
                    var r1x = command.ExecuteNonQuery();
                }
                //////////////////////////////////////
            }
            var c1 = "select DISTINCT flt+'_'+convert(varchar, cast(dateutc as date), 23) from fmisleg where flt is not null and flt not in (select fdp from _xfdp)";
            List<string> newFLTs = new List<string>();
            using (SqlCommand command = new SqlCommand(c1, cnnAP))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        newFLTs.Add(reader.GetString(0));
                    }
                }
            }

            string insertNewFLT = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/fdpnew.txt"));
            foreach (var flt in newFLTs)
            {
                var qry = insertNewFLT.Replace("@flt", "'" + flt.Split('_')[0] + "'").Replace("@Date", "'" + flt.Split('_')[1] + "'").Replace("@upd", upd);
                using (SqlCommand command = new SqlCommand(qry, cnnAP))
                {
                    var r1y = command.ExecuteNonQuery();
                }
            }




            ///////////////////////////////////////
            var c3 = "SELECT  id from _XFDP where crewid is not null and FDP+'_'+cast(crewid as varchar(10)) not in (select flt+'_'+cast(crewid as varchar(10)) from fmislegassign where crewid is not null)";
            List<string> delFLTCrews = new List<string>();
            using (SqlCommand command = new SqlCommand(c3, cnnAP))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        delFLTCrews.Add(reader.GetInt32(0).ToString());
                    }
                }
            }
            if (delFLTCrews.Count > 0)
            {
                using (SqlCommand command = new SqlCommand("delete from _xfdp where id in (" + string.Join(",", delFLTCrews) + ")", cnnAP))
                {
                    var r1y = command.ExecuteNonQuery();
                }
            }
            ////////////////////////////////////////
            //select DISTINCT flt+'_'+cast(CrewId as varchar(5)) from fmislegassign where flt+'_'+cast(CrewId as varchar(5)) not in (select fdp+'_'+cast(CrewId as varchar(5)) from _xfdpitem where crewid is not null) and crewid is not null
            var c2 = "select DISTINCT flt+'_'+cast(CrewId as varchar(5))+'_'+convert(varchar, cast(dateutc as date), 23) from fmislegassign where flt+'_'+cast(CrewId as varchar(5))+'_'+convert(varchar, cast(dateutc as date), 23) not in (select fdp+'_'+cast(CrewId as varchar(5))+'_'+convert(varchar, cast(dateutc as date), 23) from _xfdp  where crewid is not null) and crewid is not null";
            List<string> newFLTCrews = new List<string>();
            using (SqlCommand command = new SqlCommand(c2, cnnAP))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        newFLTCrews.Add(reader.GetString(0));
                    }
                }
            }

            string insertNewFLTCrew = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/fdpcrewnew.txt"));
            foreach (var flt in newFLTCrews)
            {
                var qry = insertNewFLTCrew.Replace("@flt", "'" + flt.Split('_')[0] + "'").Replace("@Crew", "'" + flt.Split('_')[1] + "'").Replace("@Date", "'" + flt.Split('_')[2] + "'").Replace("@upd", upd);
                using (SqlCommand command = new SqlCommand(qry, cnnAP))
                {
                    var r1y = command.ExecuteNonQuery();
                }
            }

            /////////////////////////////////////
            string insertNewFLT_FDP = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/mainfdpnew.txt"));
            foreach (var flt in newFLTs)
            {
                var qry = insertNewFLT_FDP.Replace("@flt", "'" + flt.Split('_')[0] + "'").Replace("@upd", upd).Replace("@temp", "1");
                using (SqlCommand command = new SqlCommand(qry, cnnAP))
                {
                    var r1y = command.ExecuteNonQuery();
                }
            }

            foreach (var flt in newFLTs)
            {
                var qry = insertNewFLT_FDP.Replace("@flt", "'" + flt.Split('_')[0] + "'").Replace("@upd", upd).Replace("@temp", "0");
                using (SqlCommand command = new SqlCommand(qry, cnnAP))
                {
                    var r1y = command.ExecuteNonQuery();
                }
            }

            if (newFLTs.Count > 0)
            {
                var updfdpqry = "update FDP  set TemplateId = (select top 1 ID from fdp f where f.IsTemplate = 1 and f.remark2 = fdp.remark2) where fdp.IsTemplate = 0 and remark = " + upd;
                using (SqlCommand command = new SqlCommand(updfdpqry, cnnAP))
                {
                    var r1y = command.ExecuteNonQuery();
                }

                var fdpitemqry = "INSERT INTO dbo.FDPItem (  FDPId ,FlightId ,IsSector ,IsPositioning ,IsOff ,PositionId ,RosterPositionId ,remark) "
                              + " SELECT (select top 1 Id from fdp where upd = xfdpid)  ,flightid ,1  ,0  ,0  ,pos ,rosterindex ," + upd + " from _xfdpitem where upd = " + upd;
                using (SqlCommand command = new SqlCommand(fdpitemqry, cnnAP))
                {
                    command.CommandTimeout = 1000000;
                    var r1y = command.ExecuteNonQuery();
                }
            }



            /////////////////////////////////////////////
            return Ok(true);
        }

        [Route("api/flt/ext/get/{clr}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFMIS(int clr, DateTime from)
        {
            try
            {
                var dt = from.Date; //DateTime.Now.Date;

                //var ctx = new PPAEntities();
                //var fmis = new CrewVAEntities();
                //var query = from x in fmis.FlightInformations
                //            join 
                //            where x.DateUTC == dt
                //            select x;

                var fmis_cnn_string = "Data Source=VA.FMIS.IR,2019;Initial Catalog=CrewVA;User ID=WinUsers;Password=Crew1018!)!*";
                var ap_cnn_string = "Data Source=65.21.14.236;Initial Catalog=ppa_varesh;User ID=Vahid;Password=Atrina1359@aA";

                //var fmis_cnn_string = "Data Source=185.116.160.80;Initial Catalog=CrewCH;User ID=WinUsers;Password=Crew1018!)!*";
                //var ap_cnn_string = "Data Source=185.141.132.14;Initial Catalog=ppa_chb;User ID=chb;Password=Atrina1359@aA";



                SqlConnection cnnAP = new SqlConnection(ap_cnn_string);
                cnnAP.Open();
                if (clr == 1)
                {
                    using (SqlCommand command = new SqlCommand("delete from flightinformation where cast(std as date)='" + dt.ToString("yyyy-MM-dd") + "'", cnnAP))
                    {
                        command.CommandTimeout = 1000000;
                        var rr = command.ExecuteNonQuery();
                    }
                }



                SqlConnection cnnFMIS = new SqlConnection(fmis_cnn_string);
                cnnFMIS.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM FMISFLT", cnnAP);
                cmd.ExecuteNonQuery();


                // var fmisSql = "SELECT  DateUTC ,FltNo ,DepStn ,ArrStn ,ACType ,ACReg ,STD ,STA ,ATD ,ATA ,OffBlock ,OnBlock ,TakeOff ,OnRunway ,SaveTime ,PaxADL ,PaxCHD ,PaxINF ,TotalSeats ,OverPax ,FuelRemain ,FuelUpLift ,FuelDefuel ,FuelTotal ,FuelTaxi ,FuelTrip ,FuelUnit ,CargoWeight ,CargoPiece ,Baggage ,BagPiece ,ExtraBag ,ExtraBagPiece ,ExtraBagAmount ,CargoUnit ,FlightType ,FlightCharterer ,DelayReason ,Distance ,StationIncome ,CrewXML ,PaxXML ,DelayXML ,ExtraXML ,CargoXML ,MaintenanceXML ,Tag1 ,Tag2 ,Tag3 ,Parking ,PAXStation ,StationIncomeCurrency ,AlternateStation ,Status ,UpdateUser ,UpdateTime ,SavingTime ,Remark ,Male ,Female FROM dbo.FlightInformation "
                //              + " WHERE DateUTC='" + dt.ToString("yyyy-MM-dd") + "'";

                var fmisSql = "SELECT  DateUTC ,FltNo ,DepStn ,ArrStn ,ACType ,ACReg ,STD ,STA  FROM dbo.LEG "
                              + " WHERE DateUTC='" + dt.ToString("yyyy-MM-dd") + "'";
                SqlDataAdapter da = new SqlDataAdapter(fmisSql, cnnFMIS);
                DataSet ds = new DataSet();
                da.FillError += new FillErrorEventHandler(FillError);
                da.Fill(ds);
                var tbl = ds.Tables[0];
                var columns = tbl.Columns;
                var rows = tbl.Select();

                SqlBulkCopy sqlbc = new SqlBulkCopy(cnnAP);
                sqlbc.BulkCopyTimeout = 6000;
                sqlbc.NotifyAfter = 100000;
                //sqlbc.BatchSize = 1;
                var xxxx = sqlbc.BatchSize;

                sqlbc.DestinationTableName = "FMISFLT";
                sqlbc.WriteToServer(rows);

                var updkey = "update FMISFLT set [key]=convert(varchar, dateutc, 23)+'_'+FltNo+'_'+DepStn+'_'+ArrStn";
                SqlCommand upd1 = new SqlCommand(updkey, cnnAP);
                var r1 = upd1.ExecuteNonQuery();



                var updmvt = "update FMISFLT set OffBlock=STD,TakeOff=STD,OnBlock=STA,OnRunway=STA";
                SqlCommand updmvtcom = new SqlCommand(updmvt, cnnAP);
                var r1111 = updmvtcom.ExecuteNonQuery();





                cnnFMIS.Close();


                var newRecsTxt = "select [Key] from FMISFLT where [Key] not in (select isnull(alt5,'') from FlightInformation)";
                List<string> newKeys = new List<string>();
                using (SqlCommand command = new SqlCommand(newRecsTxt, cnnAP))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            newKeys.Add(reader.GetString(0));
                        }
                    }
                }

                if (newKeys.Count > 0)
                {
                    #region new

                    string insertNewCmd = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/insertnew.txt"));
                    insertNewCmd = insertNewCmd.Replace("#rem", dt.ToString("yyyy-MM-dd"));
                    insertNewCmd = insertNewCmd.Replace("#key", string.Join(",", newKeys.Select(q => "'" + q + "'").ToList()));
                    using (SqlCommand command = new SqlCommand(insertNewCmd, cnnAP))
                    {
                        var r1y = command.ExecuteNonQuery();
                    }
                    #endregion
                }

                //////////////////////////////////
                //////////////////////////////////
                var updRecsTxt = "select ID from ViewFMISFLT  where DateUTC='" + dt.ToString("yyyy-MM-dd") + "' and (STD<>STD1  or STA<>STA1 or Takeoff<>Takeoff1 or offblock<>offblock1 or onblock<>onblock1 or landing<>landing1 or reg<>reg1 or StatusId1<>statusid or depstn<>depstn1 or arrstn<>arrstn1)";
                List<string> updKeys = new List<string>();
                using (SqlCommand command = new SqlCommand(updRecsTxt, cnnAP))
                {
                    command.CommandTimeout = 1000000;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            updKeys.Add(reader.GetInt32(0).ToString());
                        }
                    }
                }
                if (updKeys.Count > 0)
                {
                    #region new

                    string insertNewCmd = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/update.txt"));
                    insertNewCmd = insertNewCmd.Replace("#ID", string.Join(",", updKeys.Select(q => q.ToString()).ToList()));

                    using (SqlCommand command = new SqlCommand(insertNewCmd, cnnAP))
                    {
                        command.CommandTimeout = 100000;
                        var r1x = command.ExecuteNonQuery();
                    }
                    #endregion
                }

                cnnAP.Close();

                //ctx.OFPSkyPuters.Add(entity);
                //ctx.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "  INNER: " + ex.InnerException.Message;
                return Ok(msg);
            }



        }

        [Route("api/flt/ext/get/status/{clr}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFMISStatus(int clr, DateTime from)
        {
            try
            {
                var dt = from.Date; //DateTime.Now.Date;

                //var ctx = new PPAEntities();
                //var fmis = new CrewVAEntities();
                //var query = from x in fmis.FlightInformations
                //            join 
                //            where x.DateUTC == dt
                //            select x;

                 var fmis_cnn_string = "Data Source=VA.FMIS.IR,2019;Initial Catalog=CrewVA;User ID=WinUsers;Password=Crew1018!)!*";
                //var fmis_cnn_string = "Data Source=65.21.100.132;Initial Catalog=VARESH;User ID=sa;Password=Atrina1359";
                var ap_cnn_string = "Data Source=65.21.14.236;Initial Catalog=ppa_varesh;User ID=Vahid;Password=Atrina1359@aA";

                //var fmis_cnn_string = "Data Source=185.116.160.80;Initial Catalog=CrewCH;User ID=WinUsers;Password=Crew1018!)!*";
                //var ap_cnn_string = "Data Source=185.141.132.14;Initial Catalog=ppa_chb;User ID=chb;Password=Atrina1359@aA";



                SqlConnection cnnAP = new SqlConnection(ap_cnn_string);
                cnnAP.Open();
                if (clr == 1)
                {
                    using (SqlCommand command = new SqlCommand("delete from flightinformation where cast(std as date)='" + dt.ToString("yyyy-MM-dd") + "'", cnnAP))
                    {
                        command.CommandTimeout = 1000000;
                        var rr = command.ExecuteNonQuery();
                    }
                }



                SqlConnection cnnFMIS = new SqlConnection(fmis_cnn_string);
                cnnFMIS.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM FMISFLT", cnnAP);
                cmd.ExecuteNonQuery();


                var fmisSql = "SELECT  DateUTC ,FltNo ,DepStn ,ArrStn ,ACType ,ACReg ,STD ,STA ,ATD ,ATA ,OffBlock ,OnBlock ,TakeOff ,OnRunway ,SaveTime ,PaxADL ,PaxCHD ,PaxINF ,TotalSeats ,OverPax ,FuelRemain ,FuelUpLift ,FuelDefuel ,FuelTotal ,FuelTaxi ,FuelTrip ,FuelUnit ,CargoWeight ,CargoPiece ,Baggage ,BagPiece ,ExtraBag ,ExtraBagPiece ,ExtraBagAmount ,CargoUnit ,FlightType ,FlightCharterer ,DelayReason ,Distance ,StationIncome ,CrewXML ,PaxXML ,DelayXML ,ExtraXML ,CargoXML ,MaintenanceXML ,Tag1 ,Tag2 ,Tag3 ,Parking ,PAXStation ,StationIncomeCurrency ,AlternateStation ,Status ,UpdateUser ,UpdateTime ,SavingTime ,Remark ,Male ,Female FROM dbo.FlightInformation "
                             + " WHERE DateUTC='" + dt.ToString("yyyy-MM-dd") + "'";

                // var fmisSql = "SELECT  DateUTC ,FltNo ,DepStn ,ArrStn ,ACType ,ACReg ,STD ,STA  FROM dbo.LEG "
                //              + " WHERE DateUTC='" + dt.ToString("yyyy-MM-dd") + "'";
                SqlDataAdapter da = new SqlDataAdapter(fmisSql, cnnFMIS);
                DataSet ds = new DataSet();
                da.FillError += new FillErrorEventHandler(FillError);
                da.Fill(ds);
                var tbl = ds.Tables[0];
                var columns = tbl.Columns;
                var rows = tbl.Select();

                SqlBulkCopy sqlbc = new SqlBulkCopy(cnnAP);
                sqlbc.BulkCopyTimeout = 6000;
                sqlbc.NotifyAfter = 100000;
                //sqlbc.BatchSize = 1;
                var xxxx = sqlbc.BatchSize;

                sqlbc.DestinationTableName = "FMISFLT";
                sqlbc.WriteToServer(rows);

                var updkey = "update FMISFLT set [key]=convert(varchar, dateutc, 23)+'_'+FltNo+'_'+DepStn+'_'+ArrStn";
                SqlCommand upd1 = new SqlCommand(updkey, cnnAP);
                var r1 = upd1.ExecuteNonQuery();



             //   var updmvt = "update FMISFLT set OffBlock=STD,TakeOff=STD,OnBlock=STA,OnRunway=STA";
             //   SqlCommand updmvtcom = new SqlCommand(updmvt, cnnAP);
             //   var r1111 = updmvtcom.ExecuteNonQuery();





                cnnFMIS.Close();


                var newRecsTxt = "select [Key] from FMISFLT where [Key] not in (select isnull(alt5,'') from FlightInformation)";
                List<string> newKeys = new List<string>();
                using (SqlCommand command = new SqlCommand(newRecsTxt, cnnAP))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            newKeys.Add(reader.GetString(0));
                        }
                    }
                }

                if (newKeys.Count > 0)
                {
                    #region new

                    string insertNewCmd = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/insertnew.txt"));
                    insertNewCmd = insertNewCmd.Replace("#rem", dt.ToString("yyyy-MM-dd"));
                    insertNewCmd = insertNewCmd.Replace("#key", string.Join(",", newKeys.Select(q => "'" + q + "'").ToList()));
                    using (SqlCommand command = new SqlCommand(insertNewCmd, cnnAP))
                    {
                        var r1y = command.ExecuteNonQuery();
                    }
                    #endregion
                }

                //////////////////////////////////
                //////////////////////////////////
                var updRecsTxt = "select ID from ViewFMISFLT  where DateUTC='" + dt.ToString("yyyy-MM-dd") + "' and (STD<>STD1  or STA<>STA1 or Takeoff<>Takeoff1 or offblock<>offblock1 or onblock<>onblock1 or landing<>landing1 or reg<>reg1 or StatusId1<>statusid or depstn<>depstn1 or arrstn<>arrstn1 or PaxAdult1<>PaxAdult or PaxChild1<>PaxChild or PaxInfant1<>PaxInfant)";
                List<string> updKeys = new List<string>();
                using (SqlCommand command = new SqlCommand(updRecsTxt, cnnAP))
                {
                    command.CommandTimeout = 1000000;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            updKeys.Add(reader.GetInt32(0).ToString());
                        }
                    }
                }
                if (updKeys.Count > 0)
                {
                    #region new

                    string insertNewCmd = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/update.txt"));
                    insertNewCmd = insertNewCmd.Replace("#ID", string.Join(",", updKeys.Select(q => q.ToString()).ToList()));

                    using (SqlCommand command = new SqlCommand(insertNewCmd, cnnAP))
                    {
                        command.CommandTimeout = 100000;
                        var r1x = command.ExecuteNonQuery();
                    }
                    #endregion
                }

                cnnAP.Close();

                //ctx.OFPSkyPuters.Add(entity);
                //ctx.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "  INNER: " + ex.InnerException.Message;
                return Ok(msg);
            }



        }



        protected void FillError(object sender, FillErrorEventArgs args)
        {
            //var str=
            var str = args.Errors.Message + " ## " + args.Values[0].ToString();
            //logError(str);
            args.Continue = true;

        }

        //[Route("api/skyputer/get")]
        //[AcceptVerbs("GET")]
        //public IHttpActionResult GetSkyputer(string key, string plan)
        //{

        //    if (string.IsNullOrEmpty( key))
        //        return BadRequest("Authorization key not found.");
        //    if (string.IsNullOrEmpty( plan))
        //        return BadRequest("Plan cannot be empty.");
        //    if ( key != "Skyputer@1359#")
        //        return BadRequest("Authorization key is wrong.");
        //    var entity = new OFPSkyPuter()
        //    {
        //        OFP =  plan,

        //    };
        //    var ctx = new PPAEntities();
        //    ctx.OFPSkyPuters.Add(entity);
        //    ctx.SaveChanges();
        //    return Ok(true);


        //}

        public class skyputer
        {
            public string plan { get; set; }
            public string fltno { get; set; }
            public string date { get; set; }

            public string key { get; set; }
        }


    }
}
