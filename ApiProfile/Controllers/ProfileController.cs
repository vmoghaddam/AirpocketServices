using ApiProfile.Models;
using ApiProfile.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiProfile.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfileController : ApiController
    {
        [Route("api/profile/employee/save")]

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> PostEmployee(ViewModels.Employee dto)
        {
            var context = new Models.dbEntities();

            var nidCheck = await context.People.Where(q => q.Id != dto.PersonId && q.NID == dto.Person.NID).FirstOrDefaultAsync();
            if (nidCheck!=null)
            {
                return Exceptions.getDuplicateException("Person-01", "NID");
            }

            Models.Person person = null;
            if (dto.PersonId != -1)
                person = await context.People.Where(q => q.Id == dto.PersonId).FirstOrDefaultAsync();
            else
                person = await context.People.Where(q => q.NID == dto.Person.NID).FirstOrDefaultAsync();
            if (person == null)
            {
                person = new Models.Person();
                person.DateCreate = DateTime.Now;
                context.People.Add(person);
            }
            ViewModels.Person.Fill(person, dto.Person);
            var cid = (int)dto.CustomerId;
            Models.PersonCustomer personCustomer = await context.PersonCustomers.Where(q => q.CustomerId == cid && q.PersonId == dto.Person.PersonId).FirstOrDefaultAsync();
                //await unitOfWork.PersonRepository.GetPersonCustomer((int)dto.CustomerId, dto.Person.PersonId);
            if (personCustomer == null)
            {
                personCustomer = new Models.PersonCustomer();

                person.PersonCustomers.Add(personCustomer);
            }
            ViewModels.PersonCustomer.Fill(personCustomer, dto);
            Models.Employee employee = await context.Employees.Where(q => q.Id == personCustomer.Id).FirstOrDefaultAsync();
            if (employee == null)
                employee = new Models.Employee();
            personCustomer.Employee = employee;
            ViewModels.Employee.Fill(employee, dto);

            FillEmployeeLocations(context,employee, dto);

           FillAircraftTypes(context,person, dto);
             FillDocuments(context,person, dto);

            var saveResult = await context.SaveAsync();
            if (saveResult.Code != HttpStatusCode.OK)
                return saveResult;


            dto.Id = employee.Id;
            return Ok(dto);
        }

        [Route("api/profile/employee/nid/{cid}/{nid}")]
        public async Task<IHttpActionResult> GetEmployee(string nid, int cid)
        {
            var context = new Models.dbEntities();
            ViewModels.Employee employee = null;
            var entity = await  context.People.SingleOrDefaultAsync(q => q.NID == nid && !q.IsDeleted);
            if (entity == null)
                return Ok();
            employee = new ViewModels.Employee();
            employee.Person = new ViewModels.Person();
            ViewModels.Person.FillDto(entity, employee.Person);
            var actypes = await context.ViewPersonAircraftTypes.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.AircraftTypes = ViewModels.PersonAircraftType.GetDtos(actypes);

            var doc = await context.ViewPersonDocuments.Where(q => q.PersonId == entity.Id).ToListAsync();
            var docIds = doc.Select(q => q.Id).ToList();
            var files = await context.ViewPersonDocumentFiles.Where(q => q.PersonId == entity.Id).ToListAsync();
            employee.Person.Documents = ViewModels.PersonDocument.GetDtos(doc, files);

            var pc = context.PersonCustomers.SingleOrDefault(q => q.CustomerId == cid && q.PersonId == entity.Id && !q.IsDeleted);

            if (pc != null)
            {
                var emp = await context.Employees.FirstOrDefaultAsync(q => q.Id == pc.Id);
                if (emp != null)
                {
                    employee.CustomerId = cid;
                    employee.DateActiveEnd = pc.DateActiveEnd;
                    employee.DateActiveStart = pc.DateActiveStart;
                    employee.DateJoinCompany = pc.DateJoinCompany;
                    employee.DateJoinCompanyP = pc.DateJoinCompanyP;
                    employee.DateConfirmedP = pc.DateConfirmedP;
                    employee.DateConfirmed = pc.DateConfirmed;
                    employee.DateLastLogin = pc.DateLastLogin;
                    employee.DateLastLoginP = pc.DateLastLoginP;
                    employee.DateRegister = pc.DateRegister;
                    employee.DateRegisterP = pc.DateRegisterP;
                    employee.Id = pc.Id;
                    employee.IsActive = pc.IsActive;
                    employee.Password = pc.Password;
                    employee.PersonId = entity.Id;
                    employee.GroupId = pc.GroupId;
                    employee.C1GroupId = pc.C1GroupId;
                    employee.C2GroupId = pc.C2GroupId;
                    employee.C3GroupId = pc.C3GroupId;
                    employee.PID = emp.PID;
                    employee.Phone = emp.Phone;
                    employee.BaseAirportId = emp.BaseAirportId;
                    employee.DateInactiveBegin = emp.DateInactiveBegin;
                    employee.DateInactiveEnd = emp.DateInactiveEnd;
                    employee.InActive = emp.InActive;
                    var locs = await context.ViewEmployeeLocations.Where(q => q.EmployeeId == pc.Id).ToListAsync();
                    employee.Locations = ViewModels.EmployeeLocation.GetDtos(locs);


                }


            }

            //soosk
            ///var employee = await unitOfWork.PersonRepository.GetEmployeeDtoByNID(nid, cid);
            return Ok(employee);
        }

        [Route("api/profiles/main/{cid}/{active}/{grp}")]

        public async Task<IHttpActionResult> GetProfilesByCustomerId(int cid,int active,string grp)
        {
            try
            {
                var context = new Models.dbEntities();
                var query = context.ViewProfiles.Where(q => q.CustomerId == cid);
                if (active == 1)
                    query = query.Where(q => q.InActive == false);
                grp = grp.Replace('x', '/');
                if (grp != "-1")
                    query = query.Where(q => q.JobGroupRoot == grp);
                var profiles = await query.ToListAsync();

                return Ok(profiles);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        public void FillEmployeeLocations(dbEntities context, Models. Employee employee, ViewModels.Employee dto)
        {
            var exists =  context.EmployeeLocations.Where(q => q.EmployeeId == employee.Id).ToList();
            var dtoLocation = dto.Locations.First();
            if (exists == null || exists.Count == 0)
            {
                employee.EmployeeLocations.Add(new Models. EmployeeLocation()
                {
                    DateActiveEnd = dtoLocation.DateActiveEnd,
                    DateActiveEndP = dtoLocation.DateActiveEnd != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveEnd)) : null,
                    DateActiveStart = dtoLocation.DateActiveStart,
                    DateActiveStartP = dtoLocation.DateActiveStart != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveStart)) : null,
                    IsMainLocation = dtoLocation.IsMainLocation,
                    LocationId = dtoLocation.LocationId,
                    OrgRoleId = dtoLocation.OrgRoleId,
                    Phone = dtoLocation.Phone,
                    Remark = dtoLocation.Remark

                });
            }
            else
            {
                exists[0].DateActiveEnd = dtoLocation.DateActiveEnd;
                exists[0].DateActiveEndP = dtoLocation.DateActiveEnd != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveEnd)) : null;
                exists[0].DateActiveStart = dtoLocation.DateActiveStart;
                exists[0].DateActiveStartP = dtoLocation.DateActiveStart != null ? (Nullable<decimal>)Convert.ToDecimal(Utils.DateTimeUtil.GetPersianDateTimeDigital((DateTime)dtoLocation.DateActiveStart)) : null;
                exists[0].IsMainLocation = dtoLocation.IsMainLocation;
                exists[0].LocationId = dtoLocation.LocationId;
                exists[0].OrgRoleId = dtoLocation.OrgRoleId;
                exists[0].Phone = dtoLocation.Phone;
                exists[0].Remark = dtoLocation.Remark;
            }

        }
        public void FillAircraftTypes(dbEntities context, Models.Person person, ViewModels.Employee dto)
        {
            var existing =  context.PersonAircraftTypes.Where(q => q.PersonId == person.Id).ToList();
            var deleted = (from x in existing
                           where dto.Person.AircraftTypes.FirstOrDefault(q => q.Id == x.Id) == null
                           select x).ToList();
            var added = (from x in dto.Person.AircraftTypes
                         where existing.FirstOrDefault(q => q.Id == x.Id) == null
                         select x).ToList();
            var edited = (from x in existing
                          where dto.Person.AircraftTypes.FirstOrDefault(q => q.Id == x.Id) != null
                          select x).ToList();
            foreach (var x in deleted)
                context.PersonAircraftTypes.Remove(x);
            foreach (var x in added)
                context.PersonAircraftTypes.Add(new Models.PersonAircraftType()
                {
                    Person = person,
                    AircraftTypeId = x.AircraftTypeId,
                    IsActive = x.IsActive,
                    Remark = x.Remark,
                    DateLimitBegin = x.DateLimitBegin,
                    DateLimitEnd = x.DateLimitEnd

                });
            foreach (var x in edited)
            {
                var item = dto.Person.AircraftTypes.FirstOrDefault(q => q.Id == x.Id);
                if (item != null)
                {
                    x.AircraftTypeId = item.AircraftTypeId;
                    x.DateLimitBegin = item.DateLimitBegin;
                    x.DateLimitEnd = item.DateLimitEnd;
                    x.IsActive = item.IsActive;
                    x.Remark = item.Remark;

                }
            }
        }
        public void FillDocuments(dbEntities context, Models.Person person, ViewModels.Employee dto)
        {
            var existing =  context.PersonDocuments.Include("Documents").Where(q => q.PersonId == person.Id).ToList();
            var deleted = (from x in existing
                           where dto.Person.Documents.FirstOrDefault(q => q.Id == x.Id) == null
                           select x).ToList();
            var added = (from x in dto.Person.Documents
                         where existing.FirstOrDefault(q => q.Id == x.Id) == null
                         select x).ToList();
            var edited = (from x in existing
                          where dto.Person.Documents.FirstOrDefault(q => q.Id == x.Id) != null
                          select x).ToList();
            foreach (var x in deleted)
                context.PersonDocuments.Remove(x);
            foreach (var x in added)
            {
                var pd = new Models.PersonDocument()
                {
                    Person = person,

                    Remark = x.Remark,
                    DocumentTypeId = x.DocumentTypeId,
                    Title = x.Title,


                };
                foreach (var file in x.Documents)
                {
                    pd.Documents.Add(new Document()
                    {
                        FileType = file.FileType,
                        FileUrl = file.FileUrl,
                        SysUrl = file.SysUrl,
                        Title = file.Title,

                    });
                }
                context.PersonDocuments.Add(pd);
            }
            foreach (var x in edited)
            {
                var item = dto.Person.Documents.FirstOrDefault(q => q.Id == x.Id);
                if (item != null)
                {
                    x.DocumentTypeId = item.DocumentTypeId;
                    x.Title = item.Title;
                    x.Remark = item.Remark;

                    while (x.Documents.Count > 0)
                    {
                        var f = x.Documents.First();
                         context.Documents.Remove(f);
                    }
                    foreach (var f in item.Documents)
                        x.Documents.Add(new Document()
                        {
                            FileType = f.FileType,
                            FileUrl = f.FileUrl,
                            SysUrl = f.SysUrl,
                            Title = f.Title,

                        });
                }
            }
        }




    }
}
