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
using ApiXLS.Models;
using System.Net.Http.Headers;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;
using Spire.Xls;

namespace ApiXLS.Controllers
{
    public class DispatchController : ApiController
    {
        [Route("api/xls/dispatch/daily/shift")]
        [AcceptVerbs("GET")]
        public HttpResponseMessage GetIdeaUniqueSync(DateTime dt)
        {
            
                var ctx = new Models.dbEntities();
                var legs = ctx.AppLegs.Where(q =>q.STDDayLocal!=null && q.STDDayLocal == dt && q.FlightStatusID != 4).ToList();
                var pics = legs.Select(q => q.PICId).ToList();
                var piclegs = ctx.AppCrewFlights.Where(q => q.STDDayLocal == dt && pics.Contains(q.CrewId)).ToList();
                var piclegsIds = piclegs.Select(q => q.FlightId).ToList();
                var applegcrew = ctx.AppCrewFlights.Where(q => piclegsIds.Contains(q.FlightId)).ToList();
                var fdpsQry = from leg in piclegs

                              group leg by new { leg.FDPId } into grp
                              select new ShiftData()
                              {
                                  FDPId = grp.Key.FDPId,
                                  STD = grp.OrderBy(q => q.STD).Select(q => q.STD).First(),
                                  REG = grp.First().Register,
                                  Flights = grp.Select(q => new ShiftFlight()
                                  {
                                      FlightId = q.FlightId,
                                      FlightNumber = q.FlightNumber,
                                      Dep = q.FromAirportIATA2,
                                      Dest = q.ToAirportIATA2,
                                      STD = q.STD,
                                      STDLocal = q.STDLocal,
                                      STA = q.STA,
                                      STALocal = q.STALocal
                                  }).Distinct().OrderBy(q => q.STD).ToList(),
                                  FlightIds = grp.Select(q => q.FlightId).Distinct().ToList()
                                  //Cockpit=grp.Where(q=>q.JobGroupCode.StartsWith("00101")).OrderBy(q=>q.GroupOrder).Select(q=>q.ScheduleName).Distinct().ToList(),
                                  // Cabin = grp.Where(q => q.JobGroupCode.StartsWith("00102")).OrderBy(q => q.GroupOrder).Select(q => q.ScheduleName).Distinct().ToList(),
                              };
                var fdps = fdpsQry.OrderBy(q => q.STD).ToList();
                foreach (var x in fdps)
                {
                    var cabin = applegcrew.Where(q => x.FlightIds.Contains(q.FlightId) && q.JobGroupCode.StartsWith("00102")).OrderBy(q => q.GroupOrder).Select(q => q.ScheduleName).Distinct().ToList();
                    var cockpit = applegcrew.Where(q => x.FlightIds.Contains(q.FlightId) && q.JobGroupCode.StartsWith("00101")).OrderBy(q => q.GroupOrder).Select(q => q.ScheduleName).Distinct().ToList();
                    x.Cabin = cabin;
                    x.Cockpit = cockpit;
                }

                var _sheets = new List<List<ShiftData>>();
                int fdpCount = 0;
                var _sheet = new List<ShiftData>();
                _sheets.Add(_sheet);
                foreach (var fdp in fdps)
                {
                    _sheet.Add(fdp);
                    if (_sheet.Count == 4)
                    {
                        _sheet = new List<ShiftData>();
                        _sheets.Add(_sheet);
                    }

                }
                string LData = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiIHN0YW5kYWxvbmU9InllcyI/Pgo8TGljZW5zZSBLZXk9ImNyOERaN1hKMkR5MUs2UUJBTkRPSVRLdlpjTzZkelVod2lsSHBnVlluQ3k0cXlHV2V6TFZubFJGeFAxNU1mSWZnUmdNWm1XaEdOQWRFNFRqZWZnQ1ovbFR2b1BkSXRIbDZXdDVBNWk1TVhFbnFkQnVPMUthRnovRFFzYUdWTGhzdjlySG1ybnRxSElFRGxJeGRxYUpNcGtLb0Frd1A3d1N6T01KMVkrbUNmVTVVRmV6REwvTjd1enJ4M0Y0d2I1SGErd0E2VFQ5VFJ3SzAzejlFS01aRmwzU1lSL3o0YVU3TE0wZFNYWTlqU0ZKZ2dqZlZzRFVLaUJyVm5td1ljaXVyOUVrYmw5Q3RaWTAzdG1yZm01QlplKzZnaHRFTm4wb2gzMzh0WlJleWpjcjc0QWs3MWhnWWtuTE9CQzE1VllmalhzcXVBVW13MlI2TWNWMlBPT2JyY1RSYlhBZ3pvUWJPeWQ4U2JFWmN3aE43NktQd1dzUVFTMUowdGlZSFVLeE9tMnQ0ZkJWMGhQVmhhOUI4Y0swNHFKUVp0MDBaMWNKRGEwd2I4VWx6RWs5QkhVVzJlbk9mVDE0UnlIQ2krWUdlbVBLY2RDUXJoMXpyWVRGN0ltb0x4N3h1NGV2RFRZc2xzV0JrbFFJb3g4NnJWckVVa1N0dXErQUNTWS9xVTM5L1Zhd3Y5S0FmUjVUZUVicGt3RGhTYjBOQkFqVDhBeXRsRFZkR2ZpZzBxS0czVllpVHBYRnc1cHRMVmgrYmtkK2RnN3Z4dHZyNDVaVVdKZXlyekdOR0g3YUZZZDZwLzJNRy9YSlRsR3ovU05RbzJDUExraU83SlhuOU5HZXhaN3BIbTBkZ3pNWmJHRVhxVmR2bG04MTJhL1hMMVNxeEdVWStvNVpsVUM3WTV4Z2dhRCtGZVA5enpoeUpxSUVwcDk3My9ScTRteG1wQWZMcVNzTzJSeHlTcStpdjFDc3AwQ3JvMDc4OEhybDFteWt4dVQweWRSWVpDNkRTeDhNMi9MWTNkOXNud3U3NkFmYjVDOVF1ZE9Zc0wzREh2aGZncmNVSWUvcUhmVFo5QWF6Y3pUanlyM2RPQkFjczBLZk12Y0xVUzRSeHZDdW1NNDVyNDJnMXJ3UGluN2JBcmYvZnNMTzZtS3g0WWRoSURNWlF6V3RjbkhFSTF5TXJ6aU9pdXhMdE8xalRBV25uU2VLVDJ0cXI3Tm42Qmg5TURHNjZZK2lJaW4xV05TUCtMdDFYdXRkajNKTyt4b1FNUVB5ZFpoZkJYZXpVMEhRMnd0eEdwdzRNczRTMTVJbFg1TEdXR3dXeUdYTWNjVWd3b1RDeFRGYmgyZFo0Vkg3OVZHTEVFR1JRWEZrNTRBdlFLdFBpdUcxY0w4RFo3WEoyRHkxSzZUUWVORE9YeFl2NFNveitCMHNBS0VwTVRrNCtTYWpYNksrSjlUOFhZVXRTOE8wWWZGUFZqZkhIYTZORWQyODdVcUlqMnJnQlF1bjVDV3hCczFHUm5BYmd1Z3MyL2ZQakcwZmdQemdSYzR5Q3ZObFg4V2pKUnloc3U5VFRKTjd1R3NOdnprU2IyZWlyQmhEaG1vQ0Jqa0wyYnMzT3I2d2pnNnBUNVpmNGhEdDF0STBJNXo1aytxQXVSZnRhd1lmamhXYmpMS0xKOTlUVk1kRDZaTCtTenNtQkNWN05lYm96V0RUTWgrRnJPT292R09ZbUk1bWp4Smd1MVRXNnI1V0JUK2oxSjBFNmJIb2tEMWo0Wm1DWUQreVBPUW1PMm1yUTNGdC9jVmZwQWlJdzliRkgwZ1FIbXQ4QnNuZnQ2MVV3c1h6cSs2akNvY1hOOUMvRXZPblhTczZuVlNGSkVBL3l1QmNIazZxOWdqanBnRG1NTEcrNlpxR1VjRWMzZEp2THpuK3pNT0p3TDI4WUQxN3BLSXBUNnd6WFBFVFJwWS9qNHhoMkQvaFhJRVNHcTk1eTVmZE9MNmx1QT09IiBWZXJzaW9uPSI5LjkiPgogICAgPFR5cGU+UnVudGltZTwvVHlwZT4KICAgIDxVc2VybmFtZT5Vc2VyTmFtZTwvVXNlcm5hbWU+CiAgICA8RW1haWw+ZU1haWxAaG9zdC5jb208L0VtYWlsPgogICAgPE9yZ2FuaXphdGlvbj5Pcmdhbml6YXRpb248L09yZ2FuaXphdGlvbj4KICAgIDxMaWNlbnNlZERhdGU+MjAxNi0wMS0wMVQxMjowMDowMFo8L0xpY2Vuc2VkRGF0ZT4KICAgIDxFeHBpcmVkRGF0ZT4yMDk5LTEyLTMxVDEyOjAwOjAwWjwvRXhwaXJlZERhdGU+CiAgICA8UHJvZHVjdHM+CiAgICAgICAgPFByb2R1Y3Q+CiAgICAgICAgICAgIDxOYW1lPlNwaXJlLk9mZmljZSBQbGF0aW51bTwvTmFtZT4KICAgICAgICAgICAgPFZlcnNpb24+OS45OTwvVmVyc2lvbj4KICAgICAgICAgICAgPFN1YnNjcmlwdGlvbj4KICAgICAgICAgICAgICAgIDxOdW1iZXJPZlBlcm1pdHRlZERldmVsb3Blcj45OTk5OTwvTnVtYmVyT2ZQZXJtaXR0ZWREZXZlbG9wZXI+CiAgICAgICAgICAgICAgICA8TnVtYmVyT2ZQZXJtaXR0ZWRTaXRlPjk5OTk5PC9OdW1iZXJPZlBlcm1pdHRlZFNpdGU+CiAgICAgICAgICAgIDwvU3Vic2NyaXB0aW9uPgogICAgICAgIDwvUHJvZHVjdD4KICAgIDwvUHJvZHVjdHM+CiAgICA8SXNzdWVyPgogICAgICAgIDxOYW1lPklzc3VlcjwvTmFtZT4KICAgICAgICA8RW1haWw+aXNzdWVyQGlzc3Vlci5jb208L0VtYWlsPgogICAgICAgIDxVcmw+aHR0cDovL3d3dy5pc3N1ZXIuY29tPC9Vcmw+CiAgICA8L0lzc3Vlcj4KPC9MaWNlbnNlPg==";

                Spire.License.LicenseProvider.SetLicenseKey(LData);
                Workbook workbook = new Workbook();
                var mappedPathSource = System.Web.Hosting.HostingEnvironment.MapPath("~/upload/" + "shift" + ".xlsx");
                workbook.LoadFromFile(mappedPathSource);


                var sheetNumber = 0;
                foreach (var sh in _sheets)
                {
                    Worksheet sheet = workbook.Worksheets[sheetNumber];
                    sheet.Range[3, 3].Text = dt.ToString("yyyy-MM-dd");
                    var ln = 6;
                    foreach (var fdp in sh)
                    {
                        sheet.Range[ln + 0, 2].Text = fdp.REG;

                        var _fr = ln + 3;
                        var _fc = 6;
                        foreach (var flt in fdp.Flights)
                        {
                            sheet.Range[_fr, 2].Text = flt.FlightNumber;

                            sheet.Range[ln, _fc].Text = flt.Dep;
                            sheet.Range[ln, _fc + 1].Text = flt.Dest;

                            sheet.Range[ln + 1, _fc].Text = ((DateTime)flt.STDLocal).ToString("HH:mm");
                            sheet.Range[ln + 1, _fc + 1].Text = ((DateTime)flt.STALocal).ToString("HH:mm");

                            _fc = _fc + 2;

                            _fr++;
                        }

                        var _cr = ln;
                        foreach (var c in fdp.Cockpit)
                        {
                            sheet.Range[_cr, 3].Text = c;

                            _cr++;
                        }

                        _cr = ln;
                        foreach (var c in fdp.Cabin)
                        {
                            sheet.Range[_cr, 4].Text = c;

                            _cr++;
                        }

                        ln = ln + 9;
                    }


                    sheetNumber++;
                }

                var name = "shift-" + ((DateTime)dt).ToString("yyyy-MMM-dd");
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/upload/" + name + ".xlsx");



                workbook.SaveToFile(mappedPath, ExcelVersion.Version2016);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(mappedPath, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = name + ".xlsx";
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");


                return response;
            
            
            
        }

        public class ShiftFlight
        {
            public int? FlightId { get; set; }
            public string FlightNumber { get; set; }
            public string Dep { get; set; }
            public string Dest { get; set; }
            public DateTime? STD { get; set; }
            public DateTime? STDLocal { get; set; }
            public DateTime? STA { get; set; }
            public DateTime? STALocal { get; set; }
        }
        public class ShiftData
        {
            public int FDPId { get; set; }
            public DateTime? STD { get; set; }
            public string REG { get; set; }
            public List<ShiftFlight> Flights { get; set; }
            public List<string> Cockpit { get; set; }
            public List<string> Cabin { get; set; }
            public List<int?> FlightIds { get; set; }

        }


    }
}
