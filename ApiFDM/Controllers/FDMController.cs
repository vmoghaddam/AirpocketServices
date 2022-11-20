using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiFDM.Objects;
using ApiFDM.Models;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace ApiFDM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FDMController : ApiController
    {
        dbEntities context = new dbEntities();

        [HttpGet]
        [Route("api/get/fdm/{yf}/{yt}")]
        public DataResponse GetFDM(int yf, int yt)
        {
            // var context = new dbEntities();
            //sdfsdf
            List<int> years = new List<int>();
            List<ViewFDM> result = new List<ViewFDM>();

            years.Add(yt);
            for (int i = 0; yf != yt; i++)
            {
                years.Add(yf);
                yf++;
            }

            foreach (var year in years)
            {
                result.AddRange(context.ViewFDMs.Where(q => q.Date.Value.Year == year).ToList());

            };

            var Boeing = result.Where(q => q.AircraftType.Contains("B")).ToList();
            var MD = result.Where(q => q.AircraftType.Contains("MD")).ToList();
            var newRecord = result.Where(q => q.Removed == false && q.Approved == false).ToList();
            var confirmed = result.Where(q => q.Approved == true).ToList();
            var removed = result.Where(q => q.Removed == true).ToList();


            if (result != null)
            {
                return new DataResponse
                {
                    IsSuccess = true,
                    Data = new
                    {
                        newRecord,
                        confirmed,
                        removed,
                        Boeing,
                        MD
                    }
                };
            }
            else
            {
                return new DataResponse
                {
                    IsSuccess = false,
                    Data = null
                };
            }
        }

        [HttpPost]
        [Route("api/remove/fdm/{rowId}")]
        public async Task<DataResponse> RemoveFDM(int rowId)
        {
            try
            {
                var record = context.FDMs.Single(q => q.Id == rowId);
                record.Removed = true;
                record.Approved = false;
                var result = context.ViewFDMs.Single(q => q.Id == rowId);
                await context.SaveChangesAsync();
                return new DataResponse()
                {
                    IsSuccess = true,
                    Data = result,

                };
            }
            catch (Exception ex)
            {

                return new DataResponse()
                {
                    IsSuccess = false,
                    Data = null
                };
            }

        }

        [HttpPost]
        [Route("api/confirm/fdm/{rowID}")]
        public async Task<DataResponse> confirmFDM(int rowId)
        {
            try
            {
                var record = context.FDMs.Single(q => q.Id == rowId);
                record.Approved = true;
                record.Removed = false;
                var result = context.ViewFDMs.Single(q => q.Id == rowId);
                context.SaveChangesAsync();
                return new DataResponse()
                {
                    IsSuccess = true,
                    Data = result
                };

            }
            catch
            {
                return new DataResponse()
                {
                    IsSuccess = false,
                    Data = null
                };

            }

        }

        [HttpPost]
        [Route("api/delete/fdm/{rowId}")]
        public async Task<DataResponse> DeleteFDM(int rowId)
        {
            var viewRecord = context.ViewFDMs.Single(q => q.Id == rowId);
            if (viewRecord.Status != "Removed" || viewRecord.Status != "Confirmed")
            {
                var record = context.FDMs.Single(q => q.Id == viewRecord.Id);
                context.FDMs.Remove(record);
                context.SaveChangesAsync();
                return new DataResponse()
                {
                    IsSuccess = true,
                    Data = viewRecord
                };
            }
            else
            {
                return new DataResponse()
                {
                    IsSuccess = false,
                    Data = viewRecord.Status
                };
            }
        }


        [HttpGet]
        [Route("api/fdm/dashboard/type/{df}/{dt}")]
        public async Task<DataResponse> GetFDMTypeDaily(DateTime df, DateTime dt)
        {
            var query = from x in context.FDMTypeDailies
                        where x.FlightDate <= dt.Date && x.FlightDate >= df.Date
                        group x by new { x.AircraftType, x.AircraftTypeId } into grp
                        select new
                        {
                            grp.Key.AircraftTypeId,
                            grp.Key.AircraftType,
                            FlightCount = grp.Sum(q => q.flightcount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.flightcount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.flightcount) * 100,

                        };

            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFlights = ds.Sum(q => q.FlightCount),
                TotalHighLevel = ds.Sum(q => q.HighCount),
                TotalMediumLevel = ds.Sum(q => q.MediumCount),
                TotalLowLevel = ds.Sum(q => q.LowCount),
            };

            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/eventname/{df}/{dt}")]
        public async Task<DataResponse> GetAllFDMEventName(DateTime df, DateTime dt)
        {
            var query = from x in context.FDMCptEventDailies
                        where x.Day >= df && x.Day <= dt
                        group x by new { x.EventName } into grp
                        select new
                        {
                            grp.Key.EventName,
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            //  ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,

                        };


            var ds = query.OrderByDescending(q => q.IncidentCount);
            var result = new
            {
                data = ds,
                ///////TotalFlights = ds.Sum(q => q.FlightCount),
                TotalHighLevels = ds.Sum(q => q.HighLevelCount),
                TotalMediumLevels = ds.Sum(q => q.MediumLevelCount),
                TotalLowLevels = ds.Sum(q => q.LowLevelCount),

            };


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };

        }


        [HttpGet]
        [Route("api/fdm/dashboard/type/monthly/{df}/{dt}")]
        public async Task<DataResponse> GetAllFDMTypeMonthly(DateTime df, DateTime dt)
        {
            var query = from x in context.FDMTypeMonthlies
                        where x.Month >= df.Month && x.Year >= df.Year && x.Month <= dt.Month && x.Year <= dt.Year
                        group x by new { x.AircraftType, x.AircraftTypeId, x.Month, x.Year } into grp
                        select new
                        {
                            grp.Key.AircraftTypeId,
                            grp.Key.AircraftType,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            grp.Key.Month,
                            grp.Key.Year,
                        };


            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFlights = ds.Sum(q => q.FlightCount),
                TotalHighLevel = ds.Sum(q => q.HighLevelCount),
                TotalMediumLevel = ds.Sum(q => q.MediumLevelCount),
                TotalLowLevel = ds.Sum(q => q.LowLevelCount),
            };


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/register/{df}/{dt}")]
        public async Task<DataResponse> GetFDMRegDaily(DateTime df, DateTime dt)
        {
            var query = from x in context.FDMRegDailies
                        where x.FlightDate <= dt.Date && x.FlightDate >= df.Date
                        group x by new { x.RegisterID, x.Register, } into grp
                        select new
                        {
                            grp.Key.Register,
                            grp.Key.RegisterID,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                        };

            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFlights = ds.Sum(q => q.FlightCount),
                TotalHighLevel = ds.Sum(q => q.HighLevelCount),
                TotalMediumLevel = ds.Sum(q => q.MediumLevelCount),
                TotalLowLevel = ds.Sum(q => q.LowLevelCount),
            };

            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };


        }


        [HttpGet]
        [Route("api/fdm/dashboard/register/monthly/{df}/{dt}")]
        public async Task<DataResponse> GetAllFDMRegMonthly(DateTime df, DateTime dt)
        {
            var query = from x in context.FDMRegCptMonthlies
                        where x.Month >= df.Month && x.Year >= df.Year && x.Month <= dt.Month && x.Year <= dt.Year
                        group x by new { x.Register, x.RegisterID, x.Month, x.Year } into grp
                        select new
                        {
                            grp.Key.RegisterID,
                            grp.Key.Register,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            grp.Key.Month,
                            grp.Key.Year,
                        };


            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFlights = ds.Sum(q => q.FlightCount),
                TotalHighLevel = ds.Sum(q => q.HighLevelCount),
                TotalMediumLevel = ds.Sum(q => q.MediumLevelCount),
                TotalLowLevel = ds.Sum(q => q.LowLevelCount),
            };


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/tops/{count}/{df}/{dt}")]
        public async Task<DataResponse> GetTopCpt(int count, DateTime df, DateTime dt)
        {
            var query = from x in context.FDMRegCptDaily_
                        where x.Day <= dt.Date && x.Day >= df.Date
                        group x by new { x.CptName, x.CptId, x.CptCode } into grp
                        select new
                        {
                            grp.Key.CptName,
                            grp.Key.CptId,
                            grp.Key.CptCode,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,

                        };


            var result = query.OrderByDescending(q => q.Scores).Take(count).ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/last/{count}/{df}/{dt}")]
        public async Task<DataResponse> GetLastCpt(int count, DateTime df, DateTime dt)
        {
            var query = from x in context.FDMRegCptDaily_
                        where x.Day >= df && x.Day <= dt
                        group x by new { x.CptId, x.CptName, x.CptCode } into grp
                        select new
                        {
                            grp.Key.CptName,
                            grp.Key.CptId,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            grp.Key.CptCode,
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                        };

            var result = query.OrderBy(q => q.ScorePercentage).ThenBy(c => c.FlightCount).Take(count).ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/top/event/{count}/{df}/{dt}")]
        public async Task<DataResponse> GetTopCptEvents(int count, DateTime df, DateTime dt)
        {
            var query = from x in context.FDMRegCptDaily_
                        where x.Day >= df && x.Day <= dt
                        group x by new { x.CptId, x.CptName, x.CptCode } into grp
                        select new
                        {
                            grp.Key.CptName,
                            grp.Key.CptId,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            grp.Key.CptCode,
                        };

            var result = query.OrderByDescending(q => q.IncidentCount).Take(count).ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/last/event/{count}/{df}/{dt}")]
        public async Task<DataResponse> GetLastCptEvents(int count, DateTime df, DateTime dt)
        {
            var query = from x in context.FDMRegCptDaily_
                        where x.Day >= df && x.Day <= dt
                        group x by new { x.CptId, x.CptName, x.CptCode } into grp
                        select new
                        {
                            grp.Key.CptName,
                            grp.Key.CptId,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            grp.Key.CptCode,
                        };

            var result = query.OrderBy(q => q.IncidentCount).Take(count).ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/all/events/{df}/{dt}")]
        public async Task<DataResponse> GetEventsDaily(DateTime df, DateTime dt)
        {
            var query = from x in context.FDMDailies
                        where x.FlightDate >= df && x.FlightDate <= dt
                        group x by new { x.FlightDate } into grp
                        select new
                        {
                            grp.Key.FlightDate,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            EventsCount = grp.Sum(q => q.EventCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            HighPercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : grp.Sum(q => q.HighCount) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                            MediumPercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : grp.Sum(q => q.MediumCount) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                            LowPercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : grp.Sum(q => q.LowCount) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                        };
            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFlightCount = ds.Sum(q => q.FlightCount),
                TotalEventsCount = ds.Sum(q => q.EventsCount),
                TotalScores = ds.Sum(q => q.Scores),
                EventPerFlight = ds.Sum(q => q.FlightCount) != 0 ? ds.Sum(q => q.EventsCount) * 1.0 / ds.Sum(q => q.FlightCount) : 0,
            };


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };

        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/events/daily/{df}/{dt}/{p1Id}")]
        public async Task<DataResponse> GetCptDaily(DateTime df, DateTime dt, int p1Id)
        {
            var query = from x in context.FDMCptDailies
                        where x.Day >= df && x.Day <= dt && x.CptId == p1Id
                        group x by new { x.Day } into grp
                        select new
                        {
                            FlightDate = grp.Key.Day,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                            IncidentCount = grp.Sum(q => q.EventCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            HighPercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : grp.Sum(q => q.HighCount) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                            MediumPercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : grp.Sum(q => q.MediumCount) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                            LowPercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : grp.Sum(q => q.LowCount) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,

                        };
            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFilght = ds.Sum(q => q.FlightCount),
                TotalEvent = ds.Sum(q => q.IncidentCount),
                TotalHighLevel = ds.Sum(q => q.HighLevelCount),
                TotalMediumLevel = ds.Sum(q => q.MediumLevelCount),
                TotalLowLevel = ds.Sum(q => q.LowLevelCount),
                TotalScores = ds.Sum(q => q.Scores),
                AverageEvents = ds.Average(q => q.IncidentCount),
                AverageScores = ds.Average(q => q.Scores),

            };


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };

        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/events/{df}/{dt}/{p1Id}")]
        public async Task<DataResponse> GetCptEventsDaily(DateTime df, DateTime dt, int p1Id)
        {
            var query = from x in context.FDMCptEventDailies
                        where x.Day >= df && x.Day <= dt && x.CptId == p1Id
                        group x by new { x.EventName } into grp
                        select new
                        {
                            grp.Key.EventName,
                            EventCount = grp.Sum(q => q.EventCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                        };

            var result = query.ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/fo/events/{df}/{dt}/{p2Id}")]
        public async Task<DataResponse> GetFoEventsNameDaily(DateTime df, DateTime dt, int p2Id)
        {
            var query = from x in context.FDMFoEventDailies
                        where x.Day >= df && x.Day <= dt && x.P2Id == p2Id
                        group x by new { x.EventName } into grp
                        select new
                        {
                            EventCount = grp.Sum(q => q.EventCount),
                            grp.Key.EventName,
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                        };
            var result = query.ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/fo/events/{df}/{dt}/{P1Id}")]
        public async Task<DataResponse> GetRegCptFoEvents(DateTime df, DateTime dt, int P1Id)
        {
            var query = from x in context.FDMRegCptFoDailies
                        where x.Day >= df && x.Day <= dt && x.P1Id == P1Id
                        group x by new { x.P2Code } into grp
                        select new
                        {
                            grp.Key.P2Code,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),
                        };

            var result = query.ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/events/register/{df}/{dt}/{P1Id}")]
        public async Task<DataResponse> GetRegCptEvents(DateTime df, DateTime dt, int P1Id)
        {
            var query = from x in context.FDMRegCptDaily_
                        where x.Day >= df && x.Day <= dt && x.CptId == P1Id
                        group x by new { x.Register, x.RegisterID } into grp
                        select new
                        {
                            grp.Key.Register,
                            grp.Key.RegisterID,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),

                        };

            var result = query.ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetCptMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMCptMonthlies
                        where x.Month >= mf && x.Month <= mt && x.Year >= yf && x.Year <= yt && x.CptId == cptId
                        group x by new { x.Month } into grp
                        select new
                        {
                            grp.Key.Month,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            HighScore = grp.Sum(q => q.HighCount) * 4,
                            MediumScore = grp.Sum(q => q.MediumCount) * 2,
                            LowScore = grp.Sum(q => q.LowCount),
                            SelfResponse = grp.Sum(q => q.P1SelfResEventsCount) + grp.Sum(q => q.IPSelfResEventsCount),
                            OtherResponse = grp.Sum(q => q.EventCount) - (grp.Sum(q => q.P1SelfResEventsCount) + grp.Sum(q => q.IPSelfResEventsCount)),
                            FlightCount = grp.Sum(q => q.FlightCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            Score = grp.Sum(q => q.Score),
                            FaultPercentagePerFlight = grp.Sum(q => q.FaultPercentagePerFlight)
                        };

            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFilght = ds.Sum(q => q.FlightCount),
                TotalEvent = ds.Sum(q => q.EventsCount),
                TotalHighLevel = ds.Sum(q => q.HighCount),
                TotalMediumLevel = ds.Sum(q => q.MediumCount),
                TotalLowLevel = ds.Sum(q => q.LowCount),
                TotalScores = ds.Sum(q => q.Scores),
            };


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/fo/events/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetFoEventsNameMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMFoEventMonthlies
                        where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt && x.P2Id == cptId
                        group x by new { x.EventName } into grp
                        select new
                        {
                            EventCount = grp.Sum(q => q.EventCount),
                            grp.Key.EventName,
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                        };

            var result = query.ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/dashboard/cpt/events/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetCptEventsNameMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMCptEventMonthlies
                        where x.Month >= mf && x.Month <= mt && x.Year >= yf && x.Year <= yt && x.CptId == cptId
                        group x by new { x.EventName } into grp
                        select new
                        {
                            grp.Key.EventName,
                            EventCount = grp.Sum(q => q.EventCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                        };

            var result = query.ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/cpt/fo/events/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetCptFoMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMCptFoMonthlies
                        where x.Year >= yf && x.Year <= yt && x.Month >= mf && x.Month <= mt && (x.P1Id == cptId || x.IPId == cptId)
                        group x by new { x.P2Id, x.P2Code, x.P2Name } into grp
                        select new
                        {
                            grp.Key.P2Id,
                            grp.Key.P2Code,
                            grp.Key.P2Name,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                        };


            var result = query.Where(q => q.EventsCount > 0).ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/cpt/fo/events/daily/{df}/{dt}/{P1Id}")]
        public async Task<DataResponse> GetCptFoDaily(DateTime df, DateTime dt, int P1Id)
        {
            var query = from x in context.FDMCptFoDailies
                        where x.Day >= df && x.Day <= dt && (x.P1Id == P1Id || x.IPId == P1Id)
                        group x by new { x.P2Id, x.P2Code, x.P2Name } into grp
                        select new
                        {
                            grp.Key.P2Id,
                            grp.Key.P2Code,
                            grp.Key.P2Name,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                        };


            var result = query.Where(q => q.EventsCount > 0).ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/cpt/ip/events/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetCptIpMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMCptIpMonthlies
                        where x.Year >= yf && x.Year <= yt && x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.p1Id == cptId
                        group x by new { x.IpId, x.IpCode, x.IpName } into grp
                        select new
                        {
                            grp.Key.IpId,
                            grp.Key.IpCode,
                            grp.Key.IpName,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                        };


            var result = query.Where(q => q.EventsCount > 0).ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/ip/cpt/events/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetIpCptMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMCptFoMonthlies
                        where x.Year >= yf && x.Year <= yt && x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.IPId == cptId
                        group x by new { x.P1Id, x.P1Code, x.P1Name } into grp
                        select new
                        {
                            grp.Key.P1Id,
                            grp.Key.P1Code,
                            grp.Key.P1Name,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                        };


            var result = query.Where(q => q.EventsCount > 0).ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/cpt/ip/events/daily/{df}/{dt}/{P1Id}")]
        public async Task<DataResponse> GetCptIpDaily(DateTime df, DateTime dt, int P1Id)
        {
            var query = from x in context.FDMCptIpDailies
                        where x.Day >= df && x.Day <= dt && x.P1Id == P1Id
                        group x by new { x.IpId, x.IpCode, x.IpName } into grp
                        select new
                        {
                            grp.Key.IpId,
                            grp.Key.IpCode,
                            grp.Key.IpName,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                        };


            var result = query.Where(q => q.EventsCount > 0).ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/ip/cpt/events/daily/{df}/{dt}/{P1Id}")]
        public async Task<DataResponse> GetIpCptDaily(DateTime df, DateTime dt, int P1Id)
        {
            var query = from x in context.FDMCptFoDailies
                        where x.Day >= df && x.Day <= dt && (x.IPId == P1Id)
                        group x by new { x.P1Id, x.P1Code, x.P1Name } into grp
                        select new
                        {
                            grp.Key.P1Id,
                            grp.Key.P1Code,
                            grp.Key.P1Name,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                        };


            var result = query.Where(q => q.EventsCount > 0).ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/cpt/events/register/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetRegCptEventsMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMRegCptMonthlies
                        where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt && x.CptId == cptId
                        group x by new { x.Register, x.RegisterID } into grp
                        select new
                        {
                            grp.Key.Register,
                            grp.Key.RegisterID,
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighLevelCount = grp.Sum(q => q.HighCount),
                            MediumLevelCount = grp.Sum(q => q.MediumCount),
                            LowLevelCount = grp.Sum(q => q.LowCount),

                        };

            var result = query.ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/events/{p1id}/{regId}/{typeId}/{df}/{dt}")]
        public async Task<DataResponse> GetEventsFDM(int p1id, int regId, int typeId, DateTime df, DateTime dt)
        {
            var query = from x in context.ViewFDMs
                        where x.Date >= df && x.Date <= dt
                        select x;

            if (p1id != -1)
                query = query.Where(q => q.P1Id == p1id);

            if (regId != -1)
                query = query.Where(q => q.RegisterID == regId);

            if (typeId != -1)
                query = query.Where(q => q.TypeID == typeId);

            var result = query.ToList();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = result,
            };
        }

        [HttpGet]
        [Route("api/get/cpt/fdm/info/{datef}/{datet}/{crewId}")]
        public async Task<DataResponse> GetCptFDMInfo(DateTime? datef, DateTime? datet, int crewId)
        {
            var query = from x in context.ViewFDMs
                        where x.Date >= datef && x.Date <= datet && (x.P1Id == crewId || x.IPId == crewId || x.P2Id == crewId)
                        select x;

            var result = query.ToList();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }



        [HttpGet]
        [Route("api/fdm/fo/daily/{p2id}/{df}/{dt}")]
        public async Task<DataResponse> GetFDMFoDaily(int p2id, DateTime df, DateTime dt)
        {
            var query = from x in context.FDMFoDailies
                        where x.Day >= df && x.Day <= dt && x.P2Id == p2id
                        group x by new { x.Day } into grp
                        select new
                        {
                            grp.Key.Day,
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            Score = grp.Sum(q => q.Score),
                            FlightCount = grp.Sum(q => q.FlightCount),
                            EventCount = grp.Sum(q => q.EventCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                        };
            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFilght = ds.Sum(q => q.FlightCount),
                TotalEvent = ds.Sum(q => q.EventCount),
                TotalHighLevel = ds.Sum(q => q.HighCount),
                TotalMediumLevel = ds.Sum(q => q.MediumCount),
                TotalLowLevel = ds.Sum(q => q.LowCount),
                TotalScores = ds.Sum(q => q.Score),
                AverageEvents = ds.Average(q => q.EventCount),
                AverageScores = ds.Average(q => q.Score),
            };

            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/fo/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetFDMFoMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = from x in context.FDMFoMonthlies
                        where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt && x.P2Id == cptId
                        group x by new { x.Month } into grp
                        select new
                        {
                            grp.Key.Month,
                            EventsCount = grp.Sum(q => q.EventCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            HighScore = grp.Sum(q => q.HighCount) * 4,
                            MediumScore = grp.Sum(q => q.MediumCount) * 2,
                            LowScore = grp.Sum(q => q.LowCount),
                            FlightCount = grp.Sum(q => q.FlightCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            Score = grp.Sum(q => q.Score),
                            FaultPercentagePerFlight = grp.Sum(q => q.scoresPerFlight)
                        };

            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFilght = ds.Sum(q => q.FlightCount),
                TotalEvent = ds.Sum(q => q.EventsCount),
                TotalHighLevel = ds.Sum(q => q.HighCount),
                TotalMediumLevel = ds.Sum(q => q.MediumCount),
                TotalLowLevel = ds.Sum(q => q.LowCount),
                TotalScores = ds.Sum(q => q.Scores),
            };


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }


        //[HttpGet]
        //[Route("api/fdm/fo/register/{p2id}/{mf}/{mt}")]
        //public async Task<DataResponse> GetRegFoDaily(int p2id, DateTime mf, DateTime mt)
        //{
        //    var query = from x in context.FDMRegFoDaily
        //                where x.Day >= df && x.Day <= dt && x.P2Id == p2id
        //                group x by new { x.Register, x.RegisterID } into grp
        //                select new
        //                {
        //                    grp.Key.Register,
        //                    grp.Key.RegisterID,
        //                    FlightCount = grp.Sum(q => q.FlightCount),
        //                    HighLevelCount = grp.Sum(q => q.HighCount),
        //                    MediumLevelCount = grp.Sum(q => q.MediumCount),
        //                    LowLevelCount = grp.Sum(q => q.LowCount),

        //                };

        //    var result = query.ToList();
        //    return new DataResponse()
        //    {
        //        IsSuccess = true,
        //        Data = result
        //    };
        //}


        //[HttpGet]
        //[Route("api/fdm/fo/register/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        //public async Task<DataResponse> GetRegFoMonthly(int yf, int yt, int mf, int mt, int cptId)
        //{
        //    var query = from x in context.FDMRegFoMonthly
        //                where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt && x.P2Id == cptId
        //                group x by new { x.Register, x.RegisterID } into grp
        //                select new
        //                {
        //                    grp.Key.Register,
        //                    grp.Key.RegisterID,
        //                    FlightCount = grp.Sum(q => q.FlightCount),
        //                    HighLevelCount = grp.Sum(q => q.HighCount),
        //                    MediumLevelCount = grp.Sum(q => q.MediumCount),
        //                    LowLevelCount = grp.Sum(q => q.LowCount),

        //                };

        //    var result = query.ToList();
        //    return new DataResponse()
        //    {
        //        IsSuccess = true,
        //        Data = result
        //    };
        //}

        [HttpGet]
        [Route("api/fdm/register/daily/{df}/{dt}/{reg}")]
        public async Task<DataResponse> GetRegdaily(DateTime df, DateTime dt, string reg)
        {
            var query = from x in context.FDMRegDailies
                        where x.FlightDate >= df && x.FlightDate <= dt && x.Register == reg
                        group x by new { x.FlightDate } into grp
                        select new
                        {
                            grp.Key.FlightDate,
                            EventCount = grp.Sum(q => q.EventCount),
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,

                        };

            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFilght = ds.Sum(q => q.FlightCount),
                TotalEvent = ds.Sum(q => q.EventCount),
                TotalHighLevel = ds.Sum(q => q.HighCount),
                TotalMediumLevel = ds.Sum(q => q.MediumCount),
                TotalLowLevel = ds.Sum(q => q.LowCount),
                TotalScores = ds.Sum(q => q.Scores),
                AverageEvents = ds.Average(q => q.EventCount),
                AverageScores = ds.Average(q => q.Scores),

            };
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/register/monthly/{year}/{mf}/{mt}/{reg}")]
        public async Task<DataResponse> GetRegMonthly(int year, int mf, int mt, string reg)
        {
            var query = from x in context.FDMRegMonthlies
                        where x.Month >= mf && x.Month <= mt && x.Register == reg && x.Year == year
                        group x by new { x.Month } into grp
                        select new
                        {
                            grp.Key.Month,
                            EventCount = grp.Sum(q => q.EventCount),
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,

                        };

            var ds = query.ToList();
            var result = new
            {
                data = ds,
                TotalFilght = ds.Sum(q => q.FlightCount),
                TotalEvent = ds.Sum(q => q.EventCount),
                TotalHighLevel = ds.Sum(q => q.HighCount),
                TotalMediumLevel = ds.Sum(q => q.MediumCount),
                TotalLowLevel = ds.Sum(q => q.LowCount),
                TotalScores = ds.Sum(q => q.Scores),
                AverageEvents = ds.Average(q => q.EventCount),
                AverageScores = ds.Average(q => q.Scores),

            };

            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/register/cpt/daily/{df}/{dt}/{reg}")]
        public async Task<DataResponse> GetRegCptDaily(DateTime df, DateTime dt, string reg)
        {
            var query = from x in context.FDMRegCptDaily_
                        where x.Day >= df && x.Day <= dt && x.Register == reg
                        group x by new { x.CptCode } into grp
                        select new
                        {
                            grp.Key.CptCode,
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            EventCount = grp.Sum(q => q.EventCount),
                            FlightCount = grp.Sum(q => q.FlightCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                        };

            var result = query.ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/register/cpt/monthly/{year}/{mf}/{mt}/{reg}")]
        public async Task<DataResponse> GetRegCptMonthly(int year, int mf, int mt, string reg)
        {
            var query = from x in context.FDMRegCptMonthlies
                        where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Register == reg && x.Year == year
                        group x by new { x.CptCode } into grp
                        select new
                        {
                            grp.Key.CptCode,
                            EventCount = grp.Sum(q => q.EventCount),
                            FlightCount = grp.Sum(q => q.FlightCount),
                            HighCount = grp.Sum(q => q.HighCount),
                            MediumCount = grp.Sum(q => q.MediumCount),
                            LowCount = grp.Sum(q => q.LowCount),
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                            ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,
                        };

            var result = query.ToList();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/fdm/register/events/daily/{df}/{dt}/{reg}")]
        public async Task<DataResponse> GetRegEventsNameDaily(DateTime df, DateTime dt, string reg)
        {
            var query = from x in context.FDMRegEventDailies
                        where x.Day >= df && x.Day <= dt && x.register == reg
                        group x by new { x.EventName } into grp
                        select new
                        {
                            EventCount = grp.Sum(q => q.EventCount),
                            grp.Key.EventName,
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                        };
            var result = query.ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };

        }

        [HttpGet]
        [Route("api/fdm/register/events/monthly/{year}/{mf}/{mt}/{reg}")]
        public async Task<DataResponse> GetRegEventsNameMonthly(int year, int mf, int mt, string reg)
        {
            var query = from x in context.FDMRegEventMonthlies
                        where x.Month >= mf && x.Month <= mt && x.Year == year && x.register == reg
                        group x by new { x.EventName } into grp
                        select new
                        {
                            EventCount = grp.Sum(q => q.EventCount),
                            grp.Key.EventName,
                            Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                        };

            var result = query.ToList();


            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };

        }


        [HttpGet]
        [Route("api/fdm/get/cpt/monthly/{yf}/{yt}/{mf}/{mt}")]
        public async Task<DataResponse> GetCptByMonth(int yf, int yt, int mf, int mt)
        {
            try
            {
                var query = (from x in context.FDMCptAlls
                             where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt
                             select x
                            ).ToList();
                var result = (from x in query

                              group x by new { x.CptId, x.CptName, x.JobGroup } into grp
                              select new
                              {
                                  CptName = grp.Key.CptName,
                                  CptId = grp.Key.CptId,
                                  Items = grp.OrderBy(q => q.Month).ToList(),
                                  EventsCount = grp.Sum(q => q.EventCount),
                                  Flights = grp.Sum(q => q.FlightCount),
                                  Scores = grp.Sum(q => q.Score),
                                  ScorePerFlight = grp.Sum(q => q.Score) * 1.0 / grp.Sum(q => q.FlightCount) * 100.0,
                                  JobGroup = grp.Key.JobGroup
                              }).ToList();

                return new DataResponse()
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                int jjj = 0;
                return new DataResponse()
                {
                    IsSuccess = true,
                    Data = 1
                };
            }

        }

        [HttpGet]
        [Route("api/fdm/get/cpt/phase/monthly/{yf}/{yt}/{mf}/{mt}/{cptId}")]
        public async Task<DataResponse> GetCptPhaseMonthly(int yf, int yt, int mf, int mt, int cptId)
        {
            var query = (from x in context.FDMPhaseMonthlies
                         where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yt && x.Year <= yf && x.CrewId == cptId
                         select x).ToList();
            var result = (from x in query
                          group x by new { x.Phase, x.Name } into grp
                          select new
                          {
                              Phase = grp.Key.Phase,
                              CptName = grp.Key.Name,
                              Score = grp.Sum(q => q.Score)
                          }).ToList();

            //var result = query.ToList();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result
            };
        }

        [HttpGet]
        [Route("api/get/fdm/top/cpt/{yf}/{yt}/{mf}/{mt}")]
        public async Task<DataResponse> GetTopCpt(int yf, int yt, int mf, int mt)
        {
            //var query = (from x in context.FDMCptAll
            //             where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt
            //             select x).ToList();
            //var result = (from x in query
            //              group x by new { x.CptName, x.CptCode, x.CptId } into grp
            //              select new
            //              {
            //                  CptName = grp.Key.CptName,
            //                  CptCode = grp.Key.CptCode,
            //                  CptId = grp.Key.CptId,
            //                  Score = grp.Sum(q => q.Score)
            //              });

            //return new DataResponse()
            //{
            //    IsSuccess = true,
            //    Data = result.OrderByDescending(q => q.Score).Take(10)
            //};

            var query = from x in context.FDMCptAlls
                        where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt
                        group x by new { x.CptName, x.CptId, x.CptCode } into grp
                        select new
                        {
                            grp.Key.CptCode,
                            grp.Key.CptId,
                            grp.Key.CptName,
                            Score = grp.Sum(q => q.Score)
                        };

            var result = query.ToList().OrderByDescending(q => q.Score).Take(10);
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result

            };
        }

        [HttpGet]
        [Route("api/get/fdm/all/cpt/{yf}/{yt}/{mf}/{mt}")]
        public async Task<DataResponse> GetAllCpt(int yf, int yt, int mf, int mt)
            {
                var query = (from x in context.FDMCptAlls
                             where x.Month >= (mf + 1) && x.Month <= (mt + 1) && x.Year >= yf && x.Year <= yt
                             select x).ToList();
                var result = (from x in query
                              group x by new { x.HighCount, x.MediumCount, x.LowCount } into grp
                              select new
                              {
                                  HighCount = grp.Sum(q => q.HighCount),
                                  MediumCount = grp.Sum(q => q.MediumCount),
                                  LowCount = grp.Sum(q => q.LowCount),
                                  FlightCount = grp.Sum(q => q.FlightCount),
                                  IncidentCount = grp.Sum(q => q.EventCount),
                                  Scores = grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount),
                                  ScorePercentage = grp.Sum(q => q.FlightCount) == 0 ? 0 : (grp.Sum(q => q.HighCount) * 4 + grp.Sum(q => q.MediumCount) * 2 + grp.Sum(q => q.LowCount)) * 1.0 / grp.Sum(q => q.FlightCount) * 100,

                              });

                var result2 = new
                {
                    data = result,
                    TotalFlights = result.Sum(q => q.FlightCount),
                    TotalHighLevel = result.Sum(q => q.HighCount),
                    TotalMediumLevel = result.Sum(q => q.MediumCount),
                    TotalLowLevel = result.Sum(q => q.LowCount),
                    TotalEvents = result.Sum(q => q.IncidentCount),
                    TotalScore = result.Sum(q => q.Scores),

                };



                return new DataResponse()
                {
                    IsSuccess = true,
                    Data = result2
                };
            }




        }
    }