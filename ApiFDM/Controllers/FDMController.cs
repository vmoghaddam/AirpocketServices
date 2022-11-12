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
                context.SaveChangesAsync();
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


    }
}