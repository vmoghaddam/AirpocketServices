using ApiReportFlight.Models;
using System;
using System.Collections.Generic;
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

            var ds = query.ToList();
            var routes = ds.Select(q => q.Route).OrderBy(q => getRouteOrder(q)).ToList();
            var result = new List<ViewFinMonthlyRoute>();
            while (routes.Count > 0)
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

            }



            //var query = (from x in context.ViewFormCs
            //             where x.Year == year && x.Month == month
            //             select x).ToList();




            //return Ok(query);
            return Ok(result.OrderByDescending(q=>q.TotalPax).ToList());
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
        [Route("api/crew/flights/{grp}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetCrewFlightTimes(string grp, DateTime df, DateTime dt)
        {
            var ctx = new ppa_Entities();
            var _df = df.Date;
            var _dt = dt.Date.AddDays(1);
            var _query = (
                           from x in ctx.ViewLegCrews
                           where x.STDLocal >= df && x.STDLocal < dt && x.FlightStatusID != 4
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

            return Ok(_query);
        }



    }
}
