using ApiQA.Models;
using ApiQA.ViewModels;
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

namespace ApiQA.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QaController : ApiController
    {



        //[Route("api/efb/dr/{flightId}")]
        //public async Task<IHttpActionResult> GetDRByFlightId(int flightId)
        //{
        //    var _context = new ppa_entities();

        //}

        ppa_entities context = new ppa_entities();

        [HttpGet]
        [Route("api/get/csr/flightphase")]
        public async Task<DataResponse> CSRFlightPhase()
        {

            //var result = context.QAOptions.Where(q => q.ParentId == 7);
            var query = from x in context.QAOptions
                        where x.ParentId == 7
                        select new
                        {
                            Title = x.Title,
                            Id = x.Id
                        };
            return new DataResponse()
            {
                Data = query,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/csr/eventtitle")]
        public async Task<DataResponse> CSREventTitle()
        {
            var query = from x in context.QAOptions
                        where x.ParentId == 16
                        select new
                        {
                            Title = x.Title,
                            Id = x.Id
                        };
            return new DataResponse()
            {
                Data = query,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/gia/dmgby")]
        public async Task<DataResponse> GIADamageBy()
        {
            var query = from x in context.QAOptions
                        where x.ParentId == 68
                        select new
                        {
                            Title = x.Title,
                            Id = x.Id
                        };
            return new DataResponse()
            {
                Data = query,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/gia/lighting")]
        public async Task<DataResponse> GetGAILighting()
        {
            var query = from x in context.QAOptions
                        where x.ParentId == 62
                        select new
                        {
                            Title = x.Title,
                            Id = x.Id
                        };
            return new DataResponse()
            {
                Data = query,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/gia/surface")]
        public async Task<DataResponse> GetGIASurface()
        {
            var query = from x in context.QAOptions
                        where x.ParentId == 54
                        select new
                        {
                            Title = x.Title,
                            Id = x.Id
                        };

            return new DataResponse()
            {
                Data = query,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/gia/weather")]
        public async Task<DataResponse> GetGIAWeather()
        {
            var query = from x in context.QAOptions
                        where x.ParentId == 48
                        select new
                        {
                            Title = x.Title,
                            Id = x.Id
                        };
            return new DataResponse()
            {
                Data = query,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/mor/compnspec")]
        public async Task<DataResponse> GetMORComponentSpec()
        {
            var query = from x in context.QAOptions
                        where x.ParentId == 43
                        select new
                        {
                            x.Id,
                            x.Title
                        };
            return new DataResponse()
            {
                Data = query,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/flightinformation/{flightId}")]
        public async Task<DataResponse> GetFlightInformation(int flightId)
        {

            var result = context.AppLegs.SingleOrDefault(q => q.FlightId == flightId);
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }


        [HttpGet]
        [Route("api/get/csr/{FlightId}/{EmployeeId}")]
        public async Task<DataResponse> GetCSRByFlightId(int FlightId, int EmployeeId)
        {
            //var result = context.ViewQACSRs.SingleOrDefault(q => q.FlightId == FlightId);
            var result = context.QACSRGet(EmployeeId, FlightId).Single();
            var csrEvent = context.ViewQACSREvents.Where(q => q.QACSRId == result.Id).ToList();
            return new DataResponse()
            {
                Data = new { result = result, CSREvent = csrEvent },
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/csr/byid/{Id}")]
        public async Task<DataResponse> GetCSRById(int Id)
        {
            try
            {
                var result = context.ViewQACSRs.SingleOrDefault(q => q.Id == Id);
                var csrEvent = context.ViewQACSREvents.Where(q => q.QACSRId == result.Id).ToList();
                return new DataResponse()
                {
                    Data = new { result = result, CSREvent = csrEvent },
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }


        [HttpGet]
        [Route("api/get/mor/{employeeId}/{flightId}")]
        public async Task<DataResponse> GetMORByFlightId(int employeeId, int flightId)
        {
            try
            {
                var result = context.QAMaintenanceGet(employeeId, flightId).Single();
                return new DataResponse()
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpGet]
        [Route("api/get/mor/byid/{Id}")]
        public async Task<DataResponse> GetMORById(int Id)
        {
            var result = context.ViewQAMaintenances.SingleOrDefault(q => q.Id == Id);
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/gia/{employeeId}/{flightId}")]
        public async Task<DataResponse> GetGIAByFlightId(int employeeId, int flightId)
        {
            var result = context.QAGroundGet(employeeId, flightId).Single();
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/gia/byid/{Id}")]
        public async Task<DataResponse> GetGIAById(int Id)
        {
            var result = context.ViewQAGrounds.SingleOrDefault(q => q.Id == Id);
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/vhr/{Id}")]
        public async Task<DataResponse> GetVHRById(int Id)
        {
            var result = context.ViewQAHazards.SingleOrDefault(q => q.Id == Id);
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }



        [HttpPost]
        [Route("api/save/csr")]
        public async Task<DataResponse> SaveQACSR(dynamic dto)
        {

            try
            {
                int Id = dto.Id;
                string Date = dto.OccurrenceDateTime.ToString();
                var entity = context.QACSRs.SingleOrDefault(q => q.Id == Id);
                if (entity == null)
                {
                    entity = new QACSR();
                    context.QACSRs.Add(entity);
                }



                entity.FlightPhaseId = dto.FlightPhaseId;
                entity.FlightId = dto.FlightId;
                entity.Describtion = dto.Describtion;
                entity.EventLocation = dto.EventLocation;
                entity.ReportFiledBy = dto.ReportFiledBy;
                entity.OccurrenceDateTime = DateTime.Parse(Date);
                entity.Recommendation = dto.Recommendation;
                entity.WeatherCondition = dto.WeatherCondition;
                entity.Name = dto.Name;
                entity.BOX = dto.BOX;
                entity.RefNumber = dto.RefNumber;
                entity.FollowUp = dto.FollowUp;
                entity.Recived = dto.Recived;
                entity.EmployeeId = dto.EmploeeId;
                entity.EventTitleRemark = dto.EventTitleRemark;
                foreach (var x in dto.EventTitleIds)
                {
                    entity.QACSREvents.Add(new QACSREvent()
                    {
                        EventTitleId = x
                    });
                };

                context.SaveChanges();

                return new DataResponse()
                {
                    Data = null,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex.InnerException,
                    IsSuccess = false
                };
            }
        }

        [HttpPost]
        [Route("api/save/mor")]
        public async Task<DataResponse> SaveMOR(dynamic dto)
        {
            try
            {

                int Id = dto.Id;
                var entity = context.QAMaintenances.SingleOrDefault(q => q.Id == Id);
                if (entity == null)
                {
                    entity = new QAMaintenance();
                    context.QAMaintenances.Add(entity);
                }

                entity.OccurrenceDateTime = dto.OccurrenceDateTime;
                entity.ComponentSpecificationId = dto.ComponentSpecificationId;
                entity.ATLNo = dto.ATLNo;
                entity.TaskNo = dto.TaskNo;
                entity.Reference = dto.Reference;
                entity.FlightId = dto.FlightId;
                entity.EmployeeId = dto.EmploeeId;
                entity.StationId = dto.StationId;
                entity.EventDescription = dto.EventDescription;
                entity.ActionTakenDescription = dto.ActionTakenDescription;
                entity.Name = dto.Name;
                entity.CAALicenceNo = dto.CAALicenceNo;
                entity.AuthorizationNo = dto.AuthorizationNo;
                entity.SerialNumber = dto.SerialNumber;
                entity.PartNumber = dto.PartNumber;
                context.SaveChanges();
                return new DataResponse()
                {
                    Data = null,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex.InnerException,
                    IsSuccess = false
                };
            }

        }

        [HttpPost]
        [Route("api/save/vhr")]
        public async Task<DataResponse> SaveVHR(dynamic dto)
        {

            try
            {
                int Id = dto.Id;
                var entity = context.QAHazards.SingleOrDefault(q => q.Id == Id);
                if (entity == null)
                {
                    entity = new QAHazard();
                    context.QAHazards.Add(entity);
                }
                entity.Name = dto.Name;
                entity.Email = dto.Email;
                entity.TelNumber = dto.TelNumber;
                entity.ReportDate = dto.ReportDate;
                entity.AffectedArea = dto.AffectedArea;
                entity.HazardDate = dto.HazardDate;
                entity.HazardDescription = dto.HazardDescription;
                entity.RecommendedAction = dto.RecommendedAction;
                entity.EmployeeId = dto.EmploeeId;

                context.SaveChanges();
                return new DataResponse()
                {
                    Data = entity,
                    IsSuccess = false
                };

            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex.InnerException,
                    IsSuccess = false
                };
            }



        }

        [HttpPost]
        [Route("api/save/gia")]
        public async Task<DataResponse> SaveGIA(dynamic dto)
        {

            try
            {
                int Id = dto.Id;
                var entity = context.QAGroundIADs.SingleOrDefault(q => q.Id == Id);
                if (entity == null)
                {
                    entity = new QAGroundIAD();
                    context.QAGroundIADs.Add(entity);
                }
                entity.Airport = dto.Airport;
                entity.AirportId = dto.AirportId;
                entity.Area = dto.Area;
                entity.ContributoryFactors = dto.ContributoryFactors;
                entity.CorrectiveActionTaken = dto.CorrectiveActionTaken;
                entity.DamageById = dto.DamageById;
                entity.DamageDate = dto.DamageDate;
                entity.DamageDetails = dto.DamageDetails;
                //entity.DateSigne
                entity.EmployeeId = dto.EmploeeId;
                entity.CorrectiveActionTaken = dto.CorrectiveActionTaken;
                entity.EmployeesNonFatalityNr = dto.EmployeesNonFatalityNr;
                entity.EmployeesFatalityNr = dto.EmployeesFatalityNr;
                entity.Event = dto.Event;
                entity.FlightId = dto.FlightId;
                //entity.FlightInformation
                entity.OperationPhase = dto.OperationPhase;
                entity.OthersFatalityNr = dto.OthersFatalityNr;
                entity.OthersNonFatalityNr = dto.OthersNonFatalityNr;
                entity.OtherSuggestions = dto.OtherSuggestions;
                entity.PassengersFatalityNr = dto.PassengersFatalityNr;
                entity.PassengersNonFatalityNr = dto.PassengersNonFatalityNr;
                entity.PersonnelCompany1 = dto.PersonnelCompany1;
                entity.PersonnelJobTitle1 = dto.PersonnelJobTitle1;
                entity.PersonnelLicense1 = dto.PersonnelLicense1;
                entity.PersonnelName1 = dto.PersonnelName1;
                entity.PersonnelStaffNr1 = dto.PersonnelStaffNr1;
                entity.PersonnelCompany2 = dto.PersonnelCompany2;
                entity.PersonnelJobTitle2 = dto.PersonnelJobTitle2;
                entity.PersonnelLicense2 = dto.PersonnelLicense2;
                entity.PersonnelName2 = dto.PersonnelName2;
                entity.PersonnelStaffNr2 = dto.PersonnelStaffNr2;
                entity.PersonnelCompany3 = dto.PersonnelCompany3;
                entity.PersonnelJobTitle3 = dto.PersonnelJobTitle3;
                entity.PersonnelLicense3 = dto.PersonnelLicense3;
                entity.PersonnelName3 = dto.PersonnelName3;
                entity.PersonnelStaffNr3 = dto.PersonnelStaffNr3;
                entity.ScheduledGroundTime = dto.ScheduledGroundTime;
                //entity.Title
                entity.VEAge = dto.VEAge;
                entity.VEArea = dto.VEArea;
                entity.VEBrakesCon = dto.VEBrakesCon;
                entity.VEFieldofVisionCon = dto.VEFieldofVisionCon;
                entity.VEFromDrivingPoCon = dto.VEFromDrivingPoCon;
                entity.VELastOverhaul = dto.VELastOverhaul;
                entity.VELightsCon = dto.VELightsCon;
                entity.VEOwner = dto.VEOwner;
                entity.VEProtectionCon = dto.VEProtectionCon;
                entity.VERemarks = dto.VERemarks;
                entity.VESerialFleetNr = dto.VESerialFleetNr;
                entity.VEStabilizersCon = dto.VEStabilizersCon;
                entity.VESteeringCon = dto.VESteeringCon;
                entity.VETowHitchCon = dto.VETowHitchCon;
                entity.VEType = dto.VEType;
                entity.VETyresCon = dto.VETyresCon;
                entity.VEWarningDevicesCon = dto.VEWarningDevicesCon;
                entity.VEWipersCon = dto.VEWipersCon;
                entity.WXLightingId = dto.WXLightingId;
                entity.WXSurfaceId = dto.WXSurfaceId;
                entity.WXTemperature = dto.WXTemperature;
                entity.WXVisibilityM = dto.WXVisibilityM;
                entity.WXVisibilityKM = dto.WXVisibilityKM;
                entity.WXWeatherId = dto.WXWeatherId;
                entity.WXWind = dto.WXWind;
                entity.DamageRemark = dto.DamageRemark;
                context.SaveChanges();
                return new DataResponse()
                {
                    Data = null,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex.InnerException,
                    IsSuccess = false
                };
            }
        }


        [HttpGet]
        [Route("api/get/chr/reason")]
        public async Task<DataResponse> GetCHRReason()
        {
            var result = from x in context.QAOptions
                         where x.ParentId == 78
                         select new
                         {
                             Title = x.Title,
                             Id = x.Id
                         };

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/chr/{employeeId}/{flightId}")]
        public async Task<DataResponse> GetCHRByFlightId(int employeeId, int flightId)
        {
            try
            {
                var result = context.QACateringGet(employeeId, flightId).Single();
                return new DataResponse()
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpGet]
        [Route("api/get/chr/byid/[id]")]
        public async Task<DataResponse> GetCHRById(int id)
        {
            var result = context.ViewQACaterings.SingleOrDefault(q => q.Id == id);
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpPost]
        [Route("api/save/chr")]
        public async Task<DataResponse> SaveCHR(dynamic dto)
        {
            try
            {

                int Id = dto.Id;
                var entity = context.QACaterings.SingleOrDefault(q => q.Id == Id);
                if (entity == null)
                {
                    entity = new QACatering();
                    context.QACaterings.Add(entity);
                }

                entity.DateReport = dto.DateReport;
                entity.DateSign = dto.DateSign;
                entity.DateHazard = dto.DateHazard;
                entity.Description = dto.Description;
                entity.Equipment = dto.Equipment;
                entity.EmployeeId = dto.EmploeeId;
                entity.InjuryDescription = dto.InjuryDescription;
                entity.Place = dto.Place;
                entity.PreventiveActions = dto.PreventiveActions;
                entity.ReasonDescription = dto.ReasonDescription;
                entity.ReasonId = dto.ReasonId;
                entity.SaftyEquipmentType = dto.SaftyEquipmentType;
                entity.SaftyEquipmentUseage = dto.SaftyEquipmentUseage;
                entity.Transporter = dto.Transporter;
                entity.Trolley = dto.Trolley;
                entity.TrolleyEquipmentTransporterDecs = dto.TrolleyEquipmentTransporterDecs;
                entity.WorkBreak = dto.WorkBreak;
                entity.WorkBreakPeriod = dto.WorkBreakPeriod;
                entity.FlightId = dto.FlightId;

                var saveChanges = await context.SaveChangesAsync();
                dto.Id = entity.Id;
                return new DataResponse()
                {
                    Data = dto,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex.InnerException,
                    IsSuccess = false
                };
            }
        }



        [HttpGet]
        [Route("api/get/shr/reason")]
        public async Task<DataResponse> GetSHRReason()
        {
            var result = from x in context.QAOptions
                         where x.ParentId == 92
                         select new
                         {
                             Title = x.Title,
                             Id = x.Id
                         };

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/shr/{employeeId}/{flightId}")]
        public async Task<DataResponse> GetSHRByFlightId(int employeeId, int flightId)
        {
            var result = context.QASecurityGet(employeeId, flightId).Single();
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/shr/byid/[id]")]
        public async Task<DataResponse> GetSHRById(int id)
        {
            var result = context.ViewQASecurities.SingleOrDefault(q => q.Id == id);
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpPost]
        [Route("api/save/shr")]
        public async Task<DataResponse> SaveSHR(dynamic dto)
        {
            try
            {

                int Id = dto.Id;
                var entity = context.QASecurities.SingleOrDefault(q => q.Id == Id);
                if (entity == null)
                {
                    entity = new QASecurity();
                    context.QASecurities.Add(entity);
                }

                entity.DateReport = dto.DateReport;
                entity.DateSign = dto.DateSign;
                entity.DateTimeHazard = dto.DateTimeHazard;
                entity.Description = dto.Description;
                entity.Camera = dto.Camera;
                entity.CarryingBox = dto.CarryingBox;
                entity.InjuryDescription = dto.InjuryDescription;
                entity.Place = dto.Place;
                entity.PreventiveActions = dto.PreventiveActions;
                entity.ReasonDescription = dto.ReasonDescription;
                entity.ReasonId = dto.ReasonId;
                entity.EquipmentDescription = dto.EquipmentDescription;
                entity.WorkBreak = dto.WorkBreak;
                entity.WorkBreakPeriod = dto.WorkBreakPeriod;
                entity.FlightId = dto.FlightId;
                entity.Comail = dto.Comail;
                entity.HandRocket = dto.HandRocket;
                entity.InjuryOccuring = dto.InjuryOccuring;
                entity.Other = dto.Other;
                entity.EmployeeId = dto.EmployeeId;



                var saveChanges = await context.SaveChangesAsync();
                dto.Id = entity.Id;
                return new DataResponse()
                {
                    Data = dto,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex.InnerException,
                    IsSuccess = false
                };
            }
        }







        [HttpGet]
        [Route("api/get/opcatagory")]
        public async Task<DataResponse> GetDispatchOPCatagory()
        {
            var result = from x in context.QAOptions
                         where x.ParentId == 104
                         select new
                         {
                             Title = x.Title,
                             Id = x.Id
                         };

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpGet]
        [Route("api/get/discatagory")]
        public async Task<DataResponse> GetDispatchDISCatagory()
        {
            var result = from x in context.QAOptions
                         where x.ParentId == 112
                         select new
                         {
                             Title = x.Title,
                             Id = x.Id
                         };

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }


        [HttpGet]
        [Route("api/get/dhr/{employeeId}/{flightId}")]
        public async Task<DataResponse> GetDHRByFlightId(int employeeId, int flightId)
        {
            try
            {
                var result = context.QADispatchGet(employeeId, flightId).Single();
                return new DataResponse()
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse() { Data = ex, IsSuccess = false };

            }
        }

        [HttpGet]
        [Route("api/get/dhr/byid/[id]")]
        public async Task<DataResponse> GetDHRById(int id)
        {
            var result = context.ViewQADispatches.SingleOrDefault(q => q.Id == id);
            return new DataResponse()
            {
                Data = result,
                IsSuccess = true
            };
        }

        [HttpPost]
        [Route("api/save/dhr")]
        public async Task<DataResponse> SaveDHR(dynamic dto)
        {
            try
            {

                int Id = dto.Id;
                var entity = context.QADispatches.SingleOrDefault(q => q.Id == Id);
                if (entity == null)
                {
                    entity = new QADispatch();
                    context.QADispatches.Add(entity);
                }

                entity.DateReport = dto.DateReport;
                entity.DISActionResult = dto.DISActionResult;
                entity.DISCatagoryId = dto.DISCatagoryId;
                entity.DISDateTimeEvent = dto.DISDateTimeEvent;
                entity.DISLocation = dto.DISLocation;
                entity.DISTimeDuration = dto.DISTimeDuration;
                entity.FlightId = dto.FlightId;
                entity.OPCatagoryId = dto.OPCatagoryId;
                entity.OPDateTimeEvent = dto.OPDateTimeEvent;
                entity.OPLocation = dto.OPLocation;
                entity.OPReportedBy = dto.OPReportedBy;
                entity.OPSummary = dto.OPSummary;
                entity.OPTimeReceived = dto.OPTimeReceived;
                entity.Remarks = dto.Remarks;
                entity.Type = dto.Type;
                entity.EmployeeId = dto.EmployeeId;



                var saveChanges = await context.SaveChangesAsync();
                dto.Id = entity.Id;
                return new DataResponse()
                {
                    Data = dto,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex.InnerException,
                    IsSuccess = false
                };
            }
        }


        [HttpPost]
        [Route("api/save/followup")]
        public async Task<DataResponse> SaveFollowUp(dynamic dto)
        {

            try
            {
                //int Id = dto.Id;
                //var entity = context.QAFollowingUps.SingleOrDefault(q => q.Id == Id);
                //if (entity == null)
                //{
                //    entity = new QAFollowingUp();
                //    context.QAFollowingUps.Add(entity);
                //}

                int Type = dto.Type;
                var respEmployee = context.QAResponsibilties.Where(q => q.Type == Type).ToList();

                foreach (var x in respEmployee)
                {

                    var entity = new QAFollowingUp();
                    context.QAFollowingUps.Add(entity);

                    entity.EntityId = dto.EntityId;
                    entity.DateConfirmation = dto.DateConfirmation;
                    entity.DateReferr = dto.DateReferr;
                    entity.Type = dto.Type;
                    entity.ReferredId = x.ReceiverEmployeeId;
                    entity.ReferrerId = dto.ReferrerId;

                };


                context.SaveChanges();
                return new DataResponse()
                {
                    Data = dto,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpGet]
        [Route("api/get/qa/{employeeId}")]
        public async Task<DataResponse> GetQAByEmployee(int employeeId)
        {

            try
            {

                var query = from e in context.ViewQAByEmployeeCounts
                            where e.EmployeeId == employeeId
                            group e by new { e.TypeTitle } into grp
                            select new
                            {
                                grp.Key.TypeTitle,
                                ReferredCount = grp.Sum(q => q.ReferredCount),
                                ConfirmedCount = grp.Sum(q => q.ConfirmedCount),
                                NotDeterminedCount = grp.Sum(q => q.NotDeterminedCount)
                            };

                var result = query.ToList();
                //var result = new
                //{
                //    Type = ds.Single(q => q.Type),
                //    Status = ds.Count(q => q.Status),
                //};
                return new DataResponse()
                {
                    Data = result,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }


        [HttpPost]
        [Route("api/get/qa/status")]
        public async Task<DataResponse> GetQAStatus(dynamic dto)
        {
            try
            {
                dynamic result = null;
                DateTime df = dto.dt_from;
                DateTime dt = dto.dt_to;
                int employeeId = dto.employeeId;
                int type = dto.type;
                //var result = new { };
                //var result = new { Confirmed, };
                switch (type)
                {
                    case 0:
                        var q0 = from byEmp in context.ViewQABYEmployees
                                 join cabin in context.ViewQACSRs on byEmp.EntityId equals cabin.Id
                                 where byEmp.EmployeeId == employeeId && cabin.OccurrenceDateTime >= df && cabin.OccurrenceDateTime <= dt
                                 select new
                                 {
                                     Status = byEmp.Status,
                                     Date = cabin.OccurrenceDateTime,
                                     EmployeeName = cabin.EmployeeName,
                                     Id = cabin.Id
                                 };
                        var ds = q0.ToList();
                        result = new
                        {
                            Confirmed = ds.Where(q => q.Status == "Confirmed"),
                            NotDetermined = ds.Where(q => q.Status == "notDetermined"),
                            Referred = ds.Where(q => q.Status == "Referred"),
                        };


                        break;
                    case 1:
                        var q1 = from byEmp in context.ViewQABYEmployees
                                 join ground in context.ViewQAGrounds on byEmp.EntityId equals ground.Id
                                 where byEmp.EmployeeId == employeeId && ground.DamageDate >= df && ground.DamageDate <= dt
                                 select new
                                 {
                                     Status = byEmp.Status,
                                     Date = ground.DamageDate,
                                     EmployeeName = ground.EmployeeName,
                                 };
                        result = q1.ToList();
                        break;
                    case 2:
                        var q2 = from byEmp in context.ViewQABYEmployees
                                 join hazard in context.ViewQAHazards on byEmp.EntityId equals hazard.Id
                                 where byEmp.EmployeeId == employeeId && hazard.HazardDate >= df && hazard.HazardDate <= dt
                                 select new
                                 {
                                     Status = byEmp.Status,
                                     Date = hazard.HazardDate,
                                     EmployeeName = hazard.EmployeeName,
                                 };
                        var ds2 = q2.ToList();
                        result = new
                        {
                            Confirmed = ds2.Where(q => q.Status == "Confirmed"),
                            NotDetermined = ds2.Where(q => q.Status == "notDetermined"),
                            Referred = ds2.Where(q => q.Status == "Referred"),
                        };
                        break;
                    case 3:
                        var q3 = from byEmp in context.ViewQABYEmployees
                                 join maintenance in context.ViewQAMaintenances on byEmp.EntityId equals maintenance.Id
                                 where byEmp.EmployeeId == employeeId && maintenance.OccurrenceDateTime >= df && maintenance.OccurrenceDateTime <= dt
                                 select new
                                 {
                                     Status = byEmp.Status,
                                     Date = maintenance.OccurrenceDateTime,
                                     EmployeeName = maintenance.EmployeeName,
                                 };
                        var ds3 = q3.ToList();
                        result = new
                        {
                            Confirmed = ds3.Where(q => q.Status == "Confirmed"),
                            NotDetermined = ds3.Where(q => q.Status == "notDetermined"),
                            Referred = ds3.Where(q => q.Status == "Referred"),
                        };
                        break;
                    case 4:
                        var q4 = from byEmp in context.ViewQABYEmployees
                                 join catering in context.ViewQACaterings on byEmp.EntityId equals catering.Id
                                 where byEmp.EmployeeId == employeeId && catering.DateHazard >= df && catering.DateHazard <= dt
                                 select new
                                 {
                                     Status = byEmp.Status,
                                     Date = catering.DateHazard,
                                     EmployeeName = catering.EmployeeName,
                                 };
                        var ds4 = q4.ToList();
                        result = new
                        {
                            Confirmed = ds4.Where(q => q.Status == "Confirmed"),
                            NotDetermined = ds4.Where(q => q.Status == "notDetermined"),
                            Referred = ds4.Where(q => q.Status == "Referred"),
                        };
                        break;
                    case 5:
                        var q5 = from byEmp in context.ViewQABYEmployees
                                 join security in context.ViewQASecurities on byEmp.EntityId equals security.Id
                                 where byEmp.EmployeeId == employeeId && security.DateReport >= df && security.DateReport <= dt
                                 select new
                                 {
                                     Status = byEmp.Status,
                                     Date = security.DateReport,
                                     EmployeeName = security.EmployeeName,
                                 };
                        var ds5 = q5.ToList();
                        result = new
                        {
                            Confirmed = ds5.Where(q => q.Status == "Confirmed"),
                            NotDetermined = ds5.Where(q => q.Status == "notDetermined"),
                            Referred = ds5.Where(q => q.Status == "Referred"),
                        };
                        break;
                }



                //var entity = context.ViewQABYEmployees.Where(q => q.EmploeeId == employeeId).ToList();


                return new DataResponse()
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }


        [HttpGet]
        [Route("api/qa/confirm/report/{employeeId}/{type}/{entityId}")]
        public async Task<DataResponse> ConfirmReport(int employeeId, int type, int entityId)
        {
            try
            {
                var entity = new QAFollowingUp();
                context.QAFollowingUps.Add(entity);
                entity.Type = type;
                entity.ReferredId = employeeId;
                entity.ReferrerId = employeeId;
                entity.Confirmation = true;
                entity.DateConfirmation = new DateTime?();
                entity.DateReferr = new DateTime?();
                entity.EntityId = entityId;
                context.SaveChanges();

                return new DataResponse()
                {
                    Data = entity,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }

        [HttpGet]
        [Route("api/get/followup/{entityId}/{type}")]
        public async Task<DataResponse> FollowUpReport(int entityId, int type)
        {
            try
            {
                var entity = context.ViewQAFollowingUps.Where(q => q.EntityId == entityId && q.Type == type).ToList();
                return new DataResponse()
                {
                    Data = entity,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new DataResponse()
                {
                    Data = ex,
                    IsSuccess = false
                };
            }
        }


        public class DataResponse
        {
            public bool IsSuccess { get; set; }
            public object Data { get; set; }
            public List<string> Errors { get; set; }
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


    }
}
