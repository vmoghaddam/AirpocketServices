using ApiReportFlight.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiReportFlight.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        //api/flight/daily
        [Route("api/flight/daily")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightsDaily(DateTime df, DateTime dt, string regs, string routes, string from, string to, string no
            , string status
            , string type2
            , string idx
            , string chr
            )
        {
            var cmd = "select * from viewflightdaily ";
            try
            {
                var context = new ppa_Entities();


                // var cmd = "select * from viewflightdaily ";
                string whr = "  (STDDayLocal>='" + df.ToString("yyyy-MM-dd") + "' and STDDayLocal<='" + dt.ToString("yyyy-MM-dd") + "')";

                if (!string.IsNullOrEmpty(status) && status != "-1")
                {
                    var _regs = status.Split('_').ToList();
                    var col = _regs.Select(q => "status=" + q).ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }
                if (!string.IsNullOrEmpty(type2) && type2 != "-1")
                {
                    var _regs = type2.Split('_').ToList();
                    var col = _regs.Select(q => "FlightType2=N'" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(idx) && idx != "-1")
                {
                    var _regs = idx.Split('_').ToList();
                    var col = _regs.Select(q => "FlightIndex=N'" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(chr) && chr != "-1")
                {
                    var _regs = chr.Split('_').ToList();
                    var col = _regs.Select(q => "ChrTitle=N'" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(regs) && regs != "-1")
                {
                    var _regs = regs.Split('_').ToList();
                    var col = _regs.Select(q => "Register='" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(from) && from != "-1")
                {
                    var _regs = from.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "FromAirportIATA='" + q + "'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(to) && to != "-1")
                {
                    var _regs = to.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "ToAirportIATA='" + q + "'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(no) && no != "-1")
                {
                    var _regs = no.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "FlightNumber='" + q + "'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(routes) && routes != "-1")
                {
                    var _regs = routes.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "Route like '%" + q + "%'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                cmd = cmd + " WHERE " + whr + " ORDER BY STD,Register";

                var flts = context.ViewFlightDailies
                            .SqlQuery(cmd)
                            .ToList<ViewFlightDaily>();

                //var result = await courseService.GetEmployeeCertificates(id);

                return Ok(flts);
            }
            catch (Exception ex)
            {
                return Ok(cmd);
            }

        }

        [Route("api/delay/items/{flt}")]
        public IHttpActionResult GetDelayItems(int flt)
        {
            var context = new ppa_Entities();
            var result = context.ViewFlightDelays.Where(q => q.FlightId == flt).OrderByDescending(q => q.Delay).ThenBy(q => q.Code).ToList();
            return Ok(result);
        }
        [Route("api/flight/delayed")]

        // [Authorize]
        public IHttpActionResult GetDelayedFlight(DateTime df, DateTime dt, string route = "", string regs = "", string types = "", string flts = "", string cats = "", int range = 1)
        {
            var context = new ppa_Entities();
            var _df = df.Date;
            var _dt = dt.Date;//.AddHours(24);
            var query = from x in context.ViewDelayedFlights
                        where x.STDDayLocal >= _df && x.STDDayLocal <= _dt
                        select x;
            ////if (!string.IsNullOrEmpty(cats))
            ////{
            ////    var cts = cats.Split('_').ToList();
            ////    query = query.Where(q => cts.Contains(q.MapTitle2));
            ////}
            if (!string.IsNullOrEmpty(route))
            {
                var rids = route.Split('_').ToList();
                query = query.Where(q => rids.Contains(q.Route));
            }



            if (!string.IsNullOrEmpty(regs))
            {
                var regids = regs.Split('_').Select(q => (Nullable<int>)Convert.ToInt32(q)).ToList();
                query = query.Where(q => regids.Contains(q.RegisterID));
            }

            if (!string.IsNullOrEmpty(types))
            {
                var typeids = types.Split('_').Select(q => (Nullable<int>)Convert.ToInt32(q)).ToList();
                query = query.Where(q => typeids.Contains(q.TypeId));
            }
            //malakh
            if (!string.IsNullOrEmpty(flts))
            {
                var fltids = flts.Split(',').Select(q => q.Trim().Replace(" ", "")).ToList();
                query = query.Where(q => fltids.Contains(q.FlightNumber));
            }

            switch (range)
            {
                case 1:

                    break;
                case 2:
                    query = query.Where(q => q.Delay <= 30);
                    break;
                case 3:
                    query = query.Where(q => q.Delay > 30);
                    break;
                case 4:
                    query = query.Where(q => q.Delay >= 31 && q.Delay <= 60);
                    break;
                case 5:
                    query = query.Where(q => q.Delay >= 61 && q.Delay <= 120);
                    break;
                case 6:
                    query = query.Where(q => q.Delay >= 121 && q.Delay <= 180);
                    break;
                case 7:
                    query = query.Where(q => q.Delay >= 181);
                    break;
                case 8:
                    query = query.Where(q => q.Delay <= 15);
                    break;
                default: break;
            }





            var result = query.OrderBy(q => q.STDDay).ThenBy(q => q.AircraftType).ThenBy(q => q.Register).ThenBy(q => q.STD).ToList();

            return Ok(result);
        }




        [Route("api/flight/daily/twoway")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightsDailyTwoWay(DateTime df, DateTime dt, string regs, string routes, string from, string to, string no
           , string status
           , string type2
           , string idx
           , string chr
            , int cnl
           )
        {
            var cmd = "select * from viewflightdaily ";
            try
            {
                var context = new ppa_Entities();


                // var cmd = "select * from viewflightdaily ";
                string whr = "  (STDDayLocal>='" + df.ToString("yyyy-MM-dd") + "' and STDDayLocal<='" + dt.ToString("yyyy-MM-dd") + "')";

                if (!string.IsNullOrEmpty(status) && status != "-1")
                {
                    var _regs = status.Split('_').ToList();
                    var col = _regs.Select(q => "status=" + q).ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }
                if (!string.IsNullOrEmpty(type2) && type2 != "-1")
                {
                    var _regs = type2.Split('_').ToList();
                    var col = _regs.Select(q => "FlightType2=N'" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(idx) && idx != "-1")
                {
                    var _regs = idx.Split('_').ToList();
                    var col = _regs.Select(q => "FlightIndex=N'" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(chr) && chr != "-1")
                {
                    var _regs = chr.Split('_').ToList();
                    var col = _regs.Select(q => "ChrTitle=N'" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(regs) && regs != "-1")
                {
                    var _regs = regs.Split('_').ToList();
                    var col = _regs.Select(q => "Register='" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(from) && from != "-1")
                {
                    var _regs = from.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "FromAirportIATA='" + q + "'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(to) && to != "-1")
                {
                    var _regs = to.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "ToAirportIATA='" + q + "'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(no) && no != "-1")
                {
                    var _regs = no.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "FlightNumber='" + q + "'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                if (!string.IsNullOrEmpty(routes) && routes != "-1")
                {
                    var _regs = routes.Split('_').ToList();
                    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "Route like '%" + q + "%'").ToList()) + ")";
                    whr += " AND " + _whr;
                }

                if (cnl == 0)
                    whr += " AND status<>4";

                cmd = cmd + " WHERE " + whr + " ORDER BY STD,Register";

                var flts = context.ViewFlightDailies
                            .SqlQuery(cmd)
                            .ToList<ViewFlightDaily>();

                var tempflts = flts.ToList();

                var grps = (from x in flts
                            group x by new { x.Register, x.RegisterID, x.STDDayLocal } into grp
                            select new
                            {
                                grp.Key.Register,
                                grp.Key.RegisterID,
                                grp.Key.STDDayLocal,
                                Items = grp.OrderBy(q => q.STD).ToList()



                            }).ToList();
                var output = new List<TwoWayResult>();

                foreach (var g in grps)
                {
                    var rowflts = g.Items.OrderBy(q => q.STD).ToList();
                    while (rowflts.Count > 0)
                    {
                        var flt = rowflts.First();
                        var rec = new TwoWayResult()
                        {
                            Register = g.Register,
                            RegisterID = g.RegisterID,
                            STDDayLocal = g.STDDayLocal,
                            FlightNumber = flt.FlightNumber,
                            STD = flt.STD,
                            STDLocal = flt.STD
                        };
                        output.Add(rec);

                        var xflt = rowflts.Where(q => q.FlightId != flt.ID && reverseRoute(q.Route) == flt.Route).FirstOrDefault();
                        if (xflt != null)
                        {
                            //var recx = new TwoWayResult()
                            //{
                            //    Register = g.Register,
                            //    RegisterID = g.RegisterID,
                            //    STDDayLocal = g.STDDayLocal,
                            //    FlightNumber = xflt.FlightNumber,
                            //};
                            //output.Add(recx);
                            rec.FlightNumber2 = xflt.FlightNumber;
                            rowflts.Remove(xflt);
                        }
                        rowflts.Remove(flt);

                    }
                }

                //var grouped = (from x in flts
                //              group x by new { x.Register, x.RegisterID, x.STDDayLocal, x.PDate, x.PMonth, x.PDayName, x.FlightType2, x.XRoute } into grp
                //              select new TwoWayResult()
                //              {
                //                  Register= grp.Key.Register,
                //                  RegisterID=grp.Key.RegisterID,
                //                  STDDayLocal=grp.Key.STDDayLocal,
                //                  PDate=grp.Key.PDate,
                //                  PMonth=grp.Key.PMonth,
                //                  PDayName=grp.Key.PDayName,
                //                  FlightType2=grp.Key.FlightType2,
                //                  XRoute=grp.Key.XRoute,
                //                  Items=grp.OrderBy(q=>q.STD).ToList()



                //              }).ToList();
                //foreach(var grp in grouped)
                //{
                //    if (grp.Items.Count > 1)
                //    {
                //        grp.Route = grp.XRoute + "-" + grp.XRoute.Split('-')[0];
                //    }
                //    grp.STD = grp.Items.First().STD;
                //    grp.STA = grp.Items.Last().STA;
                //    grp.BlockOff = grp.Items.First().BlockOff;
                //    grp.BlockOn = grp.Items.Last().BlockOn;
                //    grp.TakeOff = grp.Items.First().TakeOff;
                //    grp.Landing = grp.Items.Last().Landing;

                //    grp.STDLocal = grp.Items.First().STDLocal;
                //    grp.STALocal = grp.Items.Last().STALocal;
                //    grp.BlockOffLocal = grp.Items.First().BlockOffLocal;
                //    grp.BlockOnLocal = grp.Items.Last().BlockOnLocal;
                //    grp.TakeOffLocal = grp.Items.First().TakeOffLocal;
                //    grp.LandingLocal = grp.Items.Last().LandingLocal;

                //}

                var result = output.Select(q => new
                {
                    //legs=q.Items.Count(),
                    q.Register,
                    q.RegisterID,
                    q.STDDayLocal,
                    q.FlightNumber,
                    q.FlightNumber2,
                    //q.PDate,
                    //q.PMonth,
                    //q.PDayName,
                    //q.FlightType2,
                    //q.Route,
                    //q.XRoute,
                    q.STD,
                    //q.STA,
                    //q.BlockOff,
                    //q.BlockOn,
                    //q.TakeOff,
                    //q.Landing,
                    //q.STDLocal,
                    //q.STALocal,
                    //q.BlockOffLocal,
                    //q.BlockOnLocal,
                    //q.TakeOffLocal,
                    //q.LandingLocal,
                }).OrderBy(q => q.STDDayLocal).ThenBy(q => q.Register).ThenBy(q => q.STD).ToList();

                // var oneway = result.Where(q => q.legs == 1).ToList();
                //var twoway = result.Where(q => q.legs == 2).ToList();



                //var result = await courseService.GetEmployeeCertificates(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(cmd);
            }

        }


        [Route("api/flight/daily/station")]
        [AcceptVerbs("GET")]
        //public IHttpActionResult GetFlightsDailyTwoWay(DateTime df, DateTime dt, string regs, string routes, string from, string to, string no
        //  , string status
        //  , string type2
        //  , string idx
        //  , string chr
        //   , int cnl
        //  )
        public IHttpActionResult GetFlightsDailyStation(DateTime df, DateTime dt
            , string regs
              // , string routes
              , string from
            //, string to, string no
            , string status
            , string type2
             //, string idx
             //, string chr
             , int cnl
          )
        {
            var cmd = "select * from viewflightdaily ";
            try
            {
                var context = new ppa_Entities();


                // var cmd = "select * from viewflightdaily ";
                string whr = "  (STDDayLocal>='" + df.ToString("yyyy-MM-dd") + "' and STDDayLocal<='" + dt.ToString("yyyy-MM-dd") + "')";

                //if (!string.IsNullOrEmpty(status) && status != "-1")
                //{
                //    var _regs = status.Split('_').ToList();
                //    var col = _regs.Select(q => "status=" + q).ToList();
                //    var _whr = "(" + string.Join(" OR ", col) + ")";
                //    whr += " AND " + _whr;
                //}
                //if (!string.IsNullOrEmpty(type2) && type2 != "-1")
                //{
                //    var _regs = type2.Split('_').ToList();
                //    var col = _regs.Select(q => "FlightType2=N'" + q + "'").ToList();
                //    var _whr = "(" + string.Join(" OR ", col) + ")";
                //    whr += " AND " + _whr;
                //}

                //if (!string.IsNullOrEmpty(idx) && idx != "-1")
                //{
                //    var _regs = idx.Split('_').ToList();
                //    var col = _regs.Select(q => "FlightIndex=N'" + q + "'").ToList();
                //    var _whr = "(" + string.Join(" OR ", col) + ")";
                //    whr += " AND " + _whr;
                //}

                //if (!string.IsNullOrEmpty(chr) && chr != "-1")
                //{
                //    var _regs = chr.Split('_').ToList();
                //    var col = _regs.Select(q => "ChrTitle=N'" + q + "'").ToList();
                //    var _whr = "(" + string.Join(" OR ", col) + ")";
                //    whr += " AND " + _whr;
                //}

                if (!string.IsNullOrEmpty(regs) && regs != "-1")
                {
                    var _regs = regs.Split('_').ToList();
                    var col = _regs.Select(q => "Register='" + q + "'").ToList();
                    var _whr = "(" + string.Join(" OR ", col) + ")";
                    whr += " AND " + _whr;
                }

                //if (!string.IsNullOrEmpty(from) && from != "-1")
                //{
                //    var _regs = from.Split('_').ToList();
                //    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "FromAirportIATA='" + q + "'").ToList()) + ")";
                //    whr += " AND " + _whr;
                //}

                //if (!string.IsNullOrEmpty(to) && to != "-1")
                //{
                //    var _regs = to.Split('_').ToList();
                //    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "ToAirportIATA='" + q + "'").ToList()) + ")";
                //    whr += " AND " + _whr;
                //}

                //if (!string.IsNullOrEmpty(no) && no != "-1")
                //{
                //    var _regs = no.Split('_').ToList();
                //    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "FlightNumber='" + q + "'").ToList()) + ")";
                //    whr += " AND " + _whr;
                //}

                //if (!string.IsNullOrEmpty(routes) && routes != "-1")
                //{
                //    var _regs = routes.Split('_').ToList();
                //    var _whr = "(" + string.Join(" OR ", _regs.Select(q => "Route like '%" + q + "%'").ToList()) + ")";
                //    whr += " AND " + _whr;
                //}

                if (cnl == 0)
                    whr += " AND status<>4";

                cmd = cmd + " WHERE " + whr + " ORDER BY STD,Register";

                var flts = context.ViewFlightDailies
                            .SqlQuery(cmd)
                            .ToList<ViewFlightDaily>();

                var stations = flts.Select(q => q.FromAirportIATA).ToList().Concat(flts.Select(q => q.ToAirportIATA).ToList()).Distinct().ToList();
                var output2 = new List<TwoWayResult>();
                foreach (var stn in stations)
                {
                    var stnflights = flts.Where(q => q.FromAirportIATA == stn || q.ToAirportIATA == stn).ToList();
                    var regGroups = (from x in stnflights
                                     group x by new { x.Register, x.RegisterID, x.STDDayLocal } into grp
                                     select new
                                     {
                                         Station = stn,
                                         grp.Key.Register,
                                         grp.Key.RegisterID,
                                         grp.Key.STDDayLocal,
                                         Items = grp.OrderBy(q => q.STD).ToList()



                                     }).ToList();
                    foreach (var grp in regGroups)
                    {
                        var _flts = grp.Items.ToList();
                        while (_flts.Count() > 0)
                        {
                            var _flt = _flts.First();

                            if (_flt.FromAirportIATA == grp.Station)
                            {
                                //out
                                var rec = new TwoWayResult()
                                {
                                    Station = grp.Station,
                                    FlightDate = _flt.FlightDate,
                                    Date = _flt.Date,
                                    DateLocal = _flt.DateLocal,
                                    PDate = _flt.PDate,
                                    PDayName = _flt.PDayName,
                                    PMonth = _flt.PMonth,
                                    PMonthName = _flt.PMonthName,
                                    STDDay = _flt.STDDay,
                                    STDDayLocal = _flt.STDDayLocal,
                                    Register = grp.Register,
                                    RegisterID = grp.RegisterID,
                                    FromAirportIATA2 = _flt.FromAirportIATA,
                                    FromAirportICAO2 = _flt.FromAirportICAO,
                                    ToAirportIATA2 = _flt.ToAirportIATA,
                                    ToAirportICAO2 = _flt.ToAirportICAO,
                                    FlightNumber2 = _flt.FlightNumber,
                                    STD2 = _flt.STD,
                                    STA2 = _flt.STA,
                                    BlockOff2 = _flt.BlockOff,
                                    BlockOn2 = _flt.BlockOn,
                                    TakeOff2 = _flt.TakeOff,
                                    Landing2 = _flt.Landing,
                                    STDLocal2 = _flt.STDLocal,
                                    STALocal2 = _flt.STALocal,
                                    TakeOffLocal2 = _flt.TakeOffLocal,
                                    LandingLocal2 = _flt.LandingLocal,
                                    BlockOffLocal2 = _flt.BlockOffLocal,
                                    BlockOnLocal2 = _flt.BlockOnLocal,
                                    PaxAdult2 = _flt.PaxAdult,
                                    PaxChild2 = _flt.PaxChild,
                                    PaxInfant2 = _flt.PaxInfant,
                                    RevPax2 = _flt.RevPax,
                                    TotalPax2 = _flt.TotalPax,
                                    FlightStatus2 = _flt.FlightStatus,
                                    IsArrInt2 = _flt.IsArrInt,
                                    IsDepInt2 = _flt.IsDepInt,
                                    STDX = _flt.STD,
                                    RegFlightCount = grp.Items.Count(),
                                };

                                output2.Add(rec);
                                _flts.Remove(_flt);
                            }
                            else
                            {
                                //in
                                var rec = new TwoWayResult()
                                {
                                    Station = grp.Station,
                                    Register = grp.Register,
                                    RegisterID = grp.RegisterID,
                                    FromAirportIATA = _flt.FromAirportIATA,
                                    ToAirportIATA = _flt.ToAirportIATA,
                                    FromAirportICAO = _flt.FromAirportICAO,
                                    ToAirportICAO = _flt.ToAirportICAO,
                                    FlightNumber = _flt.FlightNumber,
                                    FlightDate = _flt.FlightDate,
                                    Date = _flt.Date,
                                    DateLocal = _flt.DateLocal,
                                    PDate = _flt.PDate,
                                    PDayName = _flt.PDayName,
                                    PMonth = _flt.PMonth,
                                    PMonthName = _flt.PMonthName,
                                    STDDay = _flt.STDDay,
                                    STDDayLocal = _flt.STDDayLocal,
                                    STD = _flt.STD,
                                    STA = _flt.STA,
                                    BlockOff = _flt.BlockOff,
                                    BlockOn = _flt.BlockOn,
                                    TakeOff = _flt.TakeOff,
                                    Landing = _flt.Landing,
                                    STDLocal = _flt.STDLocal,
                                    STALocal = _flt.STALocal,
                                    TakeOffLocal = _flt.TakeOffLocal,
                                    LandingLocal = _flt.LandingLocal,
                                    BlockOffLocal = _flt.BlockOffLocal,
                                    BlockOnLocal = _flt.BlockOnLocal,
                                    PaxAdult = _flt.PaxAdult,
                                    PaxChild = _flt.PaxChild,
                                    PaxInfant = _flt.PaxInfant,
                                    RevPax = _flt.RevPax,
                                    TotalPax = _flt.TotalPax,
                                    FlightStatus = _flt.FlightStatus,
                                    IsArrInt = _flt.IsArrInt,
                                    IsDepInt = _flt.IsDepInt,
                                    STDX = _flt.STD,
                                    RegFlightCount = grp.Items.Count(),

                                };
                                _flts.Remove(_flt);
                                if (_flts.Count() > 0)
                                {
                                    _flt = _flts.First();
                                    rec.FromAirportIATA2 = _flt.FromAirportIATA;
                                    rec.ToAirportIATA2 = _flt.ToAirportIATA;
                                    rec.ToAirportIATA2 = _flt.ToAirportIATA;
                                    rec.ToAirportICAO2 = _flt.ToAirportICAO;
                                    rec.FlightNumber2 = _flt.FlightNumber;
                                    rec.FromAirportICAO2 = _flt.FromAirportICAO;
                                    rec.STD2 = _flt.STD;
                                    rec.STA2 = _flt.STA;
                                    rec.BlockOff2 = _flt.BlockOff;
                                    rec.BlockOn2 = _flt.BlockOn;
                                    rec.TakeOff2 = _flt.TakeOff;
                                    rec.Landing2 = _flt.Landing;
                                    rec.STDLocal2 = _flt.STDLocal;
                                    rec.STALocal2 = _flt.STALocal;
                                    rec.TakeOffLocal2 = _flt.TakeOffLocal;
                                    rec.LandingLocal2 = _flt.LandingLocal;
                                    rec.BlockOffLocal2 = _flt.BlockOffLocal;
                                    rec.BlockOnLocal2 = _flt.BlockOnLocal;
                                    rec.PaxAdult2 = _flt.PaxAdult;
                                    rec.PaxChild2 = _flt.PaxChild;
                                    rec.PaxInfant2 = _flt.PaxInfant;
                                    rec.RevPax2 = _flt.RevPax;
                                    rec.TotalPax2 = _flt.TotalPax;
                                    rec.FlightStatus2 = _flt.FlightStatus;
                                    rec.IsArrInt2 = _flt.IsArrInt;
                                    rec.IsDepInt2 = _flt.IsDepInt;
                                    _flts.Remove(_flt);
                                }

                                output2.Add(rec);

                            }
                        }
                    }


                }

                foreach (var x in output2)
                {
                    x.FlightType2 = (x.IsDepInt == 1 || x.IsDepInt2 == 1 || x.IsArrInt == 1 || x.IsArrInt2 == 1) ? "INT" : "DOM";
                }

                if (from != "-1")
                {
                    var _regs = from.Split('_').ToList();
                    output2 = output2.Where(q => _regs.Contains(q.Station)).ToList();
                }
                if (!string.IsNullOrEmpty(type2) && type2 != "-1")
                {
                    var _regs = type2.Split('_').ToList();
                    output2 = output2.Where(q => _regs.Contains(q.FlightType2)).ToList();
                }
                //var grps = (from x in flts
                //            group x by new { x.Register, x.RegisterID, x.STDDayLocal } into grp
                //            select new
                //            {
                //                grp.Key.Register,
                //                grp.Key.RegisterID,
                //                grp.Key.STDDayLocal,
                //                Items = grp.OrderBy(q => q.STD).ToList()



                //            }).ToList();
                //var output = new List<TwoWayResult>();

                //foreach (var g in grps)
                //{
                //    var rowflts = g.Items.OrderBy(q => q.STD).ToList();
                //    while (rowflts.Count > 0)
                //    {
                //        var flt = rowflts.First();
                //        var rec = new TwoWayResult()
                //        {
                //            Register = g.Register,
                //            RegisterID = g.RegisterID,
                //            STDDayLocal = g.STDDayLocal,
                //            FlightNumber = flt.FlightNumber,
                //            STD = flt.STD,
                //            STDLocal = flt.STD
                //        };
                //        output.Add(rec);

                //        var xflt = rowflts.Where(q => q.FlightId != flt.ID && reverseRoute(q.Route) == flt.Route).FirstOrDefault();
                //        if (xflt != null)
                //        {
                //            //var recx = new TwoWayResult()
                //            //{
                //            //    Register = g.Register,
                //            //    RegisterID = g.RegisterID,
                //            //    STDDayLocal = g.STDDayLocal,
                //            //    FlightNumber = xflt.FlightNumber,
                //            //};
                //            //output.Add(recx);
                //            rec.FlightNumber2 = xflt.FlightNumber;
                //            rowflts.Remove(xflt);
                //        }
                //        rowflts.Remove(flt);

                //    }
                //}



                /////////////////////////
                /////////////////////////

                //var grouped = (from x in flts
                //              group x by new { x.Register, x.RegisterID, x.STDDayLocal, x.PDate, x.PMonth, x.PDayName, x.FlightType2, x.XRoute } into grp
                //              select new TwoWayResult()
                //              {
                //                  Register= grp.Key.Register,
                //                  RegisterID=grp.Key.RegisterID,
                //                  STDDayLocal=grp.Key.STDDayLocal,
                //                  PDate=grp.Key.PDate,
                //                  PMonth=grp.Key.PMonth,
                //                  PDayName=grp.Key.PDayName,
                //                  FlightType2=grp.Key.FlightType2,
                //                  XRoute=grp.Key.XRoute,
                //                  Items=grp.OrderBy(q=>q.STD).ToList()



                //              }).ToList();
                //foreach(var grp in grouped)
                //{
                //    if (grp.Items.Count > 1)
                //    {
                //        grp.Route = grp.XRoute + "-" + grp.XRoute.Split('-')[0];
                //    }
                //    grp.STD = grp.Items.First().STD;
                //    grp.STA = grp.Items.Last().STA;
                //    grp.BlockOff = grp.Items.First().BlockOff;
                //    grp.BlockOn = grp.Items.Last().BlockOn;
                //    grp.TakeOff = grp.Items.First().TakeOff;
                //    grp.Landing = grp.Items.Last().Landing;

                //    grp.STDLocal = grp.Items.First().STDLocal;
                //    grp.STALocal = grp.Items.Last().STALocal;
                //    grp.BlockOffLocal = grp.Items.First().BlockOffLocal;
                //    grp.BlockOnLocal = grp.Items.Last().BlockOnLocal;
                //    grp.TakeOffLocal = grp.Items.First().TakeOffLocal;
                //    grp.LandingLocal = grp.Items.Last().LandingLocal;

                //}

                var fc = (from x in output2 group x by new { x.Station } into grp select new { grp.Key.Station, cnt = grp.Count() }).ToList();
                foreach (var x in output2)
                {
                    var _fc = fc.FirstOrDefault(q => q.Station == x.Station);
                    x.FlightCount = _fc == null ? 0 : _fc.cnt;
                }


                var result = new
                {
                    flts = output2.OrderBy(q => q.STDDayLocal).ThenByDescending(q => q.FlightCount).ThenBy(q => q.Station).ThenBy(q => q.Register).ThenBy(q => q.STDX).ToList(),
                    stations = fc
                };
                /*.Select(q => new
            {
                //legs=q.Items.Count(),
                q.Register,
                q.RegisterID,
                q.STDDayLocal,
                q.FlightNumber,
                q.FromAirportIATA,
                q.ToAirportIATA,
                q.FlightNumber2,
                q.FromAirportIATA2,
                q.ToAirportIATA2,
                q.Station,
                //q.PDate,
                //q.PMonth,
                //q.PDayName,
                //q.FlightType2,
                //q.Route,
                //q.XRoute,
                 q.STDX,
                //q.STA,
                //q.BlockOff,
                //q.BlockOn,
                //q.TakeOff,
                //q.Landing,
                //q.STDLocal,
                //q.STALocal,
                //q.BlockOffLocal,
                //q.BlockOnLocal,
                //q.TakeOffLocal,
                //q.LandingLocal,
            })*/

                //.OrderBy(q => q.STDDayLocal).ThenBy(q => q.Register).ThenBy(q => q.STD).ToList();

                // var oneway = result.Where(q => q.legs == 1).ToList();
                //var twoway = result.Where(q => q.legs == 2).ToList();



                //var result = await courseService.GetEmployeeCertificates(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(cmd);
            }

        }

        public class TwoWayResult
        {
            public int ID { get; set; }
            public int FlightId { get; set; }
            public int FlightId2 { get; set; }
            public string Station { get; set; }
            public int FlightCount { get; set; }
            public int RegFlightCount { get; set; }
            public Nullable<int> FlightPlanId { get; set; }
            public Nullable<System.DateTime> STD { get; set; }
            public Nullable<System.DateTime> STD2 { get; set; }
            public Nullable<System.DateTime> STDX { get; set; }
            public Nullable<System.DateTime> STA { get; set; }
            public Nullable<System.DateTime> STA2 { get; set; }
            public Nullable<System.DateTime> STDLocal { get; set; }
            public Nullable<System.DateTime> STALocal { get; set; }
            public Nullable<System.DateTime> STDLocal2 { get; set; }
            public Nullable<System.DateTime> STALocal2 { get; set; }
            public Nullable<System.DateTime> Date { get; set; }
            public Nullable<System.DateTime> DateLocal { get; set; }
            public Nullable<int> FlightStatusID { get; set; }
            public Nullable<int> FlightStatusID2 { get; set; }
            public Nullable<int> RegisterID { get; set; }
            public Nullable<int> FlightTypeID { get; set; }
            public string AircraftType { get; set; }
            public Nullable<int> TypeId { get; set; }
            public string FlightNumber { get; set; }
            public string FlightNumber2 { get; set; }
            public Nullable<int> FromAirport { get; set; }
            public string FromAirportICAO { get; set; }
            public Nullable<int> ToAirport { get; set; }
            public string ToAirportICAO { get; set; }
            public Nullable<int> FromAirport2 { get; set; }
            public string FromAirportICAO2 { get; set; }
            public Nullable<int> ToAirport2 { get; set; }
            public string ToAirportICAO2 { get; set; }
            public Nullable<int> CustomerId { get; set; }
            public string FromAirportIATA { get; set; }
            public string FromAirportIATA2 { get; set; }
            public string ToAirportIATA { get; set; }
            public string ToAirportIATA2 { get; set; }
            public string Register { get; set; }
            public string FlightStatus { get; set; }
            public string FlightStatus2 { get; set; }
            public string ArrivalRemark { get; set; }
            public string DepartureRemark { get; set; }
            public Nullable<System.DateTime> STDDay { get; set; }
            public Nullable<System.DateTime> STDDayLocal { get; set; }
            public Nullable<System.DateTime> STADay { get; set; }
            public Nullable<int> DelayOffBlock { get; set; }
            public Nullable<int> DelayTakeoff { get; set; }
            public Nullable<System.DateTime> OSTA { get; set; }
            public Nullable<int> OToAirportId { get; set; }
            public string OToAirportIATA { get; set; }
            public Nullable<int> FPFlightHH { get; set; }
            public Nullable<int> FPFlightMM { get; set; }
            public Nullable<System.DateTime> Departure { get; set; }
            public Nullable<System.DateTime> Arrival { get; set; }
            public Nullable<System.DateTime> Departure2 { get; set; }
            public Nullable<System.DateTime> Arrival2 { get; set; }
            public Nullable<System.DateTime> BlockOff { get; set; }
            public Nullable<System.DateTime> BlockOn { get; set; }
            public Nullable<System.DateTime> TakeOff { get; set; }
            public Nullable<System.DateTime> Landing { get; set; }
            public Nullable<System.DateTime> BlockOff2 { get; set; }
            public Nullable<System.DateTime> BlockOn2 { get; set; }
            public Nullable<System.DateTime> TakeOff2 { get; set; }
            public Nullable<System.DateTime> Landing2 { get; set; }
            public Nullable<System.DateTime> BlockOffLocal { get; set; }
            public Nullable<System.DateTime> BlockOnLocal { get; set; }
            public Nullable<System.DateTime> TakeOffLocal { get; set; }
            public Nullable<System.DateTime> LandingLocal { get; set; }
            public Nullable<System.DateTime> DepartureLocal { get; set; }
            public Nullable<System.DateTime> ArrivalLocal { get; set; }
            public Nullable<System.DateTime> BlockOffLocal2 { get; set; }
            public Nullable<System.DateTime> BlockOnLocal2 { get; set; }
            public Nullable<System.DateTime> TakeOffLocal2 { get; set; }
            public Nullable<System.DateTime> LandingLocal2 { get; set; }
            public Nullable<System.DateTime> DepartureLocal2 { get; set; }
            public Nullable<System.DateTime> ArrivalLocal2 { get; set; }
            public Nullable<int> BlockTime { get; set; }
            public Nullable<int> ScheduledTime { get; set; }
            public Nullable<int> FlightTime { get; set; }
            public Nullable<int> status { get; set; }
            public Nullable<System.DateTime> JLOffBlock { get; set; }
            public Nullable<System.DateTime> JLOnBlock { get; set; }
            public Nullable<System.DateTime> JLTakeOff { get; set; }
            public Nullable<System.DateTime> JLLanding { get; set; }
            public Nullable<int> PFLR { get; set; }
            public int PaxChild { get; set; }
            public int PaxInfant { get; set; }
            public int PaxAdult { get; set; }
            public Nullable<int> RevPax { get; set; }
            public Nullable<int> TotalPax { get; set; }
            public int PaxChild2 { get; set; }
            public int PaxInfant2 { get; set; }
            public int PaxAdult2 { get; set; }
            public Nullable<int> RevPax2 { get; set; }
            public Nullable<int> TotalPax2 { get; set; }
            public Nullable<int> FuelUnitID { get; set; }
            public Nullable<decimal> FuelArrival { get; set; }
            public Nullable<decimal> FuelDeparture { get; set; }
            public Nullable<double> UpliftLtr { get; set; }
            public Nullable<double> UpliftLbs { get; set; }
            public Nullable<double> UpliftKg { get; set; }
            public Nullable<decimal> UsedFuel { get; set; }
            public Nullable<int> TotalSeat { get; set; }
            public int BaggageWeight { get; set; }
            public int CargoWeight { get; set; }
            public int BaggageWeight2 { get; set; }
            public int CargoWeight2 { get; set; }
            public Nullable<int> Freight { get; set; }
            public Nullable<double> BaggageWeightLbs { get; set; }
            public Nullable<double> BaggageWeightKg { get; set; }
            public Nullable<double> CargoWeightLbs { get; set; }
            public Nullable<double> CargoWeightKg { get; set; }
            public Nullable<double> FreightLbs { get; set; }
            public Nullable<double> FreightKg { get; set; }
            public Nullable<System.DateTime> FlightDate { get; set; }
            public Nullable<int> CargoCount { get; set; }
            public Nullable<int> BaggageCount { get; set; }
            public Nullable<int> CargoCount2 { get; set; }
            public Nullable<int> BaggageCount2 { get; set; }
            public Nullable<int> JLBlockTime { get; set; }
            public Nullable<int> JLFlightTime { get; set; }
            public Nullable<decimal> FPFuel { get; set; }
            public Nullable<decimal> FPTripFuel { get; set; }
            public Nullable<int> MaxWeightTO { get; set; }
            public Nullable<int> MaxWeightLND { get; set; }
            public string MaxWeighUnit { get; set; }
            public string ChrCode { get; set; }
            public string ChrTitle { get; set; }
            public Nullable<int> ChrCapacity { get; set; }
            public Nullable<int> ChrAdult { get; set; }
            public Nullable<int> ChrChild { get; set; }
            public Nullable<int> ChrInfant { get; set; }
            public Nullable<int> PMonth { get; set; }
            public string PMonthName { get; set; }
            public string PDayName { get; set; }
            public string FlightType2 { get; set; }
            public string FlightIndex { get; set; }
            public Nullable<int> AirlineSold { get; set; }
            public Nullable<int> CherterSold { get; set; }
            public Nullable<int> OverSeat { get; set; }
            public Nullable<int> EmptySeat { get; set; }
            public string DelayReason { get; set; }
            public Nullable<double> Distance { get; set; }
            public Nullable<double> StationIncome { get; set; }
            public string TotalRemark { get; set; }
            public string Route { get; set; }
            public string PDate { get; set; }
            public int IsDepInt { get; set; }
            public int IsArrInt { get; set; }
            public int IsDepInt2 { get; set; }
            public int IsArrInt2 { get; set; }
            public string XRoute { get; set; }
            public List<ViewFlightDaily> Items { get; set; }
        }

        public class _filter
        {
            public string FromAirport { get; set; }
            public string ToAirport { get; set; }
            public string Register { get; set; }
        }

        [Route("api/flight/daily/filters")]
        public IHttpActionResult GetFlightsDailyFilters(DateTime df, DateTime dt)
        {
            var context = new ppa_Entities();
            df = df.Date;
            dt = dt.Date;
            var qry = from x in context.ViewFlightDailies

                      where x.STDDay >= df && x.STDDay <= dt
                      select new _filter() { FromAirport = x.FromAirportIATA, ToAirport = x.ToAirportIATA, Register = x.Register };
            var ds = qry.ToList();
            var origins = qry.Select(q => q.FromAirport).Distinct().ToList();
            var destinations = qry.Select(q => q.ToAirport).Distinct().ToList();
            var registers = qry.Select(q => q.Register).Distinct().ToList();
            return Ok(new { origins, destinations, registers });
        }

        [Route("api/fuel")]
        public IHttpActionResult GetFuelOFP(DateTime df, DateTime dt)
        {
            var context = new ppa_Entities();
            df = df.Date;
            dt = dt.Date;
            var qry = from x in context.RptFuelOFPs

                      where x.STDDay >= df && x.STDDay <= dt
                      select x;
            var ds = qry.OrderBy(q => q.STDDay).ThenBy(q => q.Register).ThenBy(q => q.STD).ToList();

            return Ok(ds);
        }

        [Route("api/rvsm")]
        public IHttpActionResult GetFlightRvsm(DateTime df, DateTime dt)
        {
            var context = new ppa_Entities();
            df = df.Date;
            dt = dt.Date;
            var qry = from x in context.RptFlightRVSMs

                      where x.STDDay >= df && x.STDDay <= dt
                      select x;
            var ds = qry.OrderBy(q => q.STDDay).ThenBy(q => q.Register).ThenBy(q => q.STD).ToList();

            return Ok(ds);
        }

        [Route("api/formb/report")]

        // [Authorize]
        public IHttpActionResult GetFormBReport(int year, int qrt)
        {
            var context = new ppa_Entities();
            var query = (from x in context.ViewFormBs
                         where x.Year == year && x.Quarter == qrt
                         select x).ToList();




            return Ok(query);
        }

        [Route("api/formc/report")]

        // [Authorize]
        public IHttpActionResult GetFormCReport(int year, int month)
        {
            var context = new ppa_Entities();
            var query = (from x in context.ViewFormCs
                         where x.Year == year && x.Month == month
                         select x).ToList();




            return Ok(query);
        }

        string reverseRoute(string rt)
        {
            var prts = rt.Split('-');
            return prts[1] + "-" + prts[0];
        }
        int getRouteOrder(string r)
        {
            if (r.StartsWith("THR"))
                return 1;
            if (r.StartsWith("IKA"))
                return 2;
            if (r.StartsWith("SRY"))
                return 3;
            return 10;

        }
        [Route("api/citypair/report")]

        // [Authorize]
        public IHttpActionResult GetCityPairReport(int year, int month, int? dom = -1)
        {
            var context = new ppa_Entities();


            var query = from x in context.ViewFinMonthlyRoutes
                        where x.Year == year && x.Month == month
                        select x;

            if (dom == 1)
                query = query.Where(q => q.IsDom == true);
            if (dom == 0)
                query = query.Where(q => q.IsDom == false);

            var ds = query.ToList().OrderByDescending(q => q.Legs).ThenBy(q => getRouteOrder(q.Route)).ToList();
            var routes = ds.Select(q => q.Route).OrderBy(q => getRouteOrder(q)).ToList();
            var result = new List<ViewFinMonthlyRoute>();
            while (ds.Count > 0)
            {
                var data = ds.First();
                var rev = reverseRoute(data.Route);
                var data_rev = ds.Where(q => q.Route == rev).FirstOrDefault();
                result.Add(data);
                ds.Remove(data);
                if (data_rev != null && rev != data.Route)
                {
                    result.Add(data_rev);
                    ds.Remove(data_rev);
                }
            }
            /* while (routes.Count > 0)
             { 
                 var rt = routes.First(); 

                 var data = ds.Where(q => q.Route == rt).FirstOrDefault();

                 var rec = new ViewFinMonthlyRoute()
                 {
                     Adult = data.Adult,
                     BaggageWeight = data.BaggageWeight,
                     Route = data.Route,
                     CargoWeight = data.CargoWeight,
                     Child = data.Child,
                     Delay = data.Delay,
                     Freight = data.Freight,
                     FromAirportIATA = data.FromAirportIATA,
                     Infant = data.Infant,
                     IsDom = data.IsDom,
                     Legs = data.Legs,
                     Month = data.Month,
                     MonthName = data.MonthName,
                     ToAirportIATA = data.ToAirportIATA,
                     TotalPax = data.TotalPax,
                     TotalSeat = data.TotalSeat,
                     UpliftFuel = data.UpliftFuel,
                     UsedFuel = data.UsedFuel,
                     Year = data.Year,
                     YearMonth = data.YearMonth,
                     YearName = data.YearName,
                 };
                 var rev = reverseRoute(rt);
                 var data2 = ds.Where(q => q.Route == rev).FirstOrDefault();
                 if (data2 != null)
                 {
                     rec.Route = rec.Route +"-"+ (rec.Route.Split('-')[0]);
                     rec.Adult += data2.Adult;
                     rec.BaggageWeight += data2.BaggageWeight;
                     rec.CargoWeight += data2.CargoWeight;
                     rec.Child += data2.Child;
                     rec.Delay += data2.Delay;
                     rec.Freight += data2.Freight;
                     rec.Infant += data2.Infant;
                     rec.Legs += data2.Legs;
                     rec.TotalPax += data2.TotalPax;
                     rec.TotalSeat += data2.TotalSeat;
                     rec.UsedFuel += data2.UsedFuel;
                     rec.UpliftFuel += data2.UpliftFuel;
                 }

                 routes.Remove(rt);
                 routes.Remove(reverseRoute(rt));

                 result.Add(rec);

             }*/



            //var query = (from x in context.ViewFormCs
            //             where x.Year == year && x.Month == month
            //             select x).ToList();




            //return Ok(query);
            return Ok(result);
        }



        string toHHMM(int? mm)
        {
            if (mm == null || mm <= 0)
                return "00:00";
            // TimeSpan ts = TimeSpan.FromMinutes(Convert.ToDouble(mm));
            // var result = ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0');
            var hh = mm / 60;
            var min = mm % 60;
            var result = hh.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0');
            return result;

        }

        public class FlightTimeDto
        {
            public int? CrewId { get; set; }
            public string ScheduleName { get; set; }
            public string JobGroup { get; set; }
            public string JobGropCode { get; set; }
            public string Name { get; set; }
            public int GroupOrder { get; set; }
            public int Legs { get; set; }
            public int DH { get; set; }
            public int? FlightTime { get; set; }
            public int? BlockTime { get; set; }
            public int? JLFlightTime { get; set; }
            public int? JLBlockTime { get; set; }
            public int? FixTime { get; set; }
            public string PID { get; set; }
        }

        int getOrder(string group)
        {
            switch (group)
            {
                case "TRE":
                    return 1;
                case "TRI":
                    return 2;
                case "P1":
                    return 3;
                case "P2":
                    return 4;
                case "ISCCM":
                    return 5;
                case "SCCM":
                    return 6;
                case "CCM":
                    return 7;
                default:
                    return 1000;
            }
        }
        string GETUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        [Route("api/report/flights")]
        
        //nookp
        public IHttpActionResult GetFlightCockpit(DateTime? df, DateTime? dt, int? ip, int? cpt, int? status)
        {
            //nooz
            //this.context.Database.CommandTimeout = 160;
            var ctx = new ppa_Entities();
            df = df != null ? ((DateTime)df).Date : DateTime.MinValue.Date;
            dt = dt != null ? ((DateTime)dt).Date : DateTime.MaxValue.Date;
            var query = from x in ctx.ViewFlightCockpits
                            // where x.FlightStatusID != 1 && x.FlightStatusID != 4
                        select x;
            query = query.Where(q => q.STDDayLocal >= df && q.STDDayLocal <= dt);
            if (ip != null)
                query = query.Where(q => q.IPId == ip);
            if (cpt != null)
                query = query.Where(q => q.CaptainId == cpt);
            if (status != null)
            {
                //       { Id: 1, Title: 'Done' },
                //{ Id: 2, Title: 'Scheduled' },
                //{ Id: 3, Title: 'Canceled' },
                //{ Id: 4, Title: 'Starting' },
                // { Id: 5, Title: 'All' },
                List<int> sts = new List<int>();
                switch ((int)status)
                {
                    case 1:
                        sts.Add(15);
                        sts.Add(3);
                        query = query.Where(q => sts.Contains(q.FlightStatusID));
                        break;
                    case 2:
                        sts.Add(1);
                        query = query.Where(q => sts.Contains(q.FlightStatusID));
                        break;
                    case 3:
                        sts.Add(4);
                        query = query.Where(q => sts.Contains(q.FlightStatusID));
                        break;
                    case 4:
                        sts.Add(20);
                        sts.Add(21);
                        sts.Add(22);
                        sts.Add(4);
                        sts.Add(2);
                        sts.Add(23);
                        sts.Add(24);
                        sts.Add(25);
                        query = query.Where(q => sts.Contains(q.FlightStatusID));

                        break;
                    case 5:
                        break;
                    default:
                        break;
                }
            }
            var result = query.OrderBy(q => q.STD).ToList();

            // return result.OrderBy(q => q.STD);
            return Ok( result);

        }





        [Route("api/crew/flights/{grp}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCrewFlightTimes(string grp, DateTime df, DateTime dt)
        {


            //List<FlightTimeDto> external_list = new List<FlightTimeDto>();
            //try
            //{
            //    var ids = "4325_4287_4289";
            //    var dfStr = df.ToString("yyyy-MM-dd");
            //    var dtStr = dt.ToString("yyyy-MM-dd");
            //    var extStr = GETUrl("https://apireportflight.varesh.click/api/crew/flights/ids/" + ids + "?df=" + dfStr + "&dt=" + dtStr);
            //    external_list = JsonConvert.DeserializeObject<List<FlightTimeDto>>(extStr);
            //}
            //catch (Exception ex)
            //{

            //}

            var ctx = new ppa_Entities();
            var _df = df.Date;
            var _dt = dt.Date.AddDays(1);
            var _query = (
                           from x in ctx.ViewLegCrews
                           where x.STDLocal >= df && x.STDLocal < dt && x.FlightStatusID != 4
                           group x by new { x.CrewId, x.ScheduleName, x.JobGroup, x.JobGroupCode, x.Name ,x.PID} into _grp
                           select new FlightTimeDto()
                           {
                               CrewId = _grp.Key.CrewId,
                               ScheduleName = _grp.Key.ScheduleName,
                               JobGroup = _grp.Key.JobGroup,
                               JobGropCode = _grp.Key.JobGroupCode,
                               Name = _grp.Key.Name,
                               PID=_grp.Key.PID,

                               Legs = _grp.Where(q => q.IsPositioning == false).Count(),
                               DH = _grp.Where(q => q.IsPositioning == true).Count(),
                               FlightTime = _grp.Sum(q => q.FlightTime),
                               BlockTime = _grp.Sum(q => q.BlockTime),
                               JLFlightTime = _grp.Sum(q => q.JL_FlightTime),
                               JLBlockTime = _grp.Sum(q => q.JL_BlockTime),
                               FixTime = _grp.Sum(q => q.FixTime),
                           }
                          ).ToList();
            foreach (var x in _query)
                x.GroupOrder = getOrder(x.JobGroup);
           // if (external_list.Count > 0)
           //     _query = _query.Concat(external_list).ToList();

            _query = _query.OrderBy(q => q.GroupOrder).ThenByDescending(q => q.FixTime).ToList();
            return Ok(_query);
        }

        [Route("api/crew/flights/ids/{ids}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCrewFlightTimesByIds(string ids, DateTime df, DateTime dt)
        {

            var _ids = ids.Split('_').Select(q => (Nullable<int>)Convert.ToInt32(q)).ToList();

            var ctx = new ppa_Entities();
            var _df = df.Date;
            var _dt = dt.Date.AddDays(1);
            var _query = (
                           from x in ctx.ViewLegCrews
                           where x.STDLocal >= df && x.STDLocal < dt && x.FlightStatusID != 4 && _ids.Contains(x.CrewId)
                           group x by new { x.CrewId, x.ScheduleName, x.JobGroup, x.JobGroupCode, x.Name } into _grp
                           select new FlightTimeDto()
                           {
                               CrewId = _grp.Key.CrewId,
                               ScheduleName = _grp.Key.ScheduleName,
                               JobGroup = _grp.Key.JobGroup,
                               JobGropCode = _grp.Key.JobGroupCode,
                               Name = _grp.Key.Name,

                               Legs = _grp.Where(q => q.IsPositioning == false).Count(),
                               DH = _grp.Where(q => q.IsPositioning == true).Count(),
                               FlightTime = _grp.Sum(q => q.FlightTime),
                               BlockTime = _grp.Sum(q => q.BlockTime),
                               JLFlightTime = _grp.Sum(q => q.JL_FlightTime),
                               JLBlockTime = _grp.Sum(q => q.JL_BlockTime),
                               FixTime = _grp.Sum(q => q.FixTime),
                           }
                          ).ToList();
            foreach (var x in _query)
                x.GroupOrder = getOrder(x.JobGroup);
            _query = _query.OrderBy(q => q.GroupOrder).ThenByDescending(q => q.FixTime).ToList();
            return Ok(_query);
        }

        [Route("api/crew/flights/detail/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCrewFlightTimes(int id, DateTime df, DateTime dt)
        {
            var ctx = new ppa_Entities();
            var _df = df.Date;
            var _dt = dt.Date.AddDays(1);
            var _query = (
                           from x in ctx.ViewLegCrews
                           where x.STDLocal >= df && x.STDLocal < dt && x.FlightStatusID != 4 && x.CrewId == id
                           orderby x.STD
                           select x

                          ).ToList();

            List<ViewLegCrew> external_list = new List<ViewLegCrew>();
            var _ids = new List<int>() { 4235, 4287, 4289 };
            if (_ids.IndexOf(id) != -1)
            {
                try
                {
                    var ids = "4325_4287_4289";
                    var dfStr = df.ToString("yyyy-MM-dd");
                    var dtStr = dt.ToString("yyyy-MM-dd");
                    var extStr = GETUrl("https://apireportflight.varesh.click/api/crew/flights/detail/" + id + "?df=" + dfStr + "&dt=" + dtStr);
                    external_list = JsonConvert.DeserializeObject<List<ViewLegCrew>>(extStr);
                }
                catch (Exception ex)
                {

                }
                if (external_list.Count > 0)
                    _query = _query.Concat(external_list).OrderBy(q => q.STD).ToList();
            }


            return Ok(_query);
        }




    }
}
