using ApiQA.Models;
using ApiQA.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using static ApiQA.Controllers.QaController;

namespace ApiQA.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogController : ApiController
    {
        ppa_entities context = new ppa_entities();

        [HttpGet]
        [Route("api/qa/log/main")]
        public async Task<DataResponse> GetFlightLogMain(DateTime df,DateTime dt)
        {
            try
            {
                df = df.Date;
                dt = dt.Date.AddDays(1);
                var result = await context.ViewFlightLogMains.Where(q => q.STDLocal >= df && q.STDLocal < dt).OrderBy(q => q.STDLocal).ToListAsync();
                return new DataResponse()
                {
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "   Inner: " + ex.InnerException.Message;
                return new DataResponse()
                {
                    Data = msg,
                    IsSuccess = false
                };
            }
        }


        [HttpGet]
        [Route("api/qa/log/detail/{fid}")]
        public async Task<DataResponse> GetFlightLogDetail(int fid)
        {
            try
            {

                var flight_log = await context.ViewFlightLogs.Where(q => q.FlightId == fid).OrderBy(q => q.DateCreate).ToListAsync();
                var crew_log = await context.ViewDutyLogs.Where(q => q.FlightId == fid).OrderBy(q => q.DateCreate).ToListAsync();
                return new DataResponse()
                {
                    Data = new { flight_log, crew_log },
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "   Inner: " + ex.InnerException.Message;
                return new DataResponse()
                {
                    Data = msg,
                    IsSuccess = false
                };
            }
        }






    }
}
