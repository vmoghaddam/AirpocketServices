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
using ApiAPSB.Models;
using System.Net.Http.Headers;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Threading;


namespace ApiAPSB.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DispatchController : ApiController
    {
        [Route("api/dr/test/{fltid}")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> GetDR(int fltid)
        {
            var x = Environment.GetEnvironmentVariable("cnn_string", EnvironmentVariableTarget.User);
            var _context = new Models.dbEntities();

            var appleg = await _context.XAppLegs.OrderByDescending(q => q.ID).Select(q => q.FlightId).FirstOrDefaultAsync();

            return Ok(appleg);

            // return new DataResponse() { IsSuccess = false };
        }


        [Route("api/applegs/{crtbl}")]

        //nookp
        public IHttpActionResult GetAppLegs(DateTime? df, DateTime? dt, int? ip, int? cpt, int? status, int? asrvr, int crtbl)
        {
            //nooz
            //this.context.Database.CommandTimeout = 160;
            var step = "0";
            try
            {
                df = df != null ? ((DateTime)df).Date : DateTime.MinValue.Date;
                dt = dt != null ? ((DateTime)dt).Date : DateTime.MaxValue.Date;
                var context = new Models.dbEntities();
                var query = from x in context.AppLegOPS
                                // where x.FlightStatusID != 1 && x.FlightStatusID != 4
                            select x;
                query = query.Where(q => q.STDDay >= df && q.STDDay <= dt);
                if (crtbl == 1)
                    query = query.Where(q => q.CRTBL == 1);
                if (ip != null)
                    query = query.Where(q => q.IPId == ip);
                if (cpt != null)
                    query = query.Where(q => q.P1Id == cpt);
                if (asrvr != null)
                {
                    if (asrvr == 1)
                        query = query.Where(q => q.MSN == 1);


                }
                if (status != null)
                {

                    List<int?> sts = new List<int?>();
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
                step = "1";
                var result = query.OrderBy(q => q.STD).ToList();
                step = "2";
                // return result.OrderBy(q => q.STD);
                return Ok(result);
            }
            catch(Exception ex)
            {
                var msg =step+"    "+ ex.Message;
                if (ex.InnerException != null)
                    msg += " INNER: " + ex.InnerException.Message;
                return Ok(msg);
            }
           

        }




        [Route("api/appleg/ofp/{flightId}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetOPF(int flightId)
        {


            var context = new Models.dbEntities();
            var ofp = context.OFPImports.FirstOrDefault(q => q.FlightId == flightId);
            if (ofp == null)
                return Ok(new { Id = -1 });
            else
            {
                var props = context.OFPImportProps.Where(q => q.OFPId == ofp.Id).Select(q =>
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



                    ofp.DOW,
                    ofp.FLL,
                    ofp.MCI,
                    ofp.JAPlan1,
                    ofp.JAPlan2,
                    ofp.JPlan,
                    ofp.JFuel,

                    ofp.JWTDRF,
                    ofp.JCSTBL,
                    ofp.JALDRF,


                    props

                });
            }



        }



        [Route("api/atc/text/get/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetImportATCTextGET(int id)
        {
            try
            {
                var context = new Models.dbEntities();


                var flightObj = context.FlightInformations.FirstOrDefault(q => q.ID == id);


                return Ok(flightObj.ATCPlan);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += ex.InnerException.Message;
                return Ok(msg);
            }

        }


        [Route("api/upload/atc/flightplan")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadATCFLIGHPLAN()
        {
            try
            {
                IHttpActionResult outPut = Ok(200);

                string key = string.Empty;
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var date = DateTime.Now;
                        var ext = System.IO.Path.GetExtension(postedFile.FileName);
                        key = "atc-" + date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + ext;

                        var filePath = ConfigurationManager.AppSettings["atc"] + key; //HttpContext.Current.Server.MapPath("~/upload/" + key);
                        postedFile.SaveAs(filePath);
                        docfiles.Add(filePath);
                    }
                    // outPut = (await ImportFlights2(key));
                    // var ctrl = new FlightController();
                    //  outPut = await ctrl.UploadFlights3(key);
                    outPut = Ok(key);

                }
                else
                {
                    return Ok("error");
                }
                return outPut;
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + "   IN    " + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }

        }
        [Route("api/flight/atc/update/{id}/{fn}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetATcUpdate(int id, string fn)
        {
            var context = new Models.dbEntities();
            try
            {
                var flt = context.FlightInformations.FirstOrDefault(q => q.ID == id);
                if (flt != null)
                    flt.ATCPlan = fn.Split('X')[0] + "." + fn.Split('X')[1]; //".pdf";
                context.SaveChanges();
                return Ok("done");
            }
            catch (Exception ex)
            {
                var msg = ex.Message + " IN:" + (ex.InnerException != null ? ex.InnerException.Message : "NO");
                return Ok(msg);
            }

        }


        [Route("api/atc/text/input")]
        [AcceptVerbs("POST")]
        public IHttpActionResult PostImportATCTextInput(dynamic dto)
        {
            try
            {
                string user = Convert.ToString(dto.user);
                int fltId = Convert.ToInt32(dto.fltId);
                var context = new Models.dbEntities();

                var flight = context.ViewLegTimes.FirstOrDefault(q => q.ID == fltId);
                var flightObj = context.FlightInformations.FirstOrDefault(q => q.ID == fltId);

                string ftext = Convert.ToString(dto.text);
                flightObj.ATCPlan = ftext;
                context.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                var msg = ex.Message + " IN:" + (ex.InnerException != null ? ex.InnerException.Message : "NO");
                return Ok(msg);
            }

        }


        [Route("api/upload/flight/doc")]
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> UploadFlightDoc()
        {
            try
            {
                IHttpActionResult outPut = Ok(200);

                string key = string.Empty;
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var date = DateTime.Now;
                        var ext = System.IO.Path.GetExtension(postedFile.FileName);
                        key = "doc-" + date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + ext;

                        var filePath = ConfigurationManager.AppSettings["fltdoc"] + key; //HttpContext.Current.Server.MapPath("~/upload/" + key);
                        postedFile.SaveAs(filePath);
                        docfiles.Add(filePath);
                    }
                    // outPut = (await ImportFlights2(key));
                    // var ctrl = new FlightController();
                    //  outPut = await ctrl.UploadFlights3(key);
                    outPut = Ok(key);

                }
                else
                {
                    return Ok("error");
                }
                return outPut;
            }
            catch (Exception ex)
            {
                return Ok(ex.Message + "   IN    " + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }

        }


        [Route("api/flight/doc/save/")]
        [AcceptVerbs("POST")]
        public IHttpActionResult PostFlightDoc(FlightDocDto dto)
        {
            var context = new Models.dbEntities();
            try
            {
                var doc = context.FlightDocuments.Where(q => q.FlightId==dto.FlightId && q.DocumentType==dto.DocumentType).FirstOrDefault();
                if (doc == null)
                {
                    doc = new FlightDocument();
                    context.FlightDocuments.Add(doc);
                }
                doc.FlightId = dto.FlightId;
                doc.Remark = dto.Remark;
                doc.DateCreate = DateTime.UtcNow;
                doc.DocumentUrl = dto.DocumentUrl;
                doc.DocumentType = dto.DocumentType;
                var flight = context.FlightInformations.Where(q => q.ID == dto.FlightId).FirstOrDefault();
                switch (dto.DocumentType)
                {
                    case "ddl":
                        flight.CPInstructor = doc.DocumentUrl;
                        break;
                    case "packinglist":
                        flight.CPP1 = doc.DocumentUrl;
                        break;
                    case "spwx":
                        flight.CPP2 = doc.DocumentUrl;
                        break;
                    case "notam":
                        flight.CPISCCM = doc.DocumentUrl;
                        break;
                    case "other":
                        flight.CPSCCM = doc.DocumentUrl;
                        break;

                    default:
                        break;
                }

                context.SaveChanges();
                return Ok(doc);
            }
            catch (Exception ex)
            {
                var msg = ex.Message + " IN:" + (ex.InnerException != null ? ex.InnerException.Message : "NO");
                return Ok(msg);
            }

        }

        [Route("api/flight/doc/visit/{flt}/{type}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetDocVisit(int flt, string type)
        {
            var context = new Models.dbEntities();
            try
            {
                //  var flight = context.FlightInformations.FirstOrDefault(q => q.ID == flt);
                var doc = context.FlightDocuments.Where(q => q.FlightId == flt && q.DocumentType == type).OrderByDescending(q => q.Id).FirstOrDefault();
                if (doc != null)
                    doc.DateVisite = DateTime.Now;
                context.SaveChanges();
                return Ok("done");
            }
            catch (Exception ex)
            {
                var msg = ex.Message + " IN:" + (ex.InnerException != null ? ex.InnerException.Message : "NO");
                return Ok(msg);
            }

        }

        [Route("api/flight/docs/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightsDocs(int id)
        {
            var context = new Models.dbEntities();
            try
            {
                var docs = context.FlightDocuments.Where(q => q.FlightId == id).OrderBy(q => q.DateCreate).ToList();
                return Ok(docs);
            }
            catch (Exception ex)
            {
                var msg = ex.Message + " IN:" + (ex.InnerException != null ? ex.InnerException.Message : "NO");
                return Ok(msg);
            }

        }


        

        [Route("api/flight/doc/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightsDoc(int id)
        {
            var context = new Models.dbEntities();
            try
            {
                var docs = context.FlightDocuments.Where(q => q.Id == id).FirstOrDefault();
                return Ok(docs);
            }
            catch (Exception ex)
            {
                var msg = ex.Message + " IN:" + (ex.InnerException != null ? ex.InnerException.Message : "NO");
                return Ok(msg);
            }

        }

        [Route("api/flight/doc/type/{id}/{type}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetFlightsDoc(int id,string type)
        {
            var context = new Models.dbEntities();
            try
            {
                var docs = context.FlightDocuments.Where(q => q.FlightId == id && q.DocumentType==type).FirstOrDefault();
                return Ok(docs);
            }
            catch (Exception ex)
            {
                var msg = ex.Message + " IN:" + (ex.InnerException != null ? ex.InnerException.Message : "NO");
                return Ok(msg);
            }

        }

        public class FlightDocDto
        {
            public int Id { get; set; }

            public int FlightId { get; set; }
            public string DocumentType { get; set; }
            public string DocumentUrl { get; set; }
            public string Remark { get; set; }
        }


        [Route("api/dr/test1/{fltid}")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> GetDR1(int fltid)
        {
            var _context = new Models.dbEntities();

            var appleg = await _context.XAppLegs.FirstOrDefaultAsync(q => q.FlightId == fltid);
            var appcrewflight = await _context.AppCrewFlights.Where(q => q.FlightId == appleg.FlightId && q.CrewId == appleg.PICId).FirstOrDefaultAsync();
            // var fdpitems = await _context.FDPItems.Where(q => q.FDPId == appcrewflight.FDPId).ToListAsync();
            //var fltIds = fdpitems.Select(q => q.FlightId).ToList();

            return Ok(appcrewflight);

            // return new DataResponse() { IsSuccess = false };
        }
        [Route("api/dr/test2/{fltid}")]
        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> GetDR2(int fltid)
        {
            var _context = new Models.dbEntities();

            var appleg = await _context.XAppLegs.FirstOrDefaultAsync(q => q.FlightId == fltid);
            var appcrewflight = await _context.AppCrewFlights.Where(q => q.FlightId == appleg.FlightId && q.CrewId == appleg.PICId).FirstOrDefaultAsync();
            var fdpitems = await _context.FDPItems.Where(q => q.FDPId == appcrewflight.FDPId).ToListAsync();
            //var fltIds = fdpitems.Select(q => q.FlightId).ToList();

            return Ok(fdpitems);

            // return new DataResponse() { IsSuccess = false };
        }


        [Route("api/efb/dr/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostDR(DSPReleaseViewModel DSPRelease)
        {
            var step = "0";
            try
            {
                var _context = new Models.dbEntities();

                var appleg = await _context.XAppLegs.FirstOrDefaultAsync(q => q.FlightId == DSPRelease.FlightId);
                var appcrewflight = await _context.AppCrewFlights.Where(q => q.FlightId == appleg.FlightId && q.CrewId == appleg.PICId).FirstOrDefaultAsync();
                var fdpitems = await _context.FDPItems.Where(q => q.FDPId == appcrewflight.FDPId).ToListAsync();
                var fltIds = fdpitems.Select(q => q.FlightId).ToList();
                step = "1";
                var drs = await _context.EFBDSPReleases.Where(q => fltIds.Contains(q.FlightId)).ToListAsync();
                step = "2";
                _context.EFBDSPReleases.RemoveRange(drs);
                await _context.SaveChangesAsync();
                var _res = new List<object>();
                foreach (var flightId in fltIds)
                {
                    var release = await _context.EFBDSPReleases.FirstOrDefaultAsync(q => q.FlightId == DSPRelease.FlightId);
                    if (release == null)
                    {
                        release = new EFBDSPRelease();
                        _context.EFBDSPReleases.Add(release);

                    }

                    release.User = DSPRelease.User;
                    release.DateUpdate = DateTime.UtcNow.ToString("yyyyMMddHHmm");


                    release.FlightId = flightId; //DSPRelease.FlightId;
                    release.ActualWXDSP = DSPRelease.ActualWXDSP;
                    release.ActualWXCPT = DSPRelease.ActualWXCPT;
                    release.ActualWXDSPRemark = DSPRelease.ActualWXDSPRemark;
                    release.ActualWXCPTRemark = DSPRelease.ActualWXCPTRemark;
                    release.WXForcastDSP = DSPRelease.WXForcastDSP;
                    release.WXForcastCPT = DSPRelease.WXForcastCPT;
                    release.WXForcastDSPRemark = DSPRelease.WXForcastDSPRemark;
                    release.WXForcastCPTRemark = DSPRelease.WXForcastCPTRemark;
                    release.SigxWXDSP = DSPRelease.SigxWXDSP;
                    release.SigxWXCPT = DSPRelease.SigxWXCPT;
                    release.SigxWXDSPRemark = DSPRelease.SigxWXDSPRemark;
                    release.SigxWXCPTRemark = DSPRelease.SigxWXCPTRemark;
                    release.WindChartDSP = DSPRelease.WindChartDSP;
                    release.WindChartCPT = DSPRelease.WindChartCPT;
                    release.WindChartDSPRemark = DSPRelease.WindChartDSPRemark;
                    release.WindChartCPTRemark = DSPRelease.WindChartCPTRemark;
                    release.NotamDSP = DSPRelease.NotamDSP;
                    release.NotamCPT = DSPRelease.NotamCPT;
                    release.NotamDSPRemark = DSPRelease.NotamDSPRemark;
                    release.NotamCPTRemark = DSPRelease.NotamCPTRemark;
                    release.ComputedFligthPlanDSP = DSPRelease.ComputedFligthPlanDSP;
                    release.ComputedFligthPlanCPT = DSPRelease.ComputedFligthPlanCPT;
                    release.ComputedFligthPlanDSPRemark = DSPRelease.ComputedFligthPlanDSPRemark;
                    release.ComputedFligthPlanCPTRemark = DSPRelease.ComputedFligthPlanCPTRemark;
                    release.ATCFlightPlanDSP = DSPRelease.ATCFlightPlanDSP;
                    release.ATCFlightPlanCPT = DSPRelease.ATCFlightPlanCPT;
                    release.ATCFlightPlanDSPRemark = DSPRelease.ATCFlightPlanDSPRemark;
                    release.ATCFlightPlanCPTRemark = DSPRelease.ATCFlightPlanCPTRemark;
                    release.PermissionsDSP = DSPRelease.PermissionsDSP;
                    release.PermissionsCPT = DSPRelease.PermissionsCPT;
                    release.PermissionsDSPRemark = DSPRelease.PermissionsDSPRemark;
                    release.PermissionsCPTRemark = DSPRelease.PermissionsCPTRemark;
                    release.JeppesenAirwayManualDSP = DSPRelease.JeppesenAirwayManualDSP;
                    release.JeppesenAirwayManualCPT = DSPRelease.JeppesenAirwayManualCPT;
                    release.JeppesenAirwayManualDSPRemark = DSPRelease.JeppesenAirwayManualDSPRemark;
                    release.JeppesenAirwayManualCPTRemark = DSPRelease.JeppesenAirwayManualCPTRemark;
                    release.MinFuelRequiredDSP = DSPRelease.MinFuelRequiredDSP;
                    release.MinFuelRequiredCPT = DSPRelease.MinFuelRequiredCPT;
                 //   release.MinFuelRequiredCFP = DSPRelease.MinFuelRequiredCFP;
                  //  release.MinFuelRequiredPilotReq = DSPRelease.MinFuelRequiredPilotReq;
                    release.GeneralDeclarationDSP = DSPRelease.GeneralDeclarationDSP;
                    release.GeneralDeclarationCPT = DSPRelease.GeneralDeclarationCPT;
                    release.GeneralDeclarationDSPRemark = DSPRelease.GeneralDeclarationDSPRemark;
                    release.GeneralDeclarationCPTRemark = DSPRelease.GeneralDeclarationCPTRemark;
                    release.FlightReportDSP = DSPRelease.FlightReportDSP;
                    release.FlightReportCPT = DSPRelease.FlightReportCPT;
                    release.FlightReportDSPRemark = DSPRelease.FlightReportDSPRemark;
                    release.FlightReportCPTRemark = DSPRelease.FlightReportCPTRemark;
                    release.TOLndCardsDSP = DSPRelease.TOLndCardsDSP;
                    release.TOLndCardsCPT = DSPRelease.TOLndCardsCPT;
                    release.TOLndCardsDSPRemark = DSPRelease.TOLndCardsDSPRemark;
                    release.TOLndCardsCPTRemark = DSPRelease.TOLndCardsCPTRemark;
                    release.LoadSheetDSP = DSPRelease.LoadSheetDSP;
                    release.LoadSheetCPT = DSPRelease.LoadSheetCPT;
                    release.LoadSheetDSPRemark = DSPRelease.LoadSheetDSPRemark;
                    release.LoadSheetCPTRemark = DSPRelease.LoadSheetCPTRemark;
                    release.FlightSafetyReportDSP = DSPRelease.FlightSafetyReportDSP;
                    release.FlightSafetyReportCPT = DSPRelease.FlightSafetyReportCPT;
                    release.FlightSafetyReportDSPRemark = DSPRelease.FlightSafetyReportDSPRemark;
                    release.FlightSafetyReportCPTRemark = DSPRelease.FlightSafetyReportCPTRemark;
                    release.AVSECIncidentReportDSP = DSPRelease.AVSECIncidentReportDSP;
                    release.AVSECIncidentReportCPT = DSPRelease.AVSECIncidentReportCPT;
                    release.AVSECIncidentReportDSPRemark = DSPRelease.AVSECIncidentReportDSPRemark;
                    release.AVSECIncidentReportCPTRemark = DSPRelease.AVSECIncidentReportCPTRemark;
                    release.OperationEngineeringDSP = DSPRelease.OperationEngineeringDSP;
                    release.OperationEngineeringCPT = DSPRelease.OperationEngineeringCPT;
                    release.OperationEngineeringDSPRemark = DSPRelease.OperationEngineeringDSPRemark;
                    release.OperationEngineeringCPTRemark = DSPRelease.OperationEngineeringCPTRemark;
                    release.VoyageReportDSP = DSPRelease.VoyageReportDSP;
                    release.VoyageReportCPT = DSPRelease.VoyageReportCPT;
                    release.VoyageReportDSPRemark = DSPRelease.VoyageReportDSPRemark;
                    release.VoyageReportCPTRemark = DSPRelease.VoyageReportCPTRemark;
                    release.PIFDSP = DSPRelease.PIFDSP;
                    release.PIFCPT = DSPRelease.PIFCPT;
                    release.PIFDSPRemark = DSPRelease.PIFDSPRemark;
                    release.PIFCPTRemark = DSPRelease.PIFCPTRemark;
                    release.GoodDeclarationDSP = DSPRelease.GoodDeclarationDSP;
                    release.GoodDeclarationCPT = DSPRelease.GoodDeclarationCPT;
                    release.GoodDeclarationDSPRemark = DSPRelease.GoodDeclarationDSPRemark;
                    release.GoodDeclarationCPTRemark = DSPRelease.GoodDeclarationCPTRemark;
                    release.IPADDSP = DSPRelease.IPADDSP;
                    release.IPADCPT = DSPRelease.IPADCPT;
                    release.IPADDSPRemark = DSPRelease.IPADDSPRemark;
                    release.IPADCPTRemark = DSPRelease.IPADCPTRemark;
                    release.DateConfirmed = DSPRelease.DateConfirmed;
                    release.DispatcherId = DSPRelease.DispatcherId;
                    release.ATSFlightPlanCMDR = DSPRelease.ATSFlightPlanCMDR;
                    release.ATSFlightPlanFOO = DSPRelease.ATSFlightPlanFOO;
                    release.ATSFlightPlanFOORemark = DSPRelease.ATSFlightPlanFOORemark;
                    release.ATSFlightPlanCMDRRemark = DSPRelease.ATSFlightPlanCMDRRemark;
                    release.VldCMCCMDR = DSPRelease.VldCMCCMDR;
                    release.VldCMCCMDRRemark = DSPRelease.VldCMCCMDRRemark;
                    release.VldCMCFOO = DSPRelease.VldCMCFOO;
                    release.VldCMCFOORemark = DSPRelease.VldCMCFOORemark;
                    release.VldEFBCMDR = DSPRelease.VldEFBCMDR;
                    release.VldEFBCMDRRemark = DSPRelease.VldEFBCMDRRemark;
                    release.VldEFBFOO = DSPRelease.VldEFBFOO;
                    release.VldEFBFOORemark = DSPRelease.VldEFBFOORemark;
                    release.VldFlightCrewCMDR = DSPRelease.VldFlightCrewCMDR;
                    release.VldFlightCrewCMDRRemark = DSPRelease.VldFlightCrewCMDRRemark;
                    release.VldFlightCrewFOO = DSPRelease.VldFlightCrewFOO;
                    release.VldFlightCrewFOORemark = DSPRelease.VldFlightCrewFOORemark;
                    release.VldMedicalCMDR = DSPRelease.VldMedicalCMDR;
                    release.VldMedicalCMDRRemark = DSPRelease.VldMedicalCMDRRemark;
                    release.VldMedicalFOO = DSPRelease.VldMedicalFOO;
                    release.VldMedicalFOORemark = DSPRelease.VldMedicalFOORemark;
                    release.VldPassportCMDR = DSPRelease.VldPassportCMDR;
                    release.VldPassportCMDRRemark = DSPRelease.VldPassportCMDRRemark;
                    release.VldPassportFOO = DSPRelease.VldPassportFOO;
                    release.VldPassportFOORemark = DSPRelease.VldPassportFOORemark;
                    release.VldRampPassCMDR = DSPRelease.VldRampPassCMDR;
                    release.VldRampPassCMDRRemark = DSPRelease.VldRampPassCMDRRemark;
                    release.VldRampPassFOO = DSPRelease.VldRampPassFOO;
                    release.VldRampPassFOORemark = DSPRelease.VldRampPassFOORemark;
                    release.OperationalFlightPlanFOO = DSPRelease.OperationalFlightPlanFOO;
                    release.OperationalFlightPlanFOORemark = DSPRelease.OperationalFlightPlanFOORemark;
                    release.OperationalFlightPlanCMDR = DSPRelease.OperationalFlightPlanCMDR;
                    release.OperationalFlightPlanCMDRRemark = DSPRelease.OperationalFlightPlanCMDRRemark;
                    _res.Add(release);
                }


                var saveResult = await _context.SaveChangesAsync();


                return Ok(new DataResponse() { IsSuccess = true, Data = _res });

            }
            catch(Exception ex)
            {
                var msg =step+"   "+ ex.Message;
                if (ex.InnerException != null)
                    msg += ex.InnerException.Message;
                return Ok(msg);
            }

            // return new DataResponse() { IsSuccess = false };
        }
        public class SimpleDto
        {
            public List<int> ids { get; set; }
        }
        [AcceptVerbs("POST")]
        [Route("api/drs")]
        public async Task<IHttpActionResult> PostgetDRs(SimpleDto dto)
        {
            var ids = dto.ids.Select(q => (Nullable<int>)q).ToList();
            var _context = new Models.dbEntities();
            var drs=await _context.ViewEFBDSPReleases.Where(q=> ids.Contains(q.FlightId)).ToListAsync();
            return Ok(new DataResponse()
            {
                Data = drs,
                IsSuccess = true

            });

        }

       
        [Route("api/efb/dr/{flightId}")]
        public async Task<IHttpActionResult> GetDRByFlightId(int flightId)
        {
            var _context = new Models.dbEntities();
            var entity = await _context.ViewEFBDSPReleases.FirstOrDefaultAsync(q => q.FlightId == flightId);
            var flight = await _context.FlightInformations.FirstOrDefaultAsync(q => q.ID == flightId);
            if (entity != null)
            {
                entity.MinFuelRequiredCFP = flight.OFPOFFBLOCKFUEL == null ? 0 : (decimal)flight.OFPOFFBLOCKFUEL;//flight.FPFuel == null ? 0 : (decimal)flight.FPFuel + 200;
                entity.MinFuelRequiredPilotReq =flight.FuelPlanned;// flight.FuelPlanned;
                
            }
            else
            {
                entity = new ViewEFBDSPReleas()
                {
                    Id = -1,
                    MinFuelRequiredCFP = flight.OFPOFFBLOCKFUEL == null ? 0 : (decimal)flight.OFPOFFBLOCKFUEL,
                    MinFuelRequiredPilotReq= flight.FuelPlanned,

                };
            }
            return Ok(new DataResponse()
            {
                Data = entity,
                IsSuccess = true

            });
        }

        DateTime ParseDate(string str)
        {
            var _y =Convert.ToInt32( str.Substring(0, 4));
            var _m = Convert.ToInt32(str.Substring(4, 2));
            var _d = Convert.ToInt32(str.Substring(6, 2));
            var _h = Convert.ToInt32(str.Substring(8, 2));
            var _mm = Convert.ToInt32(str.Substring(10, 2));
            var _s = Convert.ToInt32(str.Substring(12, 2));

            var dt = new DateTime(_y, _m, _d, _h, _mm, _s);

            return dt;
        }

        [Route("api/efb/value/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostEFBValue(EFBValues dto)
        {
            var _context = new Models.dbEntities();

            foreach(var rec in dto.Values)
            {
                var dbrec = new EFBValue();
                dbrec.UserName = rec.UserName;
                dbrec.FieldName = rec.FieldName;
                dbrec.FieldValue = rec.FieldValue;
                dbrec.TableName = rec.TableName;
                dbrec.FlightId = rec.FlightId;
                dbrec.DateCreate = ParseDate(rec.DateCreate);

                _context.EFBValues.Add(dbrec);
            }
            var saveResult = await _context.SaveChangesAsync();
 
            return Ok(new DataResponse() { IsSuccess = true });



            // return new DataResponse() { IsSuccess = false };
        }

        [Route("api/dr/flight/{fltid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetDRByFlight(int fltid)
        {
           // return Ok('X');
            try
            {
                var context = new Models.dbEntities();
                var result = context.ViewEFBDSPReleases.FirstOrDefault(q => q.FlightId == fltid);
                return Ok(result);
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "   INNER: " + ex.InnerException.Message;
                return Ok(msg);
            }
        }

        [Route("api/dsp/dr/flight/{fltid}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetDRByFlightDSP(int fltid)
        {


            var context = new Models.dbEntities();
            var result = context.ViewEFBDSPReleases.FirstOrDefault(q => q.FlightId == fltid);
            //.EFBDSPReleases.FirstOrDefault(q => q.FlightId == fltid);
            return Ok(result);
        }
        [Route("api/dr/save")]
        [AcceptVerbs("Post")]
        public IHttpActionResult PostDRX(DSPReleaseViewModel DSPRelease)
        {
            var context = new Models.dbEntities();
            var release = context.EFBDSPReleases.FirstOrDefault(q => q.FlightId == DSPRelease.FlightId);
            if (release == null)
            {
                release = new EFBDSPRelease();
                context.EFBDSPReleases.Add(release);

            }

            release.User = DSPRelease.User;
            release.DateUpdate = DateTime.UtcNow.ToString("yyyyMMddHHmm");


            release.FlightId = DSPRelease.FlightId;
            release.ActualWXDSP = DSPRelease.ActualWXDSP;
            release.ActualWXCPT = DSPRelease.ActualWXCPT;
            release.ActualWXDSPRemark = DSPRelease.ActualWXDSPRemark;
            release.ActualWXCPTRemark = DSPRelease.ActualWXCPTRemark;
            release.WXForcastDSP = DSPRelease.WXForcastDSP;
            release.WXForcastCPT = DSPRelease.WXForcastCPT;
            release.WXForcastDSPRemark = DSPRelease.WXForcastDSPRemark;
            release.WXForcastCPTRemark = DSPRelease.WXForcastCPTRemark;
            release.SigxWXDSP = DSPRelease.SigxWXDSP;
            release.SigxWXCPT = DSPRelease.SigxWXCPT;
            release.SigxWXDSPRemark = DSPRelease.SigxWXDSPRemark;
            release.SigxWXCPTRemark = DSPRelease.SigxWXCPTRemark;
            release.WindChartDSP = DSPRelease.WindChartDSP;
            release.WindChartCPT = DSPRelease.WindChartCPT;
            release.WindChartDSPRemark = DSPRelease.WindChartDSPRemark;
            release.WindChartCPTRemark = DSPRelease.WindChartCPTRemark;
            release.NotamDSP = DSPRelease.NotamDSP;
            release.NotamCPT = DSPRelease.NotamCPT;
            release.NotamDSPRemark = DSPRelease.NotamDSPRemark;
            release.NotamCPTRemark = DSPRelease.NotamCPTRemark;
            release.ComputedFligthPlanDSP = DSPRelease.ComputedFligthPlanDSP;
            release.ComputedFligthPlanCPT = DSPRelease.ComputedFligthPlanCPT;
            release.ComputedFligthPlanDSPRemark = DSPRelease.ComputedFligthPlanDSPRemark;
            release.ComputedFligthPlanCPTRemark = DSPRelease.ComputedFligthPlanCPTRemark;
            release.ATCFlightPlanDSP = DSPRelease.ATCFlightPlanDSP;
            release.ATCFlightPlanCPT = DSPRelease.ATCFlightPlanCPT;
            release.ATCFlightPlanDSPRemark = DSPRelease.ATCFlightPlanDSPRemark;
            release.ATCFlightPlanCPTRemark = DSPRelease.ATCFlightPlanCPTRemark;
            release.PermissionsDSP = DSPRelease.PermissionsDSP;
            release.PermissionsCPT = DSPRelease.PermissionsCPT;
            release.PermissionsDSPRemark = DSPRelease.PermissionsDSPRemark;
            release.PermissionsCPTRemark = DSPRelease.PermissionsCPTRemark;
            release.JeppesenAirwayManualDSP = DSPRelease.JeppesenAirwayManualDSP;
            release.JeppesenAirwayManualCPT = DSPRelease.JeppesenAirwayManualCPT;
            release.JeppesenAirwayManualDSPRemark = DSPRelease.JeppesenAirwayManualDSPRemark;
            release.JeppesenAirwayManualCPTRemark = DSPRelease.JeppesenAirwayManualCPTRemark;
            release.MinFuelRequiredDSP = DSPRelease.MinFuelRequiredDSP;
            release.MinFuelRequiredCPT = DSPRelease.MinFuelRequiredCPT;
            release.MinFuelRequiredCFP = DSPRelease.MinFuelRequiredCFP;
            release.MinFuelRequiredPilotReq = DSPRelease.MinFuelRequiredPilotReq;
            release.GeneralDeclarationDSP = DSPRelease.GeneralDeclarationDSP;
            release.GeneralDeclarationCPT = DSPRelease.GeneralDeclarationCPT;
            release.GeneralDeclarationDSPRemark = DSPRelease.GeneralDeclarationDSPRemark;
            release.GeneralDeclarationCPTRemark = DSPRelease.GeneralDeclarationCPTRemark;
            release.FlightReportDSP = DSPRelease.FlightReportDSP;
            release.FlightReportCPT = DSPRelease.FlightReportCPT;
            release.FlightReportDSPRemark = DSPRelease.FlightReportDSPRemark;
            release.FlightReportCPTRemark = DSPRelease.FlightReportCPTRemark;
            release.TOLndCardsDSP = DSPRelease.TOLndCardsDSP;
            release.TOLndCardsCPT = DSPRelease.TOLndCardsCPT;
            release.TOLndCardsDSPRemark = DSPRelease.TOLndCardsDSPRemark;
            release.TOLndCardsCPTRemark = DSPRelease.TOLndCardsCPTRemark;
            release.LoadSheetDSP = DSPRelease.LoadSheetDSP;
            release.LoadSheetCPT = DSPRelease.LoadSheetCPT;
            release.LoadSheetDSPRemark = DSPRelease.LoadSheetDSPRemark;
            release.LoadSheetCPTRemark = DSPRelease.LoadSheetCPTRemark;
            release.FlightSafetyReportDSP = DSPRelease.FlightSafetyReportDSP;
            release.FlightSafetyReportCPT = DSPRelease.FlightSafetyReportCPT;
            release.FlightSafetyReportDSPRemark = DSPRelease.FlightSafetyReportDSPRemark;
            release.FlightSafetyReportCPTRemark = DSPRelease.FlightSafetyReportCPTRemark;
            release.AVSECIncidentReportDSP = DSPRelease.AVSECIncidentReportDSP;
            release.AVSECIncidentReportCPT = DSPRelease.AVSECIncidentReportCPT;
            release.AVSECIncidentReportDSPRemark = DSPRelease.AVSECIncidentReportDSPRemark;
            release.AVSECIncidentReportCPTRemark = DSPRelease.AVSECIncidentReportCPTRemark;
            release.OperationEngineeringDSP = DSPRelease.OperationEngineeringDSP;
            release.OperationEngineeringCPT = DSPRelease.OperationEngineeringCPT;
            release.OperationEngineeringDSPRemark = DSPRelease.OperationEngineeringDSPRemark;
            release.OperationEngineeringCPTRemark = DSPRelease.OperationEngineeringCPTRemark;
            release.VoyageReportDSP = DSPRelease.VoyageReportDSP;
            release.VoyageReportCPT = DSPRelease.VoyageReportCPT;
            release.VoyageReportDSPRemark = DSPRelease.VoyageReportDSPRemark;
            release.VoyageReportCPTRemark = DSPRelease.VoyageReportCPTRemark;
            release.PIFDSP = DSPRelease.PIFDSP;
            release.PIFCPT = DSPRelease.PIFCPT;
            release.PIFDSPRemark = DSPRelease.PIFDSPRemark;
            release.PIFCPTRemark = DSPRelease.PIFCPTRemark;
            release.GoodDeclarationDSP = DSPRelease.GoodDeclarationDSP;
            release.GoodDeclarationCPT = DSPRelease.GoodDeclarationCPT;
            release.GoodDeclarationDSPRemark = DSPRelease.GoodDeclarationDSPRemark;
            release.GoodDeclarationCPTRemark = DSPRelease.GoodDeclarationCPTRemark;
            release.IPADDSP = DSPRelease.IPADDSP;
            release.IPADCPT = DSPRelease.IPADCPT;
            release.IPADDSPRemark = DSPRelease.IPADDSPRemark;
            release.IPADCPTRemark = DSPRelease.IPADCPTRemark;
            release.DateConfirmed = DSPRelease.DateConfirmed;
            release.DispatcherId = DSPRelease.DispatcherId;
            context.SaveChanges();
            return Ok(release);


        }

        [Route("api/dsp/dr/save")]
        [AcceptVerbs("Post")]
        public IHttpActionResult PostDRDSP(DSPReleaseViewModel2 DSPRelease)
        {
            try
            {
                var context = new Models.dbEntities();
                var release = context.EFBDSPReleases.FirstOrDefault(q => q.FlightId == DSPRelease.FlightId);
                if (release == null)
                {
                    release = new EFBDSPRelease();
                    context.EFBDSPReleases.Add(release);

                }

                // release.User = DSPRelease.User;



                release.DateUpdate = DateTime.UtcNow.ToString("yyyyMMddHHmm");


                release.FlightId = DSPRelease.FlightId;
                release.ActualWXDSP = DSPRelease.ActualWXDSP;
                // release.ActualWXCPT = DSPRelease.ActualWXCPT;
                release.ActualWXDSPRemark = DSPRelease.ActualWXDSPRemark;
                //release.ActualWXCPTRemark = DSPRelease.ActualWXCPTRemark;
                release.WXForcastDSP = DSPRelease.WXForcastDSP;
                //release.WXForcastCPT = DSPRelease.WXForcastCPT;
                release.WXForcastDSPRemark = DSPRelease.WXForcastDSPRemark;
                //release.WXForcastCPTRemark = DSPRelease.WXForcastCPTRemark;
                release.SigxWXDSP = DSPRelease.SigxWXDSP;
                //release.SigxWXCPT = DSPRelease.SigxWXCPT;
                release.SigxWXDSPRemark = DSPRelease.SigxWXDSPRemark;
                //release.SigxWXCPTRemark = DSPRelease.SigxWXCPTRemark;
                release.WindChartDSP = DSPRelease.WindChartDSP;
                //release.WindChartCPT = DSPRelease.WindChartCPT;
                release.WindChartDSPRemark = DSPRelease.WindChartDSPRemark;
                //release.WindChartCPTRemark = DSPRelease.WindChartCPTRemark;
                release.NotamDSP = DSPRelease.NotamDSP;
                //release.NotamCPT = DSPRelease.NotamCPT;
                release.NotamDSPRemark = DSPRelease.NotamDSPRemark;
                //release.NotamCPTRemark = DSPRelease.NotamCPTRemark;
                release.ComputedFligthPlanDSP = DSPRelease.ComputedFligthPlanDSP;
                //release.ComputedFligthPlanCPT = DSPRelease.ComputedFligthPlanCPT;
                release.ComputedFligthPlanDSPRemark = DSPRelease.ComputedFligthPlanDSPRemark;
                //release.ComputedFligthPlanCPTRemark = DSPRelease.ComputedFligthPlanCPTRemark;
                release.ATCFlightPlanDSP = DSPRelease.ATCFlightPlanDSP;
                //release.ATCFlightPlanCPT = DSPRelease.ATCFlightPlanCPT;
                release.ATCFlightPlanDSPRemark = DSPRelease.ATCFlightPlanDSPRemark;
                //release.ATCFlightPlanCPTRemark = DSPRelease.ATCFlightPlanCPTRemark;
                release.PermissionsDSP = DSPRelease.PermissionsDSP;
                //release.PermissionsCPT = DSPRelease.PermissionsCPT;
                release.PermissionsDSPRemark = DSPRelease.PermissionsDSPRemark;
                //release.PermissionsCPTRemark = DSPRelease.PermissionsCPTRemark;
                release.JeppesenAirwayManualDSP = DSPRelease.JeppesenAirwayManualDSP;
                //release.JeppesenAirwayManualCPT = DSPRelease.JeppesenAirwayManualCPT;
                release.JeppesenAirwayManualDSPRemark = DSPRelease.JeppesenAirwayManualDSPRemark;
                //release.JeppesenAirwayManualCPTRemark = DSPRelease.JeppesenAirwayManualCPTRemark;
                release.MinFuelRequiredDSP = DSPRelease.MinFuelRequiredDSP;
                //release.MinFuelRequiredCPT = DSPRelease.MinFuelRequiredCPT;
                //release.MinFuelRequiredCFP = DSPRelease.MinFuelRequiredCFP;
                //release.MinFuelRequiredPilotReq = DSPRelease.MinFuelRequiredPilotReq;
                release.GeneralDeclarationDSP = DSPRelease.GeneralDeclarationDSP;
                //release.GeneralDeclarationCPT = DSPRelease.GeneralDeclarationCPT;
                release.GeneralDeclarationDSPRemark = DSPRelease.GeneralDeclarationDSPRemark;
                //release.GeneralDeclarationCPTRemark = DSPRelease.GeneralDeclarationCPTRemark;
                release.FlightReportDSP = DSPRelease.FlightReportDSP;
                //release.FlightReportCPT = DSPRelease.FlightReportCPT;
                release.FlightReportDSPRemark = DSPRelease.FlightReportDSPRemark;
                //release.FlightReportCPTRemark = DSPRelease.FlightReportCPTRemark;
                release.TOLndCardsDSP = DSPRelease.TOLndCardsDSP;
                //release.TOLndCardsCPT = DSPRelease.TOLndCardsCPT;
                release.TOLndCardsDSPRemark = DSPRelease.TOLndCardsDSPRemark;
                //release.TOLndCardsCPTRemark = DSPRelease.TOLndCardsCPTRemark;
                release.LoadSheetDSP = DSPRelease.LoadSheetDSP;
                //release.LoadSheetCPT = DSPRelease.LoadSheetCPT;
                release.LoadSheetDSPRemark = DSPRelease.LoadSheetDSPRemark;
                //release.LoadSheetCPTRemark = DSPRelease.LoadSheetCPTRemark;
                release.FlightSafetyReportDSP = DSPRelease.FlightSafetyReportDSP;
                //release.FlightSafetyReportCPT = DSPRelease.FlightSafetyReportCPT;
                release.FlightSafetyReportDSPRemark = DSPRelease.FlightSafetyReportDSPRemark;
                //release.FlightSafetyReportCPTRemark = DSPRelease.FlightSafetyReportCPTRemark;
                release.AVSECIncidentReportDSP = DSPRelease.AVSECIncidentReportDSP;
                //release.AVSECIncidentReportCPT = DSPRelease.AVSECIncidentReportCPT;
                release.AVSECIncidentReportDSPRemark = DSPRelease.AVSECIncidentReportDSPRemark;
                //release.AVSECIncidentReportCPTRemark = DSPRelease.AVSECIncidentReportCPTRemark;
                release.OperationEngineeringDSP = DSPRelease.OperationEngineeringDSP;
                //release.OperationEngineeringCPT = DSPRelease.OperationEngineeringCPT;
                release.OperationEngineeringDSPRemark = DSPRelease.OperationEngineeringDSPRemark;
                //release.OperationEngineeringCPTRemark = DSPRelease.OperationEngineeringCPTRemark;
                release.VoyageReportDSP = DSPRelease.VoyageReportDSP;
                //release.VoyageReportCPT = DSPRelease.VoyageReportCPT;

                //release.VoyageReportCPTRemark = DSPRelease.VoyageReportCPTRemark;
                release.PIFDSP = DSPRelease.PIFDSP;
                //release.PIFCPT = DSPRelease.PIFCPT;
                release.PIFDSPRemark = DSPRelease.PIFDSPRemark;
                //release.PIFCPTRemark = DSPRelease.PIFCPTRemark;
                release.GoodDeclarationDSP = DSPRelease.GoodDeclarationDSP;
                // release.GoodDeclarationCPT = DSPRelease.GoodDeclarationCPT;
                release.GoodDeclarationDSPRemark = DSPRelease.GoodDeclarationDSPRemark;
                //  release.GoodDeclarationCPTRemark = DSPRelease.GoodDeclarationCPTRemark;
                release.IPADDSP = DSPRelease.IPADDSP;
                //release.IPADCPT = DSPRelease.IPADCPT;
                release.IPADDSPRemark = DSPRelease.IPADDSPRemark;
                //release.IPADCPTRemark = DSPRelease.IPADCPTRemark;
                release.DateConfirmed = DateTime.Now; //DSPRelease.DateConfirmed;
                release.DispatcherId = DSPRelease.DispatcherId;


                release.VoyageReportDSPRemark = DSPRelease.User;


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


        public class DataResponse
        {
            public bool IsSuccess { get; set; }
            public object Data { get; set; }
            public List<string> Errors { get; set; }
        }

       
        public class EFBValueObj
        {
            public int id { get; set; }
            public int FlightId { get; set; }
            public string DateCreate { get; set; }
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
            public string TableName { get; set; }
            public string UserName { get; set; }


        }
        public class EFBValues
        {
            public List<EFBValueObj> Values { get; set; }
        }

        public class DSPReleaseViewModel
        {
            public int? FlightId { get; set; }
            public bool? ActualWXDSP { get; set; }
            public bool? ActualWXCPT { get; set; }
            public string ActualWXDSPRemark { get; set; }
            public string ActualWXCPTRemark { get; set; }
            public bool? WXForcastDSP { get; set; }
            public bool? WXForcastCPT { get; set; }
            public string WXForcastDSPRemark { get; set; }
            public string WXForcastCPTRemark { get; set; }
            public bool? SigxWXDSP { get; set; }
            public bool? SigxWXCPT { get; set; }
            public string SigxWXDSPRemark { get; set; }
            public string SigxWXCPTRemark { get; set; }
            public bool? WindChartDSP { get; set; }
            public bool? WindChartCPT { get; set; }
            public string WindChartDSPRemark { get; set; }
            public string WindChartCPTRemark { get; set; }
            public bool? NotamDSP { get; set; }
            public bool? NotamCPT { get; set; }
            public string NotamDSPRemark { get; set; }
            public string NotamCPTRemark { get; set; }
            public bool? ComputedFligthPlanDSP { get; set; }
            public bool? ComputedFligthPlanCPT { get; set; }
            public string ComputedFligthPlanDSPRemark { get; set; }
            public string ComputedFligthPlanCPTRemark { get; set; }
            public bool? ATCFlightPlanDSP { get; set; }
            public bool? ATCFlightPlanCPT { get; set; }
            public string ATCFlightPlanDSPRemark { get; set; }
            public string ATCFlightPlanCPTRemark { get; set; }
            public bool? PermissionsDSP { get; set; }
            public bool? PermissionsCPT { get; set; }
            public string PermissionsDSPRemark { get; set; }
            public string PermissionsCPTRemark { get; set; }
            public bool? JeppesenAirwayManualDSP { get; set; }
            public bool? JeppesenAirwayManualCPT { get; set; }
            public string JeppesenAirwayManualDSPRemark { get; set; }
            public string JeppesenAirwayManualCPTRemark { get; set; }
            public bool? MinFuelRequiredDSP { get; set; }
            public bool? MinFuelRequiredCPT { get; set; }
            public decimal? MinFuelRequiredCFP { get; set; }
            public decimal? MinFuelRequiredSFP { get; set; }
            public decimal? MinFuelRequiredPilotReq { get; set; }
            public bool? GeneralDeclarationDSP { get; set; }
            public bool? GeneralDeclarationCPT { get; set; }
            public string GeneralDeclarationDSPRemark { get; set; }
            public string GeneralDeclarationCPTRemark { get; set; }
            public bool? FlightReportDSP { get; set; }
            public bool? FlightReportCPT { get; set; }
            public string FlightReportDSPRemark { get; set; }
            public string FlightReportCPTRemark { get; set; }
            public bool? TOLndCardsDSP { get; set; }
            public bool? TOLndCardsCPT { get; set; }
            public string TOLndCardsDSPRemark { get; set; }
            public string TOLndCardsCPTRemark { get; set; }
            public bool? LoadSheetDSP { get; set; }
            public bool? LoadSheetCPT { get; set; }
            public string LoadSheetDSPRemark { get; set; }
            public string LoadSheetCPTRemark { get; set; }
            public bool? FlightSafetyReportDSP { get; set; }
            public bool? FlightSafetyReportCPT { get; set; }
            public string FlightSafetyReportDSPRemark { get; set; }
            public string FlightSafetyReportCPTRemark { get; set; }
            public bool? AVSECIncidentReportDSP { get; set; }
            public bool? AVSECIncidentReportCPT { get; set; }
            public string AVSECIncidentReportDSPRemark { get; set; }
            public string AVSECIncidentReportCPTRemark { get; set; }
            public bool? OperationEngineeringDSP { get; set; }
            public bool? OperationEngineeringCPT { get; set; }
            public string OperationEngineeringDSPRemark { get; set; }
            public string OperationEngineeringCPTRemark { get; set; }
            public bool? VoyageReportDSP { get; set; }
            public bool? VoyageReportCPT { get; set; }
            public string VoyageReportDSPRemark { get; set; }
            public string VoyageReportCPTRemark { get; set; }
            public bool? PIFDSP { get; set; }
            public bool? PIFCPT { get; set; }
            public string PIFDSPRemark { get; set; }
            public string PIFCPTRemark { get; set; }
            public bool? GoodDeclarationDSP { get; set; }
            public bool? GoodDeclarationCPT { get; set; }
            public string GoodDeclarationDSPRemark { get; set; }
            public string GoodDeclarationCPTRemark { get; set; }
            public bool? IPADDSP { get; set; }
            public bool? IPADCPT { get; set; }
            public string IPADDSPRemark { get; set; }
            public string IPADCPTRemark { get; set; }
            public DateTime? DateConfirmed { get; set; }
            public bool? ATSFlightPlanFOO { get; set; }
            public bool? ATSFlightPlanCMDR { get; set; }
            public string ATSFlightPlanFOORemark { get; set; }
            public string ATSFlightPlanCMDRRemark { get; set; }
            public bool? VldEFBFOO { get; set; }
            public bool? VldEFBCMDR { get; set; }
            public string VldEFBFOORemark { get; set; }
            public string VldEFBCMDRRemark { get; set; }
            public bool? VldFlightCrewFOO { get; set; }
            public bool? VldFlightCrewCMDR { get; set; }
            public string VldFlightCrewFOORemark { get; set; }
            public string VldFlightCrewCMDRRemark { get; set; }
            public bool? VldMedicalFOO { get; set; }
            public bool? VldMedicalCMDR { get; set; }
            public string VldMedicalFOORemark { get; set; }
            public string VldMedicalCMDRRemark { get; set; }
            public bool? VldPassportFOO { get; set; }
            public bool? VldPassportCMDR { get; set; }
            public string VldPassportFOORemark { get; set; }
            public string VldPassportCMDRRemark { get; set; }
            public bool? VldCMCFOO { get; set; }
            public bool? VldCMCCMDR { get; set; }
            public string VldCMCFOORemark { get; set; }
            public string VldCMCCMDRRemark { get; set; }
            public bool? VldRampPassFOO { get; set; }
            public bool? VldRampPassCMDR { get; set; }
            public string VldRampPassFOORemark { get; set; }
            public string VldRampPassCMDRRemark { get; set; }
            public bool? OperationalFlightPlanFOO { get; set; }
            public bool? OperationalFlightPlanCMDR { get; set; }
            public string OperationalFlightPlanFOORemark { get; set; }
            public string OperationalFlightPlanCMDRRemark { get; set; }
            public int? DispatcherId { get; set; }
            public int Id { get; set; }
            public string User { get; set; }
        }

        public class DSPReleaseViewModel2
        {
            public int? FlightId { get; set; }
            public bool? ActualWXDSP { get; set; }
            // public bool? ActualWXCPT { get; set; }
            public string ActualWXDSPRemark { get; set; }
            //public string ActualWXCPTRemark { get; set; }
            public bool? WXForcastDSP { get; set; }
            //public bool? WXForcastCPT { get; set; }
            public string WXForcastDSPRemark { get; set; }
            //public string WXForcastCPTRemark { get; set; }
            public bool? SigxWXDSP { get; set; }
            //public bool? SigxWXCPT { get; set; }
            public string SigxWXDSPRemark { get; set; }
            //public string SigxWXCPTRemark { get; set; }
            public bool? WindChartDSP { get; set; }
            //public bool? WindChartCPT { get; set; }
            public string WindChartDSPRemark { get; set; }
            //public string WindChartCPTRemark { get; set; }
            public bool? NotamDSP { get; set; }
            //public bool? NotamCPT { get; set; }
            public string NotamDSPRemark { get; set; }
            //public string NotamCPTRemark { get; set; }
            public bool? ComputedFligthPlanDSP { get; set; }
            //public bool? ComputedFligthPlanCPT { get; set; }
            public string ComputedFligthPlanDSPRemark { get; set; }
            //public string ComputedFligthPlanCPTRemark { get; set; }
            public bool? ATCFlightPlanDSP { get; set; }
            //public bool? ATCFlightPlanCPT { get; set; }
            public string ATCFlightPlanDSPRemark { get; set; }
            //public string ATCFlightPlanCPTRemark { get; set; }
            public bool? PermissionsDSP { get; set; }
            //public bool? PermissionsCPT { get; set; }
            public string PermissionsDSPRemark { get; set; }
            //public string PermissionsCPTRemark { get; set; }
            public bool? JeppesenAirwayManualDSP { get; set; }
            //public bool? JeppesenAirwayManualCPT { get; set; }
            public string JeppesenAirwayManualDSPRemark { get; set; }
            //public string JeppesenAirwayManualCPTRemark { get; set; }
            public bool? MinFuelRequiredDSP { get; set; }
            //public bool? MinFuelRequiredCPT { get; set; }
            //public decimal? MinFuelRequiredCFP { get; set; }
            //public decimal? MinFuelRequiredSFP { get; set; }
            //public decimal? MinFuelRequiredPilotReq { get; set; }
            public bool? GeneralDeclarationDSP { get; set; }
            //public bool? GeneralDeclarationCPT { get; set; }
            public string GeneralDeclarationDSPRemark { get; set; }
            //public string GeneralDeclarationCPTRemark { get; set; }
            public bool? FlightReportDSP { get; set; }
            //public bool? FlightReportCPT { get; set; }
            public string FlightReportDSPRemark { get; set; }
            //public string FlightReportCPTRemark { get; set; }
            public bool? TOLndCardsDSP { get; set; }
            //public bool? TOLndCardsCPT { get; set; }
            public string TOLndCardsDSPRemark { get; set; }
            //public string TOLndCardsCPTRemark { get; set; }
            public bool? LoadSheetDSP { get; set; }
            //public bool? LoadSheetCPT { get; set; }
            public string LoadSheetDSPRemark { get; set; }
            //public string LoadSheetCPTRemark { get; set; }
            public bool? FlightSafetyReportDSP { get; set; }
            //public bool? FlightSafetyReportCPT { get; set; }
            public string FlightSafetyReportDSPRemark { get; set; }
            //public string FlightSafetyReportCPTRemark { get; set; }
            public bool? AVSECIncidentReportDSP { get; set; }
            //public bool? AVSECIncidentReportCPT { get; set; }
            public string AVSECIncidentReportDSPRemark { get; set; }
            //public string AVSECIncidentReportCPTRemark { get; set; }
            public bool? OperationEngineeringDSP { get; set; }
            //public bool? OperationEngineeringCPT { get; set; }
            public string OperationEngineeringDSPRemark { get; set; }
            //public string OperationEngineeringCPTRemark { get; set; }
            public bool? VoyageReportDSP { get; set; }
            //public bool? VoyageReportCPT { get; set; }
            public string VoyageReportDSPRemark { get; set; }
            //public string VoyageReportCPTRemark { get; set; }
            public bool? PIFDSP { get; set; }
            //public bool? PIFCPT { get; set; }
            public string PIFDSPRemark { get; set; }
            //public string PIFCPTRemark { get; set; }
            public bool? GoodDeclarationDSP { get; set; }
            //public bool? GoodDeclarationCPT { get; set; }
            public string GoodDeclarationDSPRemark { get; set; }
            //public string GoodDeclarationCPTRemark { get; set; }
            public bool? IPADDSP { get; set; }
            //public bool? IPADCPT { get; set; }
            public string IPADDSPRemark { get; set; }
            //public string IPADCPTRemark { get; set; }
            //public DateTime? DateConfirmed { get; set; }
            public int? DispatcherId { get; set; }
            public int Id { get; set; }
            public string User { get; set; }
        }
    }
}
