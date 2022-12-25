using AirpocketTRN.Models;
using AirpocketTRN.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AirpocketTRN.Services
{
    public interface ICourseService
    {

    }
    public class CourseService : ICourseService
    {
        FLYEntities context = null;
        public CourseService()
        {
            context = new FLYEntities();
            context.Configuration.LazyLoadingEnabled = false;
        }
        public async Task<DataResponse> GetCourseTypes()
        {
            var result = await context.ViewCourseTypes.OrderBy(q => q.Title).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }


        public async Task<DataResponse> GetGRPCTExpiring()
        {
            var result = await context.GRPCourseTypeExpirings.OrderByDescending(q => q.ExpiredCount).ThenByDescending(q => q.ExpiringCount).ThenBy(q => q.Title).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }
        public async Task<DataResponse> GetCourseRemaining(int dd)
        {
            var result = await context.ViewCourseRemainings.Where(q => q.Remaining == dd).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }
        public async Task<DataResponse> GetGRPCTExpiringByMainGroup(string main)
        {
            var result = await context.GRPCourseTypeExpiringMainGroups.Where(q => q.JobGroupMainCode == main).OrderByDescending(q => q.ExpiredCount).ThenByDescending(q => q.ExpiringCount).ThenBy(q => q.Title).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetGRPCTExpiringMainGroups(int typeId)
        {
            var query = from x in context.GRPCourseTypeExpiringMainGroups
                        select x;
            if (typeId != -1)
                query = query.Where(q => q.TypeId == typeId);
            var result = await query.OrderByDescending(q => q.ExpiredCount).ThenByDescending(q => q.ExpiringCount).ThenBy(q => q.JobGroupMain).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetGRPMainGroupsExpiring(string code)
        {
            var query = from x in context.GRPMainGroupsExpirings
                        select x;
            if (code != "-1")
                query = query.Where(q => q.JobGroupMainCode == code);
            var result = await query.OrderByDescending(q => q.ExpiredCount).ThenByDescending(q => q.ExpiringCount).ThenBy(q => q.JobGroupMain).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }
        public async Task<DataResponse> GetGRPGroupsExpiring(string main, string code)
        {
            var query = from x in context.GRPGroupsExpirings
                        select x;
            if (main != "-1")
                query = query.Where(q => q.JobGroupMainCode == main);
            if (code != "-1")
                query = query.Where(q => q.JobGroupCode2 == code);
            var result = await query.OrderByDescending(q => q.ExpiredCount).ThenByDescending(q => q.ExpiringCount).ThenBy(q => q.JobGroupMain).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }
        public async Task<DataResponse> GetGRPGroupsExpiringCourseTypes(string main, string code, int type)
        {
            var query = from x in context.GRPGroupsCourseTypeExpirings
                        select x;
            if (main != "-1")
                query = query.Where(q => q.JobGroupMainCode == main);
            if (code != "-1")
                query = query.Where(q => q.JobGroupCode2 == code);
            if (type != -1)
                query = query.Where(q => q.TypeId == type);
            var result = await query.OrderByDescending(q => q.ExpiredCount).ThenByDescending(q => q.ExpiringCount).ThenBy(q => q.JobGroupMain).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetGRPCTExpiringGroups(string code, int type)
        {
            var query = from x in context.GRPCourseTypeExpiringGroups
                        select x;
            if (code != "-1")
                query = query.Where(q => q.JobGroupMainCode == code);
            if (type != -1)
                query = query.Where(q => q.TypeId == type);
            var result = await query.OrderByDescending(q => q.ExpiredCount).ThenByDescending(q => q.ExpiringCount).ThenBy(q => q.JobGroupMain).ThenBy(q => q.JobGroup).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCourseTypeJobGroups(int cid)
        {
            var result = await context.ViewCourseTypeJobGroups.Where(q => q.CourseTypeId == cid).OrderBy(q => q.Title).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCourseTypeJobGroupsByGroup(int gid)
        {
            var result = await context.ViewCourseTypeJobGroups.Where(q => q.Id == gid).OrderBy(q => q.Title).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCertificateTypes()
        {
            var result = await context.CertificateTypes.OrderBy(q => q.Title).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }


        public async Task<DataResponse> GetCoursesByType(int tid, int sid)
        {
            var query = context.ViewCourseNews.Where(q => q.CourseTypeId == tid);
            if (sid != -1)
                query = query.Where(q => q.StatusId == sid);
            var result = await query.OrderBy(q => q.CourseType).ThenByDescending(q => q.DateStart).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetPersonCourses(int pid)
        {
            var result = await context.ViewCoursePeopleRankeds.Where(q => q.PersonId == pid).OrderByDescending(q => q.DateStart).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetPersonMandatoryCourses(int pid)
        {
            var result = await context.ViewMandatoryCourseEmployees.Where(q => q.Id == pid).OrderBy(q => q.CourseType).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetPersonMandatoryCoursesByType(int type, int group)
        {
            var result = await context.ViewMandatoryCourseEmployees.Where(q => q.CourseTypeId == type && q.GroupId == group).OrderBy(q => q.ValidStatus).ThenBy(q => q.Remains).ThenBy(q => q.Name).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCoursePeople(int cid)
        {
            var result = await context.ViewCoursePeoples.OrderBy(q => q.CourseId == cid).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCourseView(int cid)
        {
            var result = await context.ViewCourseNews.Where(q => q.Id == cid).FirstOrDefaultAsync();


            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCourseViewObject(int cid)
        {
            var course = await context.ViewCourseNews.Where(q => q.Id == cid).FirstOrDefaultAsync();
            var sessions = await context.CourseSessions.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();
            var syllabi = await context.CourseSyllabus.Where(q => q.CourseId == cid).ToListAsync();

            return new DataResponse()
            {
                Data = new
                {
                    course,
                    sessions,
                    syllabi

                },
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCourse(int cid)
        {
            var result = await context.Courses.Where(q => q.Id == cid).FirstOrDefaultAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }
        //zamani
        public async Task<DataResponse> GetCertificatesHistory(int pid)
        {
            var result = await context.ViewCoursePeoplePassedRankeds.Where(q => q.PersonId == pid && q.RankLast == 1).OrderBy(q => q.DateExpire).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }
        //09-11
        public async Task<DataResponse> GetTrainingCard(int pid)
        {
            var person = await context.People.Where(q => q.Id == pid).FirstOrDefaultAsync();
            var employee = await context.ViewEmployees.Where(q => q.PersonId == pid).FirstOrDefaultAsync();
            var result = await context.ViewCoursePeoplePassedRankeds.Where(q => q.PersonId == pid && q.RankLast == 1).OrderBy(q => q.DateExpire).ToListAsync();
            var trg02 = result.Where(q => q.CourseType == "AVSEC-TRG-02").FirstOrDefault();
            var ds = new List<ViewCoursePeoplePassedRanked>();
            if (employee.JobGroup == "TRE" || employee.JobGroup == "TRI" || employee.JobGroup == "LTC" || employee.JobGroup == "P1" || employee.JobGroup == "P2")
            {
                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "LPC",
                    DateIssue = person.ProficiencyCheckDate,
                    DateExpire = person.ProficiencyValidUntil,
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });
                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "OPC",
                    DateIssue = person.ProficiencyCheckDateOPC,
                    DateExpire = person.ProficiencyValidUntilOPC,
                    Interval = 6,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "AVSEC-TRG-02",
                    DateIssue = trg02 != null ? trg02.DateIssue : null,
                    DateExpire = trg02 != null ? trg02.DateExpire : null,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });


                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "AVSEC-TRG-07B",
                    DateIssue = person.AviationSecurityIssueDate,
                    DateExpire = person.AviationSecurityExpireDate,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "SMS",
                    DateIssue = person.SMSIssueDate,
                    DateExpire = person.SMSExpireDate,
                    Interval = 24,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "SEPT-P",
                    DateIssue = person.SEPTPIssueDate,
                    DateExpire = person.SEPTPExpireDate,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "ESET",
                    DateIssue = person.SEPTIssueDate,
                    DateExpire = person.SEPTExpireDate,
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "DGR",
                    DateIssue = person.DangerousGoodsIssueDate,
                    DateExpire = person.DangerousGoodsExpireDate,
                    Interval = 24,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "CRM",
                    DateIssue = person.UpsetRecoveryTrainingIssueDate,
                    DateExpire = person.UpsetRecoveryTrainingExpireDate,
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "CCRM",
                    DateIssue = person.CCRMIssueDate,
                    DateExpire = person.CCRMExpireDate,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "HOT-WX",
                    DateIssue = person.HotWeatherOperationIssueDate,
                    DateExpire = person.HotWeatherOperationExpireDate,
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "COLD-WX",
                    DateIssue = person.ColdWeatherOperationIssueDate,
                    DateExpire = person.ColdWeatherOperationExpireDate,
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "FMT",
                    DateIssue = person.EGPWSIssueDate,
                    DateExpire = person.EGPWSExpireDate,
                    Interval = 24,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "LINE CHECK",
                    DateIssue = person.DateIssueNDT,
                    DateExpire = person.DateIssueNDT == null ? null : (Nullable<DateTime>)((DateTime)person.DateIssueNDT).AddYears(1),
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });
            }
            else
            {
                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "AVSEC-TRG-02",
                    DateIssue = trg02 != null ? trg02.DateIssue : null,
                    DateExpire = trg02 != null ? trg02.DateExpire : null,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });


                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "AVSEC-TRG-07B",
                    DateIssue = person.AviationSecurityIssueDate,
                    DateExpire = person.AviationSecurityExpireDate,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });
                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "SMS",
                    DateIssue = person.SMSIssueDate,
                    DateExpire = person.SMSExpireDate,
                    Interval = 24,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "SEPT-P",
                    DateIssue = person.SEPTPIssueDate,
                    DateExpire = person.SEPTPExpireDate,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "DGR",
                    DateIssue = person.DangerousGoodsIssueDate,
                    DateExpire = person.DangerousGoodsExpireDate,
                    Interval = 24,
                    ImageUrl = result.First().ImageUrl,
                });
                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "CRM",
                    DateIssue = person.UpsetRecoveryTrainingIssueDate,
                    DateExpire = person.UpsetRecoveryTrainingExpireDate,
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "CCRM",
                    DateIssue = person.CCRMIssueDate,
                    DateExpire = person.CCRMExpireDate,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "FIRST-AID",
                    DateIssue = person.FirstAidIssueDate,
                    DateExpire = person.FirstAidExpireDate,
                    Interval = 36,
                    ImageUrl = result.First().ImageUrl,
                });
                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "FMT",
                    DateIssue = person.EGPWSIssueDate,
                    DateExpire = person.EGPWSExpireDate,
                    Interval = 24,
                    ImageUrl = result.First().ImageUrl,
                });

                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "TYPE RECURRENT",
                    DateIssue = person.RecurrentIssueDate,
                    DateExpire = person.RecurrentIssueDate == null ? null : (Nullable<DateTime>)((DateTime)person.RecurrentIssueDate).AddYears(1),
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });
                ds.Add(new ViewCoursePeoplePassedRanked()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    JobGroup = employee.JobGroup,
                    NID = person.NID,
                    Title = "LINE CHECK",
                    DateIssue = person.DateIssueNDT,
                    DateExpire = person.DateIssueNDT == null ? null : (Nullable<DateTime>)((DateTime)person.DateIssueNDT).AddYears(1),
                    Interval = 12,
                    ImageUrl = result.First().ImageUrl,
                });

            }






            return new DataResponse()
            {
                Data = ds,//result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCoursesPassedHistory(int pid)
        {
            var result = await context.ViewCoursePeopleRankeds.Where(q => q.PersonId == pid && q.CoursePeopleStatusId == 1).OrderBy(q => q.CertificateType).ThenBy(q => q.DateStart).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }


        public async Task<DataResponse> GetMainJobGroups()
        {
            var result = await context.ViewJobGroupMains.OrderBy(q => q.FullCode2).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetEmployees(string root)
        {
            var query = from x in context.ViewEmployeeTrainings where x.InActive == false select x;
            if (root != "000")
                query = query.Where(q => q.JobGroupMainCode == root);
            var result = await query.OrderByDescending(q => q.MandatoryExpired).ThenBy(q => q.JobGroup).ThenBy(q => q.LastName).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCertificate(int id)
        {
            //var query = from x in context.ViewEmployeeTrainings select x;
            //if (root != "000")
            //    query = query.Where(q => q.JobGroupMainCode == root);
            //var result = await query.OrderByDescending(q => q.MandatoryExpired).ThenBy(q => q.JobGroup).ThenBy(q => q.LastName).ToListAsync();
            var obj = context.ViewCoursePeoplePassedRankeds.FirstOrDefault(q => q.Id == id);
            if (obj != null)
                return new DataResponse()
                {
                    Data = obj,
                    IsSuccess = true,
                };
            else
                return new DataResponse()
                {
                    Data = new ViewCoursePeople() { Id = -1, },
                    IsSuccess = true,
                };
        }


        public async Task<DataResponse> DeleteCourseType(int id)
        {
            var view = await context.ViewCourseTypes.Where(q => q.Id == id).FirstOrDefaultAsync();
            if (view.CoursesCount > 0)
            {
                return new DataResponse()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<string>() { "Please remove related course(s)." }
                };
            }
            var obj = await context.CourseTypes.Where(q => q.Id == id).FirstOrDefaultAsync();
            context.CourseTypes.Remove(obj);
            var saveResult = await context.SaveAsync();
            if (!saveResult.IsSuccess)
                return saveResult;

            return new DataResponse()
            {
                IsSuccess = true,
                Data = obj,
            };
        }

        public async Task<DataResponse> DeleteCourse(int id)
        {
            var view = await context.CoursePeoples.Where(q => q.CourseId == id).FirstOrDefaultAsync();
            if (view != null)
            {
                return new DataResponse()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<string>() { "Please remove related People." }
                };
            }
            var obj = await context.Courses.Where(q => q.Id == id).FirstOrDefaultAsync();
            context.Courses.Remove(obj);
            var saveResult = await context.SaveAsync();
            if (!saveResult.IsSuccess)
                return saveResult;

            return new DataResponse()
            {
                IsSuccess = true,
                Data = obj,
            };
        }

        public async Task<DataResponse> DeleteCoursePeople(int pid, int cid)
        {

            var obj = await context.CoursePeoples.Where(q => q.CourseId == cid && q.PersonId == pid).FirstOrDefaultAsync();
            var employee = await context.ViewEmployeeAbs.Where(q => q.PersonId == pid).Select(q => q.Id).FirstOrDefaultAsync();
            context.CoursePeoples.Remove(obj);
            var sessionFdps = await context.CourseSessionFDPs.Where(q => q.CourseId == cid && q.EmployeeId == employee).Select(q => q.FDPId).ToListAsync();
            var fdps = await context.FDPs.Where(q => sessionFdps.Contains(q.Id)).ToListAsync();
            context.FDPs.RemoveRange(fdps);

            var saveResult = await context.SaveAsync();
            if (!saveResult.IsSuccess)
                return saveResult;

            return new DataResponse()
            {
                IsSuccess = true,
                Data = obj,
            };
        }

        public async Task<DataResponse> SaveCourseType(ViewModels.CourseTypeViewModel dto)
        {
            CourseType entity = null;
            var _t = dto.Title.Replace(" ", "").Replace("-", "").Replace("/", "").Trim().ToLower();
            var _exist = await context.CourseTypes.FirstOrDefaultAsync(q => q.Id != dto.Id && q.Title.Replace(" ", "").Replace("-", "").Replace("/", "").Trim().ToLower() == _t);
            if (_exist != null)
            {
                return new DataResponse()
                {
                    Data = dto,
                    IsSuccess = false,
                    Errors = new List<string>() { "Duplicated Title Found" }
                };
            }

            if (dto.Id == -1)
            {
                entity = new CourseType();
                context.CourseTypes.Add(entity);
            }

            else
            {
                entity = await context.CourseTypes.FirstOrDefaultAsync(q => q.Id == dto.Id);

            }

            if (entity == null)
                return new DataResponse()
                {
                    Data = dto,
                    IsSuccess = false,
                    Errors = new List<string>() { "entity not found" }
                };

            //ViewModels.Location.Fill(entity, dto);
            ViewModels.CourseTypeViewModel.Fill(entity, dto);

            if (dto.Id != -1)
            {
                var djgs = await context.CourseTypeJobGroups.Where(q => q.CourseTypeId == entity.Id).ToListAsync();
                context.CourseTypeJobGroups.RemoveRange(djgs);
            }

            var jgsIds = dto.JobGroups.Select(q => q.Id);
            var jgs = await context.JobGroups.Where(q => jgsIds.Contains(q.Id)).ToListAsync();
            foreach (var x in jgs)
            {
                entity.CourseTypeJobGroups.Add(new CourseTypeJobGroup()
                {
                    JobGroupId = x.Id,
                });
            }

            await context.SaveChangesAsync();
            dto.Id = entity.Id;
            return new DataResponse()
            {
                IsSuccess = true,
                Data = dto,
            };
        }


        public async Task<DataResponse> SaveCourseTypeJobGroup(int tid, int gid, int man, int sel)
        {
            CourseTypeJobGroup cj = null;
            if (sel == 0)
            {
                cj = await context.CourseTypeJobGroups.Where(q => q.CourseTypeId == tid && q.JobGroupId == gid).FirstOrDefaultAsync();
                if (cj != null)
                    context.CourseTypeJobGroups.Remove(cj);

                var childs = await context.JobGroups.Where(q => q.ParentId == gid).ToListAsync();
                var childIds = childs.Select(q => q.Id).ToList();

                var childscj = await context.CourseTypeJobGroups.Where(q => q.CourseTypeId == tid && childIds.Contains(q.JobGroupId)).ToListAsync();
                if (childscj != null && childscj.Count > 0)
                    context.CourseTypeJobGroups.RemoveRange(childscj);

            }
            else
            {
                var groupIds = new List<int>();

                var childs = await context.JobGroups.Where(q => q.ParentId == gid).ToListAsync();
                groupIds = childs.Select(q => q.Id).ToList();
                groupIds.Add(gid);

                foreach (var _gid in groupIds)
                {
                    cj = await context.CourseTypeJobGroups.Where(q => q.CourseTypeId == tid && q.JobGroupId == _gid).FirstOrDefaultAsync();
                    if (cj != null)
                    {
                        cj.Mandatory = man == 1;
                    }
                    else
                    {
                        cj = new CourseTypeJobGroup()
                        {
                            JobGroupId = _gid,
                            CourseTypeId = tid,
                            Mandatory = man == 1,

                        };
                        context.CourseTypeJobGroups.Add(cj);
                    }
                }



            }





            await context.SaveChangesAsync();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = cj,
            };
        }

        public async Task<DataResponse> GetCourseDocs(int id)
        {
            var docs = await context.ViewCourseDocuments.Where(q => q.CourseId == id).ToListAsync();

            return new DataResponse()
            {
                Data = docs,
                IsSuccess = true,
            };
        }
        public async Task<DataResponse> SaveCourse(ViewModels.CourseViewModel dto)
        {
            Course entity = null;

            if (dto.Id == -1)
            {
                entity = new Course();
                context.Courses.Add(entity);
            }

            else
            {
                entity = await context.Courses.FirstOrDefaultAsync(q => q.Id == dto.Id);

            }

            if (entity == null)
                return new DataResponse()
                {
                    Data = dto,
                    IsSuccess = false,
                    Errors = new List<string>() { "entity not found" }
                };

            //ViewModels.Location.Fill(entity, dto);
            entity.CourseTypeId = dto.CourseTypeId;
            entity.DateStart = (DateTime)DateObject.ConvertToDate(dto.DateStart).Date;
            entity.DateEnd = (DateTime)DateObject.ConvertToDate(dto.DateEnd).Date;
            entity.Instructor = dto.Instructor;
            entity.CurrencyId = dto.CurrencyId;
            entity.Location = dto.Location;
            entity.OrganizationId = dto.OrganizationId;
            entity.Duration = dto.Duration;
            entity.DurationUnitId = dto.DurationUnitId;
            entity.Remark = dto.Remark;
            entity.TrainingDirector = dto.TrainingDirector;
            entity.Title = dto.Title;
            entity.Recurrent = dto.Recurrent;
            entity.Interval = dto.Interval;
            entity.CalanderTypeId = dto.CalanderTypeId;
            entity.IsGeneral = dto.IsGeneral;
            entity.CustomerId = dto.CustomerId;
            entity.No = dto.No;
            entity.IsNotificationEnabled = dto.IsNotificationEnabled;
            entity.StatusId = dto.StatusId;
            entity.HoldingType = dto.HoldingType;
            entity.Instructor2 = dto.Instructor2Id;
            entity.Cost = dto.Cost;
            entity.HoldingType = dto.HoldingType;
            entity.SendLetter = dto.SendLetter;
            entity.Financial = dto.Financial;
            entity.InForm = dto.InForm;
            entity.Certificate = dto.Certificate;
            if (dto.Id == -1)
            {
                if (dto.Sessions.Count > 0)
                {
                    foreach (var s in dto.Sessions)
                    {
                        var dtobj = DateObject.ConvertToDateTimeSession(s);
                        entity.CourseSessions.Add(new CourseSession()
                        {
                            Done = false,
                            Key = s,
                            DateStart = dtobj[0].Date,
                            DateStartUtc = dtobj[0].DateUtc,
                            DateEnd = dtobj[1].Date,
                            DateEndUtc = dtobj[1].DateUtc,
                        });
                    }
                }

                if (dto.Syllabi.Count > 0)
                {
                    foreach (var x in dto.Syllabi)
                    {
                        entity.CourseSyllabus.Add(new CourseSyllabu()
                        {
                            Duration = x.Duration,
                            Title = x.Title,
                        });
                    }
                }
            }
            else
            {
                var _syllabi = await context.CourseSyllabus.Where(q => q.CourseId == dto.Id).ToListAsync();
                var _syllabiIds = _syllabi.Select(q => q.Id).ToList();
                var _dtoIds = dto.Syllabi.Select(q => q.Id).ToList();
                var _deletedSyl = _syllabi.Where(q => !_dtoIds.Contains(q.Id)).ToList();
                context.CourseSyllabus.RemoveRange(_deletedSyl);

                var newSyllabi = dto.Syllabi.Where(q => q.Id < 0).ToList();
                foreach(var x in newSyllabi)
                {
                    entity.CourseSyllabus.Add(new CourseSyllabu() { Duration=x.Duration, Title=x.Title });
                }


                var _sessions = await context.CourseSessions.Where(q => q.CourseId == dto.Id).ToListAsync();
                var _sessionKeys = _sessions.Select(q => q.Key).ToList();


                var _deleted = _sessions.Where(q => !dto.Sessions.Contains(q.Key)).ToList();
                var _deletedKeys = _deleted.Select(q => q.Key).ToList();

                var sessionFdps = await context.CourseSessionFDPs.Where(q => q.CourseId == dto.Id && _deletedKeys.Contains(q.SessionKey)).ToListAsync();
                var fdpIds = sessionFdps.Select(q => q.FDPId).ToList();
                var fdps = await context.FDPs.Where(q => fdpIds.Contains(q.Id)).ToListAsync();
                context.FDPs.RemoveRange(fdps);


                context.CourseSessions.RemoveRange(_deleted);

                var _newSessions = dto.Sessions.Where(q => !_sessionKeys.Contains(q)).ToList();
                foreach (var s in _newSessions)
                {
                    var dtobj = DateObject.ConvertToDateTimeSession(s);
                    entity.CourseSessions.Add(new CourseSession()
                    {
                        Done = false,
                        Key = s,
                        DateStart = dtobj[0].Date,
                        DateStartUtc = dtobj[0].DateUtc,
                        DateEnd = dtobj[1].Date,
                        DateEndUtc = dtobj[1].DateUtc,
                    });
                }
            }

            //pasco
            /* var docs = await context.CourseDocuments.Where(q => q.CourseId == dto.Id).ToListAsync();
             var docids = dto.Documents.Where(q => q.Id > 0).Select(q => q.Id).ToList();
             var deleted = docs.Where(q => !docids.Contains(q.Id)).ToList();
             context.CourseDocuments.RemoveRange(deleted);

             var newdocs = dto.Documents.Where(q => q.Id < 0).ToList();

             foreach (var x in newdocs)
                 entity.CourseDocuments.Add(new CourseDocument()
                 {
                     FileUrl = x.FileUrl,
                     Remark = x.Remark,
                     TypeId = x.TypeId,
                 });*/
            /////////////////////////////////////////


            await context.SaveChangesAsync();
            dto.Id = entity.Id;
            return new DataResponse()
            {
                IsSuccess = true,
                Data = dto,
            };
        }

        public async Task<DataResponse> SaveCertificate(ViewModels.CertificateViewModel dto)
        {
            var _dateStart = (DateTime)DateObject.ConvertToDate(dto.DateStart).Date;
            var _dateEnd = (DateTime)DateObject.ConvertToDate(dto.DateEnd).Date;

            Course entity = await context.Courses.Where(q => q.DateStart == _dateStart && q.DateEnd == _dateEnd && q.CourseTypeId == dto.CourseTypeId
            && q.OrganizationId == dto.OrganizationId).FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new Course();
                context.Courses.Add(entity);
                entity.CourseTypeId = dto.CourseTypeId;
                entity.DateStart = (DateTime)DateObject.ConvertToDate(dto.DateStart).Date;
                entity.DateEnd = (DateTime)DateObject.ConvertToDate(dto.DateEnd).Date;
                entity.Instructor = dto.Instructor;
                entity.Location = dto.Location;
                entity.OrganizationId = dto.OrganizationId;
                entity.Duration = dto.Duration;
                entity.DurationUnitId = dto.DurationUnitId;
                entity.Remark = dto.Remark;
                entity.TrainingDirector = dto.TrainingDirector;
                entity.Title = dto.Title;
                entity.Recurrent = dto.Recurrent;
                entity.Interval = dto.Interval;
                entity.CalanderTypeId = dto.CalanderTypeId;
                entity.IsGeneral = dto.IsGeneral;
                entity.CustomerId = dto.CustomerId;
                entity.No = dto.No;
                entity.IsNotificationEnabled = dto.IsNotificationEnabled;
                entity.StatusId = 3;
            }
            if (dto.PersonId != null)
            {
                var cp = await context.CoursePeoples.Where(q => q.PersonId == dto.PersonId && q.CourseId == entity.Id).FirstOrDefaultAsync();
                if (cp == null)
                {
                    cp = new CoursePeople()
                    {
                        PersonId = dto.PersonId,
                        StatusId = 1,
                        DateStatus = DateTime.Now,
                        DateExpire = (DateTime)DateObject.ConvertToDate(dto.DateExpire).Date,
                        DateIssue = (DateTime)DateObject.ConvertToDate(dto.DateIssue).Date,
                        CertificateNo = dto.CertificateNo,
                    };
                    entity.CoursePeoples.Add(cp);
                }
                else
                {
                    cp.DateExpire = (DateTime)DateObject.ConvertToDate(dto.DateExpire).Date;
                    cp.DateIssue = (DateTime)DateObject.ConvertToDate(dto.DateIssue).Date;
                    cp.CertificateNo = dto.CertificateNo;
                    cp.StatusId = 1;
                }
                //////////////////////////
                var person = await context.People.Where(q => q.Id == cp.PersonId).FirstOrDefaultAsync();
                var ct = await context.ViewCourseTypes.Where(q => q.Id == dto.CourseTypeId).FirstOrDefaultAsync();
                //12-03
                if (ct != null)
                {
                    switch (ct.CertificateType)
                    {


                        //CYBER SECURITY
                        case "CYBER SECURITY":
                            if ((DateTime)cp.DateExpire > person.ExpireDate2 || person.ExpireDate2 == null)
                            {
                                person.ExpireDate2 = cp.DateExpire;
                                person.IssueDate2 = cp.DateIssue;
                            }
                            break;

                        //ERP
                        case "ERP":
                            if ((DateTime)cp.DateExpire > person.ERPExpireDate || person.ERPExpireDate == null)
                            {
                                person.ERPExpireDate = cp.DateExpire;
                                person.ERPIssueDate = cp.DateIssue;
                            }
                            break;
                        //HF
                        case "HF":
                            if ((DateTime)cp.DateExpire > person.HFExpireDate || person.HFExpireDate == null)
                            {
                                person.HFExpireDate = cp.DateExpire;
                                person.HFIssueDate = cp.DateIssue;
                            }
                            break;
                        //MEL/CDL
                        case "MEL/CDL":
                            if ((DateTime)cp.DateExpire > person.MELExpireDate || person.MELExpireDate == null)
                            {
                                person.MELExpireDate = cp.DateExpire;
                                person.MELIssueDate = cp.DateIssue;
                            }
                            break;
                        //METEOROLOGY
                        case "METEOROLOGY":
                            if ((DateTime)cp.DateExpire > person.METExpireDate || person.METExpireDate == null)
                            {
                                person.METExpireDate = cp.DateExpire;
                                person.METIssueDate = cp.DateIssue;
                            }
                            break;
                        //PERFORMANCE
                        case "PERFORMANCE":
                            if ((DateTime)cp.DateExpire > person.PERExpireDate || person.PERExpireDate == null)
                            {
                                person.PERExpireDate = cp.DateExpire;
                                person.PERIssueDate = cp.DateIssue;
                            }
                            break;
                        //RADIO COMMUNICATION
                        case "RADIO COMMUNICATION":
                            if ((DateTime)cp.DateExpire > person.LRCExpireDate || person.LRCExpireDate == null)
                            {
                                person.LRCExpireDate = cp.DateExpire;
                                person.LRCIssueDate = cp.DateIssue;
                            }
                            break;
                        //SITA
                        case "SITA":
                            if ((DateTime)cp.DateExpire > person.RSPExpireDate || person.RSPExpireDate == null)
                            {
                                person.RSPExpireDate = cp.DateExpire;
                                person.RSPIssueDate = cp.DateIssue;
                            }
                            break;
                        //WEIGHT AND BALANCE
                        case "WEIGHT AND BALANCE":
                            if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                            {
                                person.MBExpireDate = cp.DateExpire;
                                person.MBIssueDate = cp.DateIssue;
                            }
                            break;
                        //AIRSIDE SAFETY AND DRIVING
                        case "AIRSIDE SAFETY AND DRIVING":
                            if ((DateTime)cp.DateExpire > person.ASDExpireDate || person.ASDExpireDate == null)
                            {
                                person.ASDExpireDate = cp.DateExpire;
                                person.ASDIssueDate = cp.DateIssue;
                            }
                            break;
                        //AIRSIDE SAFETY AND DRIVING (IKA)
                        case "AIRSIDE SAFETY AND DRIVING (IKA)":
                            if ((DateTime)cp.DateExpire > person.ExpireDate1 || person.ExpireDate1 == null)
                            {
                                person.ExpireDate1 = cp.DateExpire;
                                person.IssueDate1 = cp.DateIssue;
                            }
                            break;
                        //GOM
                        case "GOM":
                            if ((DateTime)cp.DateExpire > person.GOMExpireDate || person.GOMExpireDate == null)
                            {
                                person.GOMExpireDate = cp.DateExpire;
                                person.GOMIssueDate = cp.DateIssue;
                            }
                            break;
                        //AIRPORT SERVICE FAMILIARIZATION
                        case "AIRPORT SERVICE FAMILIARIZATION":
                            if ((DateTime)cp.DateExpire > person.ASFExpireDate || person.ASFExpireDate == null)
                            {
                                person.ASFExpireDate = cp.DateExpire;
                                person.ASFIssueDate = cp.DateIssue;
                            }
                            break;
                        //CUSTOMER CARE
                        case "CUSTOMER CARE":
                            if ((DateTime)cp.DateExpire > person.CCExpireDate || person.CCExpireDate == null)
                            {
                                person.CCExpireDate = cp.DateExpire;
                                person.CCIssueDate = cp.DateIssue;
                            }
                            break;
                        //LOAD SHEET
                        case "LOAD SHEET":
                            if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                            {
                                person.MBExpireDate = cp.DateExpire;
                                person.MBIssueDate = cp.DateIssue;
                            }
                            break;
                        //PASSENGER SERVICE
                        case "PASSENGER SERVICE":
                            if ((DateTime)cp.DateExpire > person.PSExpireDate || person.PSExpireDate == null)
                            {
                                person.PSExpireDate = cp.DateExpire;
                                person.PSIssueDate = cp.DateIssue;
                            }
                            break;

                        //DRM
                        case "DRM":
                            if ((DateTime)cp.DateExpire > person.DRMExpireDate || person.DRMExpireDate == null)
                            {
                                person.DRMExpireDate = cp.DateExpire;
                                person.DRMIssueDate = cp.DateIssue;
                            }
                            break;
                        //ANNEX
                        case "ANNEX":
                            if ((DateTime)cp.DateExpire > person.ANNEXExpireDate || person.ANNEXExpireDate == null)
                            {
                                person.ANNEXExpireDate = cp.DateExpire;
                                person.ANNEXIssueDate = cp.DateIssue;
                            }
                            break;
                        //FRMS
                        case "FRMS":
                            if ((DateTime)cp.DateExpire > person.TypeAirbusExpireDate || person.TypeAirbusExpireDate == null)
                            {
                                person.TypeAirbusExpireDate = cp.DateExpire;
                                person.TypeAirbusIssueDate = cp.DateIssue;
                            }
                            break;
                        //DANGEROUS GOODS
                        case "DANGEROUS GOODS":
                            if ((DateTime)cp.DateExpire > person.DangerousGoodsExpireDate || person.DangerousGoodsExpireDate == null)
                            {
                                person.DangerousGoodsExpireDate = cp.DateExpire;
                                person.DangerousGoodsIssueDate = cp.DateIssue;
                            }
                            break;
                        //1	SEPT-P
                        case "SEPT":
                            if ((DateTime)cp.DateExpire > person.SEPTPExpireDate || person.SEPTPExpireDate == null)
                            {
                                person.SEPTPExpireDate = cp.DateExpire;
                                person.SEPTPIssueDate = cp.DateIssue;
                            }
                            break;
                        //2   SEPT - T
                        case "ANNUAL SEPT":
                            if ((DateTime)cp.DateExpire > person.SEPTExpireDate || person.SEPTExpireDate == null)
                            {
                                person.SEPTExpireDate = cp.DateExpire;
                                person.SEPTIssueDate = cp.DateIssue;
                            }
                            break;
                        //4	CRM
                        case "CRM":
                            if ((DateTime)cp.DateExpire > person.UpsetRecoveryTrainingExpireDate || person.UpsetRecoveryTrainingExpireDate == null)
                            {
                                person.UpsetRecoveryTrainingExpireDate = cp.DateExpire;
                                person.UpsetRecoveryTrainingIssueDate = cp.DateIssue;
                            }
                            break;
                        //5	CCRM
                        case "CCRM":
                            if ((DateTime)cp.DateExpire > person.CCRMExpireDate || person.CCRMExpireDate == null)
                            {
                                person.CCRMExpireDate = cp.DateExpire;
                                person.CCRMIssueDate = cp.DateIssue;
                            }
                            break;
                        //6	SMS
                        case "SMS":
                            if ((DateTime)cp.DateExpire > person.SMSExpireDate || person.SMSExpireDate == null)
                            {
                                person.SMSExpireDate = cp.DateExpire;
                                person.SMSIssueDate = cp.DateIssue;
                            }
                            break;
                        //7	AV-SEC
                        case "AVIATION SECURITY":
                            if ((DateTime)cp.DateExpire > person.AviationSecurityExpireDate || person.AviationSecurityExpireDate == null)
                            {
                                person.AviationSecurityExpireDate = cp.DateExpire;
                                person.AviationSecurityIssueDate = cp.DateIssue;
                            }
                            break;
                        //8	COLD-WX
                        case "COLD WEATHER OPERATION":
                            if ((DateTime)cp.DateExpire > person.ColdWeatherOperationExpireDate || person.ColdWeatherOperationExpireDate == null)
                            {
                                person.ColdWeatherOperationExpireDate = cp.DateExpire;
                                person.ColdWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //9	HOT-WX
                        case "HOT WEATHER OPERATION":
                            if ((DateTime)cp.DateExpire > person.HotWeatherOperationExpireDate || person.HotWeatherOperationExpireDate == null)
                            {
                                person.HotWeatherOperationExpireDate = cp.DateExpire;
                                person.HotWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //10	FIRSTAID
                        case "FIRST AID":
                            if ((DateTime)cp.DateExpire > person.FirstAidExpireDate || person.FirstAidExpireDate == null)
                            {
                                person.FirstAidExpireDate = cp.DateExpire;
                                person.FirstAidIssueDate = cp.DateIssue;
                            }
                            break;
                        ////lpc
                        //case 100:
                        //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntil || person.ProficiencyValidUntil == null)
                        //    {
                        //        person.ProficiencyValidUntil = cp.DateExpire;
                        //        person.ProficiencyCheckDate = cp.DateIssue;
                        //    }
                        //    break;
                        ////opc
                        //case 101:
                        //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntilOPC || person.ProficiencyValidUntilOPC == null)
                        //    {
                        //        person.ProficiencyValidUntilOPC = cp.DateExpire;
                        //        person.ProficiencyCheckDateOPC = cp.DateIssue;
                        //    }
                        //    break;
                        ////lpr
                        //case 102:
                        //    if ((DateTime)cp.DateExpire > person.ICAOLPRValidUntil || person.ICAOLPRValidUntil == null)
                        //    {
                        //        person.ICAOLPRValidUntil = cp.DateExpire;
                        //        // person.ProficiencyCheckDateOPC = cp.DateIssue;
                        //    }
                        //    break;

                        //grt
                        case "GRT":
                            if ((DateTime)cp.DateExpire > person.DateCaoCardExpire || person.DateCaoCardExpire == null)
                            {
                                person.DateCaoCardExpire = cp.DateExpire;
                                person.DateCaoCardIssue = cp.DateIssue;
                            }
                            break;
                        //recurrent
                        case "RECURRENT 737":
                            if ((DateTime)cp.DateExpire > person.Type737ExpireDate || person.Type737ExpireDate == null)
                            {
                                person.Type737ExpireDate = cp.DateExpire;
                                person.Type737IssueDate = cp.DateIssue;
                            }
                            break;
                        //fmt
                        case "FMT":
                            if ((DateTime)cp.DateExpire > person.EGPWSExpireDate || person.EGPWSExpireDate == null)
                            {
                                person.EGPWSExpireDate = cp.DateExpire;
                                person.EGPWSIssueDate = cp.DateIssue;
                            }
                            break;
                        case "FMTD":
                            if ((DateTime)cp.DateExpire > person.FMTDExpireDate || person.FMTDExpireDate == null)
                            {
                                person.FMTDExpireDate = cp.DateExpire;
                                person.FMTDIssueDate = cp.DateIssue;
                            }
                            break;
                        case "LINE":
                            if ((DateTime)cp.DateExpire > person.LineExpireDate || person.LineExpireDate == null)
                            {
                                person.LineExpireDate = cp.DateExpire;
                                person.LineIssueDate = cp.DateIssue;
                            }
                            break;
                        default:
                            break;
                    }
                    /*switch (ct.CertificateTypeId)
                    {
                        //dg
                        case 3:
                            if ((DateTime)cp.DateExpire > person.DangerousGoodsExpireDate)
                            {
                                person.DangerousGoodsExpireDate = cp.DateExpire;
                                person.DangerousGoodsIssueDate = cp.DateIssue;
                            }
                            break;
                        //1	SEPT-P
                        case 1:
                            if ((DateTime)cp.DateExpire > person.SEPTPExpireDate)
                            {
                                person.SEPTPExpireDate = cp.DateExpire;
                                person.SEPTPIssueDate = cp.DateIssue;
                            }
                            break;
                        //2   SEPT - T
                        case 2:
                            if ((DateTime)cp.DateExpire > person.SEPTExpireDate)
                            {
                                person.SEPTExpireDate = cp.DateExpire;
                                person.SEPTIssueDate = cp.DateIssue;
                            }
                            break;
                        //4	CRM
                        case 4:
                            if ((DateTime)cp.DateExpire > person.UpsetRecoveryTrainingExpireDate)
                            {
                                person.UpsetRecoveryTrainingExpireDate = cp.DateExpire;
                                person.UpsetRecoveryTrainingIssueDate = cp.DateIssue;
                            }
                            break;
                        //5	CCRM
                        case 5:
                            if ((DateTime)cp.DateExpire > person.CCRMExpireDate)
                            {
                                person.CCRMExpireDate = cp.DateExpire;
                                person.CCRMIssueDate = cp.DateIssue;
                            }
                            break;
                        //6	SMS
                        case 6:
                            if ((DateTime)cp.DateExpire > person.SMSExpireDate)
                            {
                                person.SMSExpireDate = cp.DateExpire;
                                person.SMSIssueDate = cp.DateIssue;
                            }
                            break;
                        //7	AV-SEC
                        case 7:
                            if ((DateTime)cp.DateExpire > person.AviationSecurityExpireDate)
                            {
                                person.AviationSecurityExpireDate = cp.DateExpire;
                                person.AviationSecurityIssueDate = cp.DateIssue;
                            }
                            break;
                        //8	COLD-WX
                        case 8:
                            if ((DateTime)cp.DateExpire > person.ColdWeatherOperationExpireDate)
                            {
                                person.ColdWeatherOperationExpireDate = cp.DateExpire;
                                person.ColdWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //9	HOT-WX
                        case 9:
                            if ((DateTime)cp.DateExpire > person.HotWeatherOperationExpireDate)
                            {
                                person.HotWeatherOperationExpireDate = cp.DateExpire;
                                person.HotWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //10	FIRSTAID
                        case 10:
                            if ((DateTime)cp.DateExpire > person.FirstAidExpireDate)
                            {
                                person.FirstAidExpireDate = cp.DateExpire;
                                person.FirstAidIssueDate = cp.DateIssue;
                            }
                            break;
                        case 105:
                            if ((DateTime)cp.DateExpire > person.LineExpireDate)
                            {
                                person.LineExpireDate = cp.DateExpire;
                                person.LineIssueDate = cp.DateIssue;
                            }
                            break;
                        default:
                            break;
                    }*/
                }


                /////////////////////////


            }




            await context.SaveChangesAsync();
            dto.Id = entity.Id;
            var result = await context.ViewCoursePeopleRankeds.Where(q => q.PersonId == dto.PersonId && q.CourseId == entity.Id).FirstOrDefaultAsync();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result,
            };
        }

        public async Task<DataResponse> SaveCoursePeople(dynamic dto)
        {
            int courseId = Convert.ToInt32(dto.Id);
            string pid = Convert.ToString(dto.pid);
            // string eid = Convert.ToString(dto.eid);

            var personIds = pid.Split('-').Select(q => Convert.ToInt32(q)).ToList();
            // var employeeIds = eid.Split('-').Select(q => Convert.ToInt32(q)).ToList();

            var exists = await context.CoursePeoples.Where(q => q.CourseId == courseId).Select(q => q.PersonId).ToListAsync();
            var newids = personIds.Where(q => !exists.Contains(q)).ToList();

            foreach (var id in newids)
            {
                context.CoursePeoples.Add(new CoursePeople()
                {
                    CourseId = courseId,
                    PersonId = id,
                    StatusId = -1,
                });
            }

            await context.SaveChangesAsync();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = dto,
            };
        }
       
    public async Task<DataResponse> SaveSyllabus(dynamic dto)
        {
            int Id = Convert.ToInt32(dto.Id);
            string Remark = Convert.ToString(dto.Remark);
            int Done = Convert.ToInt32(dto.Done);
            int Instructor = Convert.ToInt32(dto.Instructor);
            string Session = Convert.ToString(dto.Session);
            var syllabus = await context.CourseSyllabus.Where(q => q.Id == Id).FirstOrDefaultAsync();
            syllabus.Remark = Remark;
            syllabus.Status = Done;
            syllabus.InstructorId = Instructor;
            syllabus.SessionKey = Session;

        

            await context.SaveChangesAsync();
            var syll = await context.ViewSyllabus .Where(q => q.Id == Id).FirstOrDefaultAsync();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = syll,
            };
        }


        public async Task<DataResponse> SaveCourseSessionPresence(dynamic dto)
        {
            int courseId = Convert.ToInt32(dto.cid);
            int pid = Convert.ToInt32(dto.pid);
            string sid = Convert.ToString(dto.sid);
            sid = sid.Replace("Session", "");



            var exists = await context.CourseSessionPresences.Where(q => q.CourseId == courseId && q.PersonId == pid && q.SessionKey == sid).FirstOrDefaultAsync();

            if (exists != null)
            {
                context.CourseSessionPresences.Remove(exists);
            }
            else
            {
                context.CourseSessionPresences.Add(new CourseSessionPresence()
                {
                    PersonId = pid,
                    SessionKey = sid,
                    CourseId = courseId,
                    Date = DateTime.Now
                });
            }

            await context.SaveChangesAsync();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = dto,
            };
        }


        public async Task<DataResponse> UpdateCoursePeopleStatus(CoursePeopleStatusViewModel dto)
        {
            CoursePeople cp = null;

            if (dto.Id != -1)
                cp = await context.CoursePeoples.Where(q => q.Id == dto.Id).FirstOrDefaultAsync();
            else
                cp = await context.CoursePeoples.Where(q => q.PersonId == dto.PersonId && q.CourseId == dto.CourseId).FirstOrDefaultAsync();
            if (dto.StatusId != cp.StatusId)
                cp.DateStatus = DateTime.Now;

            //-1: UnKnown 0:Failed 1:Passed
            if (dto.StatusId != 1)
            {
                cp.DateIssue = null;
                cp.DateExpire = null;
                cp.CertificateNo = null;


            }
            else
            {
                cp.DateExpire = string.IsNullOrEmpty(dto.Expire) ? null : DateObject.ConvertToDate(dto.Expire).Date;
                cp.DateIssue = string.IsNullOrEmpty(dto.Issue) ? null : DateObject.ConvertToDate(dto.Issue).Date;
                cp.CertificateNo = dto.No;
                if (string.IsNullOrEmpty(cp.CertificateNo))
                    cp.CertificateNo = "FPC-" + cp.Id;

            }

            cp.StatusId = dto.StatusId;
            cp.StatusRemark = dto.Remark;

            if (dto.StatusId == 1 && cp.DateIssue != null && cp.DateExpire != null && !string.IsNullOrEmpty(cp.CertificateNo))
            {

                var person = await context.People.Where(q => q.Id == cp.PersonId).FirstOrDefaultAsync();
                var course = await context.ViewCourseNews.Where(q => q.Id == cp.CourseId).FirstOrDefaultAsync();
                switch (course.CertificateType)
                {


                    //CYBER SECURITY
                    case "CYBER SECURITY":
                        if ((DateTime)cp.DateExpire > person.ExpireDate2 || person.ExpireDate2 == null)
                        {
                            person.ExpireDate2 = cp.DateExpire;
                            person.IssueDate2 = cp.DateIssue;
                        }
                        break;

                    //ERP
                    case "ERP":
                        if ((DateTime)cp.DateExpire > person.ERPExpireDate || person.ERPExpireDate == null)
                        {
                            person.ERPExpireDate = cp.DateExpire;
                            person.ERPIssueDate = cp.DateIssue;
                        }
                        break;
                    //HF
                    case "HF":
                        if ((DateTime)cp.DateExpire > person.HFExpireDate || person.HFExpireDate == null)
                        {
                            person.HFExpireDate = cp.DateExpire;
                            person.HFIssueDate = cp.DateIssue;
                        }
                        break;
                    //MEL/CDL
                    case "MEL/CDL":
                        if ((DateTime)cp.DateExpire > person.MELExpireDate || person.MELExpireDate == null)
                        {
                            person.MELExpireDate = cp.DateExpire;
                            person.MELIssueDate = cp.DateIssue;
                        }
                        break;
                    //METEOROLOGY
                    case "METEOROLOGY":
                        if ((DateTime)cp.DateExpire > person.METExpireDate || person.METExpireDate == null)
                        {
                            person.METExpireDate = cp.DateExpire;
                            person.METIssueDate = cp.DateIssue;
                        }
                        break;
                    //PERFORMANCE
                    case "PERFORMANCE":
                        if ((DateTime)cp.DateExpire > person.PERExpireDate || person.PERExpireDate == null)
                        {
                            person.PERExpireDate = cp.DateExpire;
                            person.PERIssueDate = cp.DateIssue;
                        }
                        break;
                    //RADIO COMMUNICATION
                    case "RADIO COMMUNICATION":
                        if ((DateTime)cp.DateExpire > person.LRCExpireDate || person.LRCExpireDate == null)
                        {
                            person.LRCExpireDate = cp.DateExpire;
                            person.LRCIssueDate = cp.DateIssue;
                        }
                        break;
                    //SITA
                    case "SITA":
                        if ((DateTime)cp.DateExpire > person.RSPExpireDate || person.RSPExpireDate == null)
                        {
                            person.RSPExpireDate = cp.DateExpire;
                            person.RSPIssueDate = cp.DateIssue;
                        }
                        break;
                    //WEIGHT AND BALANCE
                    case "WEIGHT AND BALANCE":
                        if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                        {
                            person.MBExpireDate = cp.DateExpire;
                            person.MBIssueDate = cp.DateIssue;
                        }
                        break;
                    //AIRSIDE SAFETY AND DRIVING
                    case "AIRSIDE SAFETY AND DRIVING":
                        if ((DateTime)cp.DateExpire > person.ASDExpireDate || person.ASDExpireDate == null)
                        {
                            person.ASDExpireDate = cp.DateExpire;
                            person.ASDIssueDate = cp.DateIssue;
                        }
                        break;
                    //AIRSIDE SAFETY AND DRIVING (IKA)
                    case "AIRSIDE SAFETY AND DRIVING (IKA)":
                        if ((DateTime)cp.DateExpire > person.ExpireDate1 || person.ExpireDate1 == null)
                        {
                            person.ExpireDate1 = cp.DateExpire;
                            person.IssueDate1 = cp.DateIssue;
                        }
                        break;
                    //GOM
                    case "GOM":
                        if ((DateTime)cp.DateExpire > person.GOMExpireDate || person.GOMExpireDate == null)
                        {
                            person.GOMExpireDate = cp.DateExpire;
                            person.GOMIssueDate = cp.DateIssue;
                        }
                        break;
                    //AIRPORT SERVICE FAMILIARIZATION
                    case "AIRPORT SERVICE FAMILIARIZATION":
                        if ((DateTime)cp.DateExpire > person.ASFExpireDate || person.ASFExpireDate == null)
                        {
                            person.ASFExpireDate = cp.DateExpire;
                            person.ASFIssueDate = cp.DateIssue;
                        }
                        break;
                    //CUSTOMER CARE
                    case "CUSTOMER CARE":
                        if ((DateTime)cp.DateExpire > person.CCExpireDate || person.CCExpireDate == null)
                        {
                            person.CCExpireDate = cp.DateExpire;
                            person.CCIssueDate = cp.DateIssue;
                        }
                        break;
                    //LOAD SHEET
                    case "LOAD SHEET":
                        if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                        {
                            person.MBExpireDate = cp.DateExpire;
                            person.MBIssueDate = cp.DateIssue;
                        }
                        break;
                    //PASSENGER SERVICE
                    case "PASSENGER SERVICE":
                        if ((DateTime)cp.DateExpire > person.PSExpireDate || person.PSExpireDate == null)
                        {
                            person.PSExpireDate = cp.DateExpire;
                            person.PSIssueDate = cp.DateIssue;
                        }
                        break;

                    //DRM
                    case "DRM":
                        if ((DateTime)cp.DateExpire > person.DRMExpireDate || person.DRMExpireDate == null)
                        {
                            person.DRMExpireDate = cp.DateExpire;
                            person.DRMIssueDate = cp.DateIssue;
                        }
                        break;
                    //ANNEX
                    case "ANNEX":
                        if ((DateTime)cp.DateExpire > person.ANNEXExpireDate || person.ANNEXExpireDate == null)
                        {
                            person.ANNEXExpireDate = cp.DateExpire;
                            person.ANNEXIssueDate = cp.DateIssue;
                        }
                        break;
                    //FRMS
                    case "FRMS":
                        if ((DateTime)cp.DateExpire > person.TypeAirbusExpireDate || person.TypeAirbusExpireDate == null)
                        {
                            person.TypeAirbusExpireDate = cp.DateExpire;
                            person.TypeAirbusIssueDate = cp.DateIssue;
                        }
                        break;
                    //DANGEROUS GOODS
                    case "DANGEROUS GOODS":
                        if ((DateTime)cp.DateExpire > person.DangerousGoodsExpireDate || person.DangerousGoodsExpireDate == null)
                        {
                            person.DangerousGoodsExpireDate = cp.DateExpire;
                            person.DangerousGoodsIssueDate = cp.DateIssue;
                        }
                        break;
                    //1	SEPT-P
                    case "SEPT":
                        if ((DateTime)cp.DateExpire > person.SEPTPExpireDate || person.SEPTPExpireDate == null)
                        {
                            person.SEPTPExpireDate = cp.DateExpire;
                            person.SEPTPIssueDate = cp.DateIssue;
                        }
                        break;
                    //2   SEPT - T
                    case "ANNUAL SEPT":
                        if ((DateTime)cp.DateExpire > person.SEPTExpireDate || person.SEPTExpireDate == null)
                        {
                            person.SEPTExpireDate = cp.DateExpire;
                            person.SEPTIssueDate = cp.DateIssue;
                        }
                        break;
                    //4	CRM
                    case "CRM":
                        if ((DateTime)cp.DateExpire > person.UpsetRecoveryTrainingExpireDate || person.UpsetRecoveryTrainingExpireDate == null)
                        {
                            person.UpsetRecoveryTrainingExpireDate = cp.DateExpire;
                            person.UpsetRecoveryTrainingIssueDate = cp.DateIssue;
                        }
                        break;
                    //5	CCRM
                    case "CCRM":
                        if ((DateTime)cp.DateExpire > person.CCRMExpireDate || person.CCRMExpireDate == null)
                        {
                            person.CCRMExpireDate = cp.DateExpire;
                            person.CCRMIssueDate = cp.DateIssue;
                        }
                        break;
                    //6	SMS
                    case "SMS":
                        if ((DateTime)cp.DateExpire > person.SMSExpireDate || person.SMSExpireDate == null)
                        {
                            person.SMSExpireDate = cp.DateExpire;
                            person.SMSIssueDate = cp.DateIssue;
                        }
                        break;
                    //7	AV-SEC
                    case "AVIATION SECURITY":
                        if ((DateTime)cp.DateExpire > person.AviationSecurityExpireDate || person.AviationSecurityExpireDate == null)
                        {
                            person.AviationSecurityExpireDate = cp.DateExpire;
                            person.AviationSecurityIssueDate = cp.DateIssue;
                        }
                        break;
                    //8	COLD-WX
                    case "COLD WEATHER OPERATION":
                        if ((DateTime)cp.DateExpire > person.ColdWeatherOperationExpireDate || person.ColdWeatherOperationExpireDate == null)
                        {
                            person.ColdWeatherOperationExpireDate = cp.DateExpire;
                            person.ColdWeatherOperationIssueDate = cp.DateIssue;
                        }
                        break;
                    //9	HOT-WX
                    case "HOT WEATHER OPERATION":
                        if ((DateTime)cp.DateExpire > person.HotWeatherOperationExpireDate || person.HotWeatherOperationExpireDate == null)
                        {
                            person.HotWeatherOperationExpireDate = cp.DateExpire;
                            person.HotWeatherOperationIssueDate = cp.DateIssue;
                        }
                        break;
                    //10	FIRSTAID
                    case "FIRST AID":
                        if ((DateTime)cp.DateExpire > person.FirstAidExpireDate || person.FirstAidExpireDate == null)
                        {
                            person.FirstAidExpireDate = cp.DateExpire;
                            person.FirstAidIssueDate = cp.DateIssue;
                        }
                        break;
                    ////lpc
                    //case 100:
                    //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntil || person.ProficiencyValidUntil == null)
                    //    {
                    //        person.ProficiencyValidUntil = cp.DateExpire;
                    //        person.ProficiencyCheckDate = cp.DateIssue;
                    //    }
                    //    break;
                    ////opc
                    //case 101:
                    //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntilOPC || person.ProficiencyValidUntilOPC == null)
                    //    {
                    //        person.ProficiencyValidUntilOPC = cp.DateExpire;
                    //        person.ProficiencyCheckDateOPC = cp.DateIssue;
                    //    }
                    //    break;
                    ////lpr
                    //case 102:
                    //    if ((DateTime)cp.DateExpire > person.ICAOLPRValidUntil || person.ICAOLPRValidUntil == null)
                    //    {
                    //        person.ICAOLPRValidUntil = cp.DateExpire;
                    //        // person.ProficiencyCheckDateOPC = cp.DateIssue;
                    //    }
                    //    break;

                    //grt
                    case "GRT":
                        if ((DateTime)cp.DateExpire > person.DateCaoCardExpire || person.DateCaoCardExpire == null)
                        {
                            person.DateCaoCardExpire = cp.DateExpire;
                            person.DateCaoCardIssue = cp.DateIssue;
                        }
                        break;
                    //recurrent
                    case "RECURRENT 737":
                        if ((DateTime)cp.DateExpire > person.Type737ExpireDate || person.Type737ExpireDate == null)
                        {
                            person.Type737ExpireDate = cp.DateExpire;
                            person.Type737IssueDate = cp.DateIssue;
                        }
                        break;
                    //fmt
                    case "FMT":
                        if ((DateTime)cp.DateExpire > person.EGPWSExpireDate || person.EGPWSExpireDate == null)
                        {
                            person.EGPWSExpireDate = cp.DateExpire;
                            person.EGPWSIssueDate = cp.DateIssue;
                        }
                        break;
                    case "FMTD":
                        if ((DateTime)cp.DateExpire > person.FMTDExpireDate || person.FMTDExpireDate == null)
                        {
                            person.FMTDExpireDate = cp.DateExpire;
                            person.FMTDIssueDate = cp.DateIssue;
                        }
                        break;
                    case "LINE":
                        if ((DateTime)cp.DateExpire > person.LineExpireDate || person.LineExpireDate == null)
                        {
                            person.LineExpireDate = cp.DateExpire;
                            person.LineIssueDate = cp.DateIssue;
                        }
                        break;
                    default:
                        break;
                }
            }





            await context.SaveChangesAsync();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = dto,
            };
        }



        public async Task<DataResponse> SaveCertificateAtlas(ViewModels.CertificateViewModel dto)
        {
            var _dateStart = (DateTime)DateObject.ConvertToDate(dto.DateStart).Date;
            var _dateEnd = (DateTime)DateObject.ConvertToDate(dto.DateEnd).Date;

            Course entity = await context.Courses.Where(q => q.DateStart == _dateStart && q.DateEnd == _dateEnd && q.CourseTypeId == dto.CourseTypeId
            && q.OrganizationId == dto.OrganizationId).FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new Course();
                context.Courses.Add(entity);
                entity.CourseTypeId = dto.CourseTypeId;
                entity.DateStart = (DateTime)DateObject.ConvertToDate(dto.DateStart).Date;
                entity.DateEnd = (DateTime)DateObject.ConvertToDate(dto.DateEnd).Date;
                entity.Instructor = dto.Instructor;
                entity.Location = dto.Location;
                entity.OrganizationId = dto.OrganizationId;
                entity.Duration = dto.Duration;
                entity.DurationUnitId = dto.DurationUnitId;
                entity.Remark = dto.Remark;
                entity.TrainingDirector = dto.TrainingDirector;
                entity.Title = dto.Title;
                entity.Recurrent = dto.Recurrent;
                entity.Interval = dto.Interval;
                entity.CalanderTypeId = dto.CalanderTypeId;
                entity.IsGeneral = dto.IsGeneral;
                entity.CustomerId = dto.CustomerId;
                entity.No = dto.No;
                entity.IsNotificationEnabled = dto.IsNotificationEnabled;
                entity.StatusId = 3;
            }
            if (dto.PersonId != null)
            {
                var cp = await context.CoursePeoples.Where(q => q.PersonId == dto.PersonId && q.CourseId == entity.Id).FirstOrDefaultAsync();
                if (cp == null)
                {
                    cp = new CoursePeople()
                    {
                        PersonId = dto.PersonId,
                        StatusId = 1,
                        DateStatus = DateTime.Now,
                        DateExpire = (DateTime)DateObject.ConvertToDate(dto.DateExpire).Date,
                        DateIssue = (DateTime)DateObject.ConvertToDate(dto.DateIssue).Date,
                        CertificateNo = dto.CertificateNo,
                    };
                    entity.CoursePeoples.Add(cp);
                }
                else
                {
                    cp.DateExpire = (DateTime)DateObject.ConvertToDate(dto.DateExpire).Date;
                    cp.DateIssue = (DateTime)DateObject.ConvertToDate(dto.DateIssue).Date;
                    cp.CertificateNo = dto.CertificateNo;
                    cp.StatusId = 1;
                }
                //////////////////////////
                var person = await context.People.Where(q => q.Id == cp.PersonId).FirstOrDefaultAsync();
                var ct = await context.ViewCourseTypes.Where(q => q.Id == dto.CourseTypeId).FirstOrDefaultAsync();
                //12-03
                if (ct != null)
                {
                    switch (ct.CertificateType)
                    {


                        //CYBER SECURITY
                        case "RECURRENT":
                            if ((DateTime)cp.DateExpire > person.ExpireDate2 || person.ExpireDate2 == null)
                            {
                                person.ExpireDate2 = cp.DateExpire;
                                person.IssueDate2 = cp.DateIssue;
                            }
                            break;

                        case "OM-A":
                            if ((DateTime)cp.DateExpire > person.OMA1ExpireDate || person.OMA1ExpireDate == null)
                            {
                                person.OMA1ExpireDate = cp.DateExpire;
                                person.OMA1IssueDate = cp.DateIssue;
                            }
                            break;
                        //OM-B AN-26
                        case "OM-B AN-26":
                            if ((DateTime)cp.DateExpire > person.OMB1ExpireDate || person.OMB1ExpireDate == null)
                            {
                                person.OMB1ExpireDate = cp.DateExpire;
                                person.OMB1IssueDate = cp.DateIssue;
                            }
                            break;
                        //OM-C AN-26
                        case "OM-C AN-26":
                            if ((DateTime)cp.DateExpire > person.OMC1ExpireDate || person.OMC1ExpireDate == null)
                            {
                                person.OMC1ExpireDate = cp.DateExpire;
                                person.OMC1IssueDate = cp.DateIssue;
                            }
                            break;
                        case "OM-C MEP":
                            if ((DateTime)cp.DateExpire > person.OMC2ExpireDate || person.OMC2ExpireDate == null)
                            {
                                person.OMC2ExpireDate = cp.DateExpire;
                                person.OMC2IssueDate = cp.DateIssue;
                            }
                            break;
                        case "OM-C":
                            if ((DateTime)cp.DateExpire > person.OMC1ExpireDate || person.OMC1ExpireDate == null)
                            {
                                person.OMC1ExpireDate = cp.DateExpire;
                                person.OMC1IssueDate = cp.DateIssue;
                            }
                            break;
                        case "UPRT":
                            if ((DateTime)cp.DateExpire > person.UPRTExpireDate || person.UPRTExpireDate == null)
                            {
                                person.UPRTExpireDate = cp.DateExpire;
                                person.UPRTIssueDate = cp.DateIssue;
                            }
                            break;
                        case "RAMP INSPECTION":
                            if ((DateTime)cp.DateExpire > person.RampExpireDate || person.RampExpireDate == null)
                            {
                                person.RampExpireDate = cp.DateExpire;
                                person.RampIssueDate = cp.DateIssue;
                            }
                            break;

                        case "A/C SYSTEM REVIEW MEP":
                            if ((DateTime)cp.DateExpire > person.ACExpireDate || person.ACExpireDate == null)
                            {
                                person.ACExpireDate = cp.DateExpire;
                                person.ACIssueDate = cp.DateIssue;
                            }
                            break;

                        case "AIR CREW REGULATION":
                            if ((DateTime)cp.DateExpire > person.AirCrewExpireDate || person.AirCrewExpireDate == null)
                            {
                                person.AirCrewExpireDate = cp.DateExpire;
                                person.AirCrewIssueDate = cp.DateIssue;
                            }
                            break;
                        case "AIROPS REGULATION":
                            if ((DateTime)cp.DateExpire > person.AirOpsExpireDate || person.AirOpsExpireDate == null)
                            {
                                person.AirOpsExpireDate = cp.DateExpire;
                                person.AirOpsIssueDate = cp.DateIssue;
                            }
                            break;
                        case "SOP MEP":
                            if ((DateTime)cp.DateExpire > person.SOPExpireDate || person.SOPExpireDate == null)
                            {
                                person.SOPExpireDate = cp.DateExpire;
                                person.SOPIssueDate = cp.DateIssue;
                            }
                            break;
                        case "DIFF TRAINING PA31":
                            if ((DateTime)cp.DateExpire > person.Diff31ExpireDate || person.Diff31ExpireDate == null)
                            {
                                person.Diff31ExpireDate = cp.DateExpire;
                                person.Diff31IssueDate = cp.DateIssue;
                            }
                            break;
                        case "DIFF TRAINING PA34":
                            if ((DateTime)cp.DateExpire > person.Diff34ExpireDate || person.Diff34ExpireDate == null)
                            {
                                person.Diff34ExpireDate = cp.DateExpire;
                                person.Diff34IssueDate = cp.DateIssue;
                            }
                            break;

                        case "AERIAL MAPPING":
                            if ((DateTime)cp.DateExpire > person.MapExpireDate || person.MapExpireDate == null)
                            {
                                person.MapExpireDate = cp.DateExpire;
                                person.MapIssueDate = cp.DateIssue;
                            }
                            break;
                        case "COM RES":
                            if ((DateTime)cp.DateExpire > person.ComResExpireDate || person.ComResExpireDate == null)
                            {
                                person.ComResExpireDate = cp.DateExpire;
                                person.ComResIssueDate = cp.DateIssue;
                            }
                            break;

                        case "MEL":
                            if ((DateTime)cp.DateExpire > person.MELExpireDate || person.MELExpireDate == null)
                            {
                                person.MELExpireDate = cp.DateExpire;
                                person.MELIssueDate = cp.DateIssue;
                            }
                            break;

                        //ERP
                        case "ERP":
                            if ((DateTime)cp.DateExpire > person.ERPExpireDate || person.ERPExpireDate == null)
                            {
                                person.ERPExpireDate = cp.DateExpire;
                                person.ERPIssueDate = cp.DateIssue;
                            }
                            break;
                        //HF
                        case "HF":
                            if ((DateTime)cp.DateExpire > person.HFExpireDate || person.HFExpireDate == null)
                            {
                                person.HFExpireDate = cp.DateExpire;
                                person.HFIssueDate = cp.DateIssue;
                            }
                            break;
                        //MEL/CDL
                        case "MEL/CDL":
                            if ((DateTime)cp.DateExpire > person.MELExpireDate || person.MELExpireDate == null)
                            {
                                person.MELExpireDate = cp.DateExpire;
                                person.MELIssueDate = cp.DateIssue;
                            }
                            break;
                        //METEOROLOGY
                        case "METEOROLOGY":
                            if ((DateTime)cp.DateExpire > person.METExpireDate || person.METExpireDate == null)
                            {
                                person.METExpireDate = cp.DateExpire;
                                person.METIssueDate = cp.DateIssue;
                            }
                            break;
                        //PERFORMANCE
                        case "PERFORMANCE":
                            if ((DateTime)cp.DateExpire > person.PERExpireDate || person.PERExpireDate == null)
                            {
                                person.PERExpireDate = cp.DateExpire;
                                person.PERIssueDate = cp.DateIssue;
                            }
                            break;
                        //RADIO COMMUNICATION
                        case "RADIO COMMUNICATION":
                            if ((DateTime)cp.DateExpire > person.LRCExpireDate || person.LRCExpireDate == null)
                            {
                                person.LRCExpireDate = cp.DateExpire;
                                person.LRCIssueDate = cp.DateIssue;
                            }
                            break;
                        //SITA
                        case "SITA":
                            if ((DateTime)cp.DateExpire > person.RSPExpireDate || person.RSPExpireDate == null)
                            {
                                person.RSPExpireDate = cp.DateExpire;
                                person.RSPIssueDate = cp.DateIssue;
                            }
                            break;
                        //WEIGHT AND BALANCE
                        case "WEIGHT AND BALANCE":
                            if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                            {
                                person.MBExpireDate = cp.DateExpire;
                                person.MBIssueDate = cp.DateIssue;
                            }
                            break;
                        //AIRSIDE SAFETY AND DRIVING
                        case "AIRSIDE SAFETY AND DRIVING":
                            if ((DateTime)cp.DateExpire > person.ASDExpireDate || person.ASDExpireDate == null)
                            {
                                person.ASDExpireDate = cp.DateExpire;
                                person.ASDIssueDate = cp.DateIssue;
                            }
                            break;
                        //EFB
                        case "EFB":
                            if ((DateTime)cp.DateExpire > person.ExpireDate1 || person.ExpireDate1 == null)
                            {
                                person.ExpireDate1 = cp.DateExpire;
                                person.IssueDate1 = cp.DateIssue;
                            }
                            break;
                        //GOM
                        case "GOM":
                            if ((DateTime)cp.DateExpire > person.GOMExpireDate || person.GOMExpireDate == null)
                            {
                                person.GOMExpireDate = cp.DateExpire;
                                person.GOMIssueDate = cp.DateIssue;
                            }
                            break;
                        //AIRPORT SERVICE FAMILIARIZATION
                        case "AIRPORT SERVICE FAMILIARIZATION":
                            if ((DateTime)cp.DateExpire > person.ASFExpireDate || person.ASFExpireDate == null)
                            {
                                person.ASFExpireDate = cp.DateExpire;
                                person.ASFIssueDate = cp.DateIssue;
                            }
                            break;
                        //CUSTOMER CARE
                        case "CUSTOMER CARE":
                            if ((DateTime)cp.DateExpire > person.CCExpireDate || person.CCExpireDate == null)
                            {
                                person.CCExpireDate = cp.DateExpire;
                                person.CCIssueDate = cp.DateIssue;
                            }
                            break;
                        //LOAD SHEET
                        case "LOAD SHEET":
                            if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                            {
                                person.MBExpireDate = cp.DateExpire;
                                person.MBIssueDate = cp.DateIssue;
                            }
                            break;
                        //PASSENGER SERVICE
                        case "PASSENGER SERVICE":
                            if ((DateTime)cp.DateExpire > person.PSExpireDate || person.PSExpireDate == null)
                            {
                                person.PSExpireDate = cp.DateExpire;
                                person.PSIssueDate = cp.DateIssue;
                            }
                            break;

                        //DRM
                        case "DRM":
                            if ((DateTime)cp.DateExpire > person.DRMExpireDate || person.DRMExpireDate == null)
                            {
                                person.DRMExpireDate = cp.DateExpire;
                                person.DRMIssueDate = cp.DateIssue;
                            }
                            break;
                        //ANNEX
                        case "ANNEX":
                            if ((DateTime)cp.DateExpire > person.ANNEXExpireDate || person.ANNEXExpireDate == null)
                            {
                                person.ANNEXExpireDate = cp.DateExpire;
                                person.ANNEXIssueDate = cp.DateIssue;
                            }
                            break;
                        //FRMS
                        case "FRMS":
                            if ((DateTime)cp.DateExpire > person.TypeAirbusExpireDate || person.TypeAirbusExpireDate == null)
                            {
                                person.TypeAirbusExpireDate = cp.DateExpire;
                                person.TypeAirbusIssueDate = cp.DateIssue;
                            }
                            break;
                        //DANGEROUS GOODS
                        case "DANGEROUS GOODS":
                            if ((DateTime)cp.DateExpire > person.DangerousGoodsExpireDate || person.DangerousGoodsExpireDate == null)
                            {
                                person.DangerousGoodsExpireDate = cp.DateExpire;
                                person.DangerousGoodsIssueDate = cp.DateIssue;
                            }
                            break;
                        //1	SEPT-P
                        case "SEPT":
                            if ((DateTime)cp.DateExpire > person.SEPTPExpireDate || person.SEPTPExpireDate == null)
                            {
                                person.SEPTPExpireDate = cp.DateExpire;
                                person.SEPTPIssueDate = cp.DateIssue;
                            }
                            break;
                        //2   SEPT - T
                        case "ANNUAL SEPT":
                            if ((DateTime)cp.DateExpire > person.SEPTExpireDate || person.SEPTExpireDate == null)
                            {
                                person.SEPTExpireDate = cp.DateExpire;
                                person.SEPTIssueDate = cp.DateIssue;
                            }
                            break;
                        //4	CRM
                        case "CRM":
                            if ((DateTime)cp.DateExpire > person.UpsetRecoveryTrainingExpireDate || person.UpsetRecoveryTrainingExpireDate == null)
                            {
                                person.UpsetRecoveryTrainingExpireDate = cp.DateExpire;
                                person.UpsetRecoveryTrainingIssueDate = cp.DateIssue;
                            }
                            break;
                        //5	CCRM
                        case "CCRM":
                            if ((DateTime)cp.DateExpire > person.CCRMExpireDate || person.CCRMExpireDate == null)
                            {
                                person.CCRMExpireDate = cp.DateExpire;
                                person.CCRMIssueDate = cp.DateIssue;
                            }
                            break;
                        //6	SMS
                        case "SMS":
                            if ((DateTime)cp.DateExpire > person.SMSExpireDate || person.SMSExpireDate == null)
                            {
                                person.SMSExpireDate = cp.DateExpire;
                                person.SMSIssueDate = cp.DateIssue;
                            }
                            break;
                        //7	AV-SEC
                        case "AVIATION SECURITY":
                            if ((DateTime)cp.DateExpire > person.AviationSecurityExpireDate || person.AviationSecurityExpireDate == null)
                            {
                                person.AviationSecurityExpireDate = cp.DateExpire;
                                person.AviationSecurityIssueDate = cp.DateIssue;
                            }
                            break;
                        //8	COLD-WX
                        case "COLD WEATHER OPERATION":
                            if ((DateTime)cp.DateExpire > person.ColdWeatherOperationExpireDate || person.ColdWeatherOperationExpireDate == null)
                            {
                                person.ColdWeatherOperationExpireDate = cp.DateExpire;
                                person.ColdWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //9	HOT-WX
                        case "HOT WEATHER OPERATION":
                            if ((DateTime)cp.DateExpire > person.HotWeatherOperationExpireDate || person.HotWeatherOperationExpireDate == null)
                            {
                                person.HotWeatherOperationExpireDate = cp.DateExpire;
                                person.HotWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //10	FIRSTAID
                        case "FIRST AID":
                            if ((DateTime)cp.DateExpire > person.FirstAidExpireDate || person.FirstAidExpireDate == null)
                            {
                                person.FirstAidExpireDate = cp.DateExpire;
                                person.FirstAidIssueDate = cp.DateIssue;
                            }
                            break;
                        ////lpc
                        //case 100:
                        //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntil || person.ProficiencyValidUntil == null)
                        //    {
                        //        person.ProficiencyValidUntil = cp.DateExpire;
                        //        person.ProficiencyCheckDate = cp.DateIssue;
                        //    }
                        //    break;
                        ////opc
                        //case 101:
                        //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntilOPC || person.ProficiencyValidUntilOPC == null)
                        //    {
                        //        person.ProficiencyValidUntilOPC = cp.DateExpire;
                        //        person.ProficiencyCheckDateOPC = cp.DateIssue;
                        //    }
                        //    break;
                        ////lpr
                        //case 102:
                        //    if ((DateTime)cp.DateExpire > person.ICAOLPRValidUntil || person.ICAOLPRValidUntil == null)
                        //    {
                        //        person.ICAOLPRValidUntil = cp.DateExpire;
                        //        // person.ProficiencyCheckDateOPC = cp.DateIssue;
                        //    }
                        //    break;

                        //grt
                        case "GRT":
                            if ((DateTime)cp.DateExpire > person.DateCaoCardExpire || person.DateCaoCardExpire == null)
                            {
                                person.DateCaoCardExpire = cp.DateExpire;
                                person.DateCaoCardIssue = cp.DateIssue;
                            }
                            break;
                        //recurrent
                        case "RECURRENT 737":
                            if ((DateTime)cp.DateExpire > person.Type737ExpireDate || person.Type737ExpireDate == null)
                            {
                                person.Type737ExpireDate = cp.DateExpire;
                                person.Type737IssueDate = cp.DateIssue;
                            }
                            break;
                        //fmt
                        case "FMT":
                            if ((DateTime)cp.DateExpire > person.EGPWSExpireDate || person.EGPWSExpireDate == null)
                            {
                                person.EGPWSExpireDate = cp.DateExpire;
                                person.EGPWSIssueDate = cp.DateIssue;
                            }
                            break;
                        case "FMTD":
                            if ((DateTime)cp.DateExpire > person.FMTDExpireDate || person.FMTDExpireDate == null)
                            {
                                person.FMTDExpireDate = cp.DateExpire;
                                person.FMTDIssueDate = cp.DateIssue;
                            }
                            break;
                        case "LINE":
                            if ((DateTime)cp.DateExpire > person.LineExpireDate || person.LineExpireDate == null)
                            {
                                person.LineExpireDate = cp.DateExpire;
                                person.LineIssueDate = cp.DateIssue;
                            }
                            break;
                        default:
                            break;
                    }
                    /*switch (ct.CertificateTypeId)
                    {
                        //dg
                        case 3:
                            if ((DateTime)cp.DateExpire > person.DangerousGoodsExpireDate)
                            {
                                person.DangerousGoodsExpireDate = cp.DateExpire;
                                person.DangerousGoodsIssueDate = cp.DateIssue;
                            }
                            break;
                        //1	SEPT-P
                        case 1:
                            if ((DateTime)cp.DateExpire > person.SEPTPExpireDate)
                            {
                                person.SEPTPExpireDate = cp.DateExpire;
                                person.SEPTPIssueDate = cp.DateIssue;
                            }
                            break;
                        //2   SEPT - T
                        case 2:
                            if ((DateTime)cp.DateExpire > person.SEPTExpireDate)
                            {
                                person.SEPTExpireDate = cp.DateExpire;
                                person.SEPTIssueDate = cp.DateIssue;
                            }
                            break;
                        //4	CRM
                        case 4:
                            if ((DateTime)cp.DateExpire > person.UpsetRecoveryTrainingExpireDate)
                            {
                                person.UpsetRecoveryTrainingExpireDate = cp.DateExpire;
                                person.UpsetRecoveryTrainingIssueDate = cp.DateIssue;
                            }
                            break;
                        //5	CCRM
                        case 5:
                            if ((DateTime)cp.DateExpire > person.CCRMExpireDate)
                            {
                                person.CCRMExpireDate = cp.DateExpire;
                                person.CCRMIssueDate = cp.DateIssue;
                            }
                            break;
                        //6	SMS
                        case 6:
                            if ((DateTime)cp.DateExpire > person.SMSExpireDate)
                            {
                                person.SMSExpireDate = cp.DateExpire;
                                person.SMSIssueDate = cp.DateIssue;
                            }
                            break;
                        //7	AV-SEC
                        case 7:
                            if ((DateTime)cp.DateExpire > person.AviationSecurityExpireDate)
                            {
                                person.AviationSecurityExpireDate = cp.DateExpire;
                                person.AviationSecurityIssueDate = cp.DateIssue;
                            }
                            break;
                        //8	COLD-WX
                        case 8:
                            if ((DateTime)cp.DateExpire > person.ColdWeatherOperationExpireDate)
                            {
                                person.ColdWeatherOperationExpireDate = cp.DateExpire;
                                person.ColdWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //9	HOT-WX
                        case 9:
                            if ((DateTime)cp.DateExpire > person.HotWeatherOperationExpireDate)
                            {
                                person.HotWeatherOperationExpireDate = cp.DateExpire;
                                person.HotWeatherOperationIssueDate = cp.DateIssue;
                            }
                            break;
                        //10	FIRSTAID
                        case 10:
                            if ((DateTime)cp.DateExpire > person.FirstAidExpireDate)
                            {
                                person.FirstAidExpireDate = cp.DateExpire;
                                person.FirstAidIssueDate = cp.DateIssue;
                            }
                            break;
                        case 105:
                            if ((DateTime)cp.DateExpire > person.LineExpireDate)
                            {
                                person.LineExpireDate = cp.DateExpire;
                                person.LineIssueDate = cp.DateIssue;
                            }
                            break;
                        default:
                            break;
                    }*/
                }


                /////////////////////////


            }




            await context.SaveChangesAsync();
            dto.Id = entity.Id;
            var result = await context.ViewCoursePeopleRankeds.Where(q => q.PersonId == dto.PersonId && q.CourseId == entity.Id).FirstOrDefaultAsync();
            return new DataResponse()
            {
                IsSuccess = true,
                Data = result,
            };
        }
        public async Task<DataResponse> UpdateCoursePeopleStatusAtlas(CoursePeopleStatusViewModel dto)
        {
            CoursePeople cp = null;

            if (dto.Id != -1)
                cp = await context.CoursePeoples.Where(q => q.Id == dto.Id).FirstOrDefaultAsync();
            else
                cp = await context.CoursePeoples.Where(q => q.PersonId == dto.PersonId && q.CourseId == dto.CourseId).FirstOrDefaultAsync();
            if (dto.StatusId != cp.StatusId)
                cp.DateStatus = DateTime.Now;

            //-1: UnKnown 0:Failed 1:Passed
            if (dto.StatusId != 1)
            {
                cp.DateIssue = null;
                cp.DateExpire = null;
                cp.CertificateNo = null;


            }
            else
            {
                cp.DateExpire = string.IsNullOrEmpty(dto.Expire) ? null : DateObject.ConvertToDate(dto.Expire).Date;
                cp.DateIssue = string.IsNullOrEmpty(dto.Issue) ? null : DateObject.ConvertToDate(dto.Issue).Date;
                cp.CertificateNo = dto.No;
                if (string.IsNullOrEmpty(cp.CertificateNo))
                    cp.CertificateNo = "FPC-" + cp.Id;

            }

            cp.StatusId = dto.StatusId;
            cp.StatusRemark = dto.Remark;

            if (dto.StatusId == 1 && cp.DateIssue != null && cp.DateExpire != null && !string.IsNullOrEmpty(cp.CertificateNo))
            {

                var person = await context.People.Where(q => q.Id == cp.PersonId).FirstOrDefaultAsync();
                var course = await context.ViewCourseNews.Where(q => q.Id == cp.CourseId).FirstOrDefaultAsync();
                switch (course.CertificateType)
                {


                    //CYBER SECURITY
                    case "RECURRENT":
                        if ((DateTime)cp.DateExpire > person.ExpireDate2 || person.ExpireDate2 == null)
                        {
                            person.ExpireDate2 = cp.DateExpire;
                            person.IssueDate2 = cp.DateIssue;
                        }
                        break;

                    case "OM-A":
                        if ((DateTime)cp.DateExpire > person.OMA1ExpireDate || person.OMA1ExpireDate == null)
                        {
                            person.OMA1ExpireDate = cp.DateExpire;
                            person.OMA1IssueDate = cp.DateIssue;
                        }
                        break;
                    //OM-B AN-26
                    case "OM-B AN-26":
                        if ((DateTime)cp.DateExpire > person.OMB1ExpireDate || person.OMB1ExpireDate == null)
                        {
                            person.OMB1ExpireDate = cp.DateExpire;
                            person.OMB1IssueDate = cp.DateIssue;
                        }
                        break;
                    //OM-C AN-26
                    case "OM-C AN-26":
                        if ((DateTime)cp.DateExpire > person.OMC1ExpireDate || person.OMC1ExpireDate == null)
                        {
                            person.OMC1ExpireDate = cp.DateExpire;
                            person.OMC1IssueDate = cp.DateIssue;
                        }
                        break;
                    case "OM-C MEP":
                        if ((DateTime)cp.DateExpire > person.OMC2ExpireDate || person.OMC2ExpireDate == null)
                        {
                            person.OMC2ExpireDate = cp.DateExpire;
                            person.OMC2IssueDate = cp.DateIssue;
                        }
                        break;
                    case "OM-C":
                        if ((DateTime)cp.DateExpire > person.OMC1ExpireDate || person.OMC1ExpireDate == null)
                        {
                            person.OMC1ExpireDate = cp.DateExpire;
                            person.OMC1IssueDate = cp.DateIssue;
                        }
                        break;
                    case "UPRT":
                        if ((DateTime)cp.DateExpire > person.UPRTExpireDate || person.UPRTExpireDate == null)
                        {
                            person.UPRTExpireDate = cp.DateExpire;
                            person.UPRTIssueDate = cp.DateIssue;
                        }
                        break;
                    case "RAMP INSPECTION":
                        if ((DateTime)cp.DateExpire > person.RampExpireDate || person.RampExpireDate == null)
                        {
                            person.RampExpireDate = cp.DateExpire;
                            person.RampIssueDate = cp.DateIssue;
                        }
                        break;

                    case "A/C SYSTEM REVIEW MEP":
                        if ((DateTime)cp.DateExpire > person.ACExpireDate || person.ACExpireDate == null)
                        {
                            person.ACExpireDate = cp.DateExpire;
                            person.ACIssueDate = cp.DateIssue;
                        }
                        break;

                    case "AIR CREW REGULATION":
                        if ((DateTime)cp.DateExpire > person.AirCrewExpireDate || person.AirCrewExpireDate == null)
                        {
                            person.AirCrewExpireDate = cp.DateExpire;
                            person.AirCrewIssueDate = cp.DateIssue;
                        }
                        break;
                    case "AIROPS REGULATION":
                        if ((DateTime)cp.DateExpire > person.AirOpsExpireDate || person.AirOpsExpireDate == null)
                        {
                            person.AirOpsExpireDate = cp.DateExpire;
                            person.AirOpsIssueDate = cp.DateIssue;
                        }
                        break;
                    case "SOP MEP":
                        if ((DateTime)cp.DateExpire > person.SOPExpireDate || person.SOPExpireDate == null)
                        {
                            person.SOPExpireDate = cp.DateExpire;
                            person.SOPIssueDate = cp.DateIssue;
                        }
                        break;
                    case "DIFF TRAINING PA31":
                        if ((DateTime)cp.DateExpire > person.Diff31ExpireDate || person.Diff31ExpireDate == null)
                        {
                            person.Diff31ExpireDate = cp.DateExpire;
                            person.Diff31IssueDate = cp.DateIssue;
                        }
                        break;
                    case "DIFF TRAINING PA34":
                        if ((DateTime)cp.DateExpire > person.Diff34ExpireDate || person.Diff34ExpireDate == null)
                        {
                            person.Diff34ExpireDate = cp.DateExpire;
                            person.Diff34IssueDate = cp.DateIssue;
                        }
                        break;

                    case "AERIAL MAPPING":
                        if ((DateTime)cp.DateExpire > person.MapExpireDate || person.MapExpireDate == null)
                        {
                            person.MapExpireDate = cp.DateExpire;
                            person.MapIssueDate = cp.DateIssue;
                        }
                        break;
                    case "COM RES":
                        if ((DateTime)cp.DateExpire > person.ComResExpireDate || person.ComResExpireDate == null)
                        {
                            person.ComResExpireDate = cp.DateExpire;
                            person.ComResIssueDate = cp.DateIssue;
                        }
                        break;

                    case "MEL":
                        if ((DateTime)cp.DateExpire > person.MELExpireDate || person.MELExpireDate == null)
                        {
                            person.MELExpireDate = cp.DateExpire;
                            person.MELIssueDate = cp.DateIssue;
                        }
                        break;

                    //ERP
                    case "ERP":
                        if ((DateTime)cp.DateExpire > person.ERPExpireDate || person.ERPExpireDate == null)
                        {
                            person.ERPExpireDate = cp.DateExpire;
                            person.ERPIssueDate = cp.DateIssue;
                        }
                        break;
                    //HF
                    case "HF":
                        if ((DateTime)cp.DateExpire > person.HFExpireDate || person.HFExpireDate == null)
                        {
                            person.HFExpireDate = cp.DateExpire;
                            person.HFIssueDate = cp.DateIssue;
                        }
                        break;
                    //MEL/CDL
                    case "MEL/CDL":
                        if ((DateTime)cp.DateExpire > person.MELExpireDate || person.MELExpireDate == null)
                        {
                            person.MELExpireDate = cp.DateExpire;
                            person.MELIssueDate = cp.DateIssue;
                        }
                        break;
                    //METEOROLOGY
                    case "METEOROLOGY":
                        if ((DateTime)cp.DateExpire > person.METExpireDate || person.METExpireDate == null)
                        {
                            person.METExpireDate = cp.DateExpire;
                            person.METIssueDate = cp.DateIssue;
                        }
                        break;
                    //PERFORMANCE
                    case "PERFORMANCE":
                        if ((DateTime)cp.DateExpire > person.PERExpireDate || person.PERExpireDate == null)
                        {
                            person.PERExpireDate = cp.DateExpire;
                            person.PERIssueDate = cp.DateIssue;
                        }
                        break;
                    //RADIO COMMUNICATION
                    case "RADIO COMMUNICATION":
                        if ((DateTime)cp.DateExpire > person.LRCExpireDate || person.LRCExpireDate == null)
                        {
                            person.LRCExpireDate = cp.DateExpire;
                            person.LRCIssueDate = cp.DateIssue;
                        }
                        break;
                    //SITA
                    case "SITA":
                        if ((DateTime)cp.DateExpire > person.RSPExpireDate || person.RSPExpireDate == null)
                        {
                            person.RSPExpireDate = cp.DateExpire;
                            person.RSPIssueDate = cp.DateIssue;
                        }
                        break;
                    //WEIGHT AND BALANCE
                    case "WEIGHT AND BALANCE":
                        if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                        {
                            person.MBExpireDate = cp.DateExpire;
                            person.MBIssueDate = cp.DateIssue;
                        }
                        break;
                    //AIRSIDE SAFETY AND DRIVING
                    case "AIRSIDE SAFETY AND DRIVING":
                        if ((DateTime)cp.DateExpire > person.ASDExpireDate || person.ASDExpireDate == null)
                        {
                            person.ASDExpireDate = cp.DateExpire;
                            person.ASDIssueDate = cp.DateIssue;
                        }
                        break;
                    //EFB
                    case "EFB":
                        if ((DateTime)cp.DateExpire > person.ExpireDate1 || person.ExpireDate1 == null)
                        {
                            person.ExpireDate1 = cp.DateExpire;
                            person.IssueDate1 = cp.DateIssue;
                        }
                        break;
                    //GOM
                    case "GOM":
                        if ((DateTime)cp.DateExpire > person.GOMExpireDate || person.GOMExpireDate == null)
                        {
                            person.GOMExpireDate = cp.DateExpire;
                            person.GOMIssueDate = cp.DateIssue;
                        }
                        break;
                    //AIRPORT SERVICE FAMILIARIZATION
                    case "AIRPORT SERVICE FAMILIARIZATION":
                        if ((DateTime)cp.DateExpire > person.ASFExpireDate || person.ASFExpireDate == null)
                        {
                            person.ASFExpireDate = cp.DateExpire;
                            person.ASFIssueDate = cp.DateIssue;
                        }
                        break;
                    //CUSTOMER CARE
                    case "CUSTOMER CARE":
                        if ((DateTime)cp.DateExpire > person.CCExpireDate || person.CCExpireDate == null)
                        {
                            person.CCExpireDate = cp.DateExpire;
                            person.CCIssueDate = cp.DateIssue;
                        }
                        break;
                    //LOAD SHEET
                    case "LOAD SHEET":
                        if ((DateTime)cp.DateExpire > person.MBExpireDate || person.MBExpireDate == null)
                        {
                            person.MBExpireDate = cp.DateExpire;
                            person.MBIssueDate = cp.DateIssue;
                        }
                        break;
                    //PASSENGER SERVICE
                    case "PASSENGER SERVICE":
                        if ((DateTime)cp.DateExpire > person.PSExpireDate || person.PSExpireDate == null)
                        {
                            person.PSExpireDate = cp.DateExpire;
                            person.PSIssueDate = cp.DateIssue;
                        }
                        break;

                    //DRM
                    case "DRM":
                        if ((DateTime)cp.DateExpire > person.DRMExpireDate || person.DRMExpireDate == null)
                        {
                            person.DRMExpireDate = cp.DateExpire;
                            person.DRMIssueDate = cp.DateIssue;
                        }
                        break;
                    //ANNEX
                    case "ANNEX":
                        if ((DateTime)cp.DateExpire > person.ANNEXExpireDate || person.ANNEXExpireDate == null)
                        {
                            person.ANNEXExpireDate = cp.DateExpire;
                            person.ANNEXIssueDate = cp.DateIssue;
                        }
                        break;
                    //FRMS
                    case "FRMS":
                        if ((DateTime)cp.DateExpire > person.TypeAirbusExpireDate || person.TypeAirbusExpireDate == null)
                        {
                            person.TypeAirbusExpireDate = cp.DateExpire;
                            person.TypeAirbusIssueDate = cp.DateIssue;
                        }
                        break;
                    //DANGEROUS GOODS
                    case "DANGEROUS GOODS":
                        if ((DateTime)cp.DateExpire > person.DangerousGoodsExpireDate || person.DangerousGoodsExpireDate == null)
                        {
                            person.DangerousGoodsExpireDate = cp.DateExpire;
                            person.DangerousGoodsIssueDate = cp.DateIssue;
                        }
                        break;
                    //1	SEPT-P
                    case "SEPT":
                        if ((DateTime)cp.DateExpire > person.SEPTPExpireDate || person.SEPTPExpireDate == null)
                        {
                            person.SEPTPExpireDate = cp.DateExpire;
                            person.SEPTPIssueDate = cp.DateIssue;
                        }
                        break;
                    //2   SEPT - T
                    case "ANNUAL SEPT":
                        if ((DateTime)cp.DateExpire > person.SEPTExpireDate || person.SEPTExpireDate == null)
                        {
                            person.SEPTExpireDate = cp.DateExpire;
                            person.SEPTIssueDate = cp.DateIssue;
                        }
                        break;
                    //4	CRM
                    case "CRM":
                        if ((DateTime)cp.DateExpire > person.UpsetRecoveryTrainingExpireDate || person.UpsetRecoveryTrainingExpireDate == null)
                        {
                            person.UpsetRecoveryTrainingExpireDate = cp.DateExpire;
                            person.UpsetRecoveryTrainingIssueDate = cp.DateIssue;
                        }
                        break;
                    //5	CCRM
                    case "CCRM":
                        if ((DateTime)cp.DateExpire > person.CCRMExpireDate || person.CCRMExpireDate == null)
                        {
                            person.CCRMExpireDate = cp.DateExpire;
                            person.CCRMIssueDate = cp.DateIssue;
                        }
                        break;
                    //6	SMS
                    case "SMS":
                        if ((DateTime)cp.DateExpire > person.SMSExpireDate || person.SMSExpireDate == null)
                        {
                            person.SMSExpireDate = cp.DateExpire;
                            person.SMSIssueDate = cp.DateIssue;
                        }
                        break;
                    //7	AV-SEC
                    case "AVIATION SECURITY":
                        if ((DateTime)cp.DateExpire > person.AviationSecurityExpireDate || person.AviationSecurityExpireDate == null)
                        {
                            person.AviationSecurityExpireDate = cp.DateExpire;
                            person.AviationSecurityIssueDate = cp.DateIssue;
                        }
                        break;
                    //8	COLD-WX
                    case "COLD WEATHER OPERATION":
                        if ((DateTime)cp.DateExpire > person.ColdWeatherOperationExpireDate || person.ColdWeatherOperationExpireDate == null)
                        {
                            person.ColdWeatherOperationExpireDate = cp.DateExpire;
                            person.ColdWeatherOperationIssueDate = cp.DateIssue;
                        }
                        break;
                    //9	HOT-WX
                    case "HOT WEATHER OPERATION":
                        if ((DateTime)cp.DateExpire > person.HotWeatherOperationExpireDate || person.HotWeatherOperationExpireDate == null)
                        {
                            person.HotWeatherOperationExpireDate = cp.DateExpire;
                            person.HotWeatherOperationIssueDate = cp.DateIssue;
                        }
                        break;
                    //10	FIRSTAID
                    case "FIRST AID":
                        if ((DateTime)cp.DateExpire > person.FirstAidExpireDate || person.FirstAidExpireDate == null)
                        {
                            person.FirstAidExpireDate = cp.DateExpire;
                            person.FirstAidIssueDate = cp.DateIssue;
                        }
                        break;
                    ////lpc
                    //case 100:
                    //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntil || person.ProficiencyValidUntil == null)
                    //    {
                    //        person.ProficiencyValidUntil = cp.DateExpire;
                    //        person.ProficiencyCheckDate = cp.DateIssue;
                    //    }
                    //    break;
                    ////opc
                    //case 101:
                    //    if ((DateTime)cp.DateExpire > person.ProficiencyValidUntilOPC || person.ProficiencyValidUntilOPC == null)
                    //    {
                    //        person.ProficiencyValidUntilOPC = cp.DateExpire;
                    //        person.ProficiencyCheckDateOPC = cp.DateIssue;
                    //    }
                    //    break;
                    ////lpr
                    //case 102:
                    //    if ((DateTime)cp.DateExpire > person.ICAOLPRValidUntil || person.ICAOLPRValidUntil == null)
                    //    {
                    //        person.ICAOLPRValidUntil = cp.DateExpire;
                    //        // person.ProficiencyCheckDateOPC = cp.DateIssue;
                    //    }
                    //    break;

                    //grt
                    case "GRT":
                        if ((DateTime)cp.DateExpire > person.DateCaoCardExpire || person.DateCaoCardExpire == null)
                        {
                            person.DateCaoCardExpire = cp.DateExpire;
                            person.DateCaoCardIssue = cp.DateIssue;
                        }
                        break;
                    //recurrent
                    case "RECURRENT 737":
                        if ((DateTime)cp.DateExpire > person.Type737ExpireDate || person.Type737ExpireDate == null)
                        {
                            person.Type737ExpireDate = cp.DateExpire;
                            person.Type737IssueDate = cp.DateIssue;
                        }
                        break;
                    //fmt
                    case "FMT":
                        if ((DateTime)cp.DateExpire > person.EGPWSExpireDate || person.EGPWSExpireDate == null)
                        {
                            person.EGPWSExpireDate = cp.DateExpire;
                            person.EGPWSIssueDate = cp.DateIssue;
                        }
                        break;
                    case "FMTD":
                        if ((DateTime)cp.DateExpire > person.FMTDExpireDate || person.FMTDExpireDate == null)
                        {
                            person.FMTDExpireDate = cp.DateExpire;
                            person.FMTDIssueDate = cp.DateIssue;
                        }
                        break;
                    case "LINE":
                        if ((DateTime)cp.DateExpire > person.LineExpireDate || person.LineExpireDate == null)
                        {
                            person.LineExpireDate = cp.DateExpire;
                            person.LineIssueDate = cp.DateIssue;
                        }
                        break;
                    default:
                        break;
                }
            }





            await context.SaveChangesAsync();

            return new DataResponse()
            {
                IsSuccess = true,
                Data = dto,
            };
        }


        public IQueryable<CertificateType> GetCertificateTypesQuery()
        {
            IQueryable<CertificateType> query = context.Set<CertificateType>().AsNoTracking();
            return query;
        }

        public IQueryable<ViewCourseType> GetCourseTypesQuery()
        {
            IQueryable<ViewCourseType> query = context.Set<ViewCourseType>().AsNoTracking();
            return query;
        }

        public IQueryable<ViewCourseNew> GetCourseQuery()
        {
            IQueryable<ViewCourseNew> query = context.Set<ViewCourseNew>().AsNoTracking();
            return query;
        }

        public IQueryable<ViewJobGroup> GetViewJobGroupQuery()
        {
            IQueryable<ViewJobGroup> query = context.Set<ViewJobGroup>().AsNoTracking();
            return query;
        }

        public async Task<DataResponse> GetCourseSessions(int cid)
        {
            var result = await context.CourseSessions.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetCoursePeopleAndSessions(int cid)
        {
            var sessions = await context.CourseSessions.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();
            var people = await context.ViewCoursePeoples.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();
            //var press = await context.CourseSessionPresences.Where(q => q.CourseId == cid).ToListAsync();
            var press = await context.ViewCourseSessionPresences.Where(q => q.CourseId == cid).ToListAsync();
            var syllabi = await context.ViewSyllabus.Where(q => q.CourseId == cid).ToListAsync();
            

            return new DataResponse()
            {
                Data = new
                {
                    sessions,
                    people,
                    press,
                    syllabi
                },
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> NotifyCoursePeople(int cid, string recs)
        {
            var recIds = recs.Split('_').Select(q => (Nullable<int>)Convert.ToInt32(q)).ToList();
            var course = await context.ViewCourseNews.Where(q => q.Id == cid).FirstOrDefaultAsync();
            var people = await context.ViewCoursePeoples.Where(q => q.CourseId == cid && recIds.Contains(q.PersonId)).OrderBy(q => q.DateStart).ToListAsync();
            var sessions = await context.CourseSessions.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();
            List<string> strs = new List<string>();
            strs.Add("COURSE NOTIFICATION");
            strs.Add(course.Title.ToUpper());
            if (!string.IsNullOrEmpty(course.Organization))
                strs.Add(course.Organization);
            if (!string.IsNullOrEmpty(course.Location))
                strs.Add(course.Location);
            if (!string.IsNullOrEmpty(course.HoldingType))
                strs.Add(course.HoldingType);
            strs.Add(course.DateStart.ToString("ddd, dd MMM yyyy"));
            if (course.DateEnd != null)
                strs.Add(((DateTime)course.DateEnd).ToString("ddd, dd MMM yyyy"));

            if (sessions.Count > 0)
            {
                strs.Add("Sessions");
                foreach (var x in sessions)
                {
                    if (x.DateStart != null && x.DateEnd != null)
                    {
                        var dt = ((DateTime)x.DateStart).ToString("ddd, dd MMM yyyy");
                        strs.Add(dt + " " + ((DateTime)x.DateStart).ToString("HH:mm") + "-" + ((DateTime)x.DateEnd).ToString("HH:mm"));
                    }

                }
            }
            strs.Add("TRAINING DEPARTMENT");

            var text = String.Join("\n", strs);
            Magfa m = new Magfa();

            var res = new List<long>();
            var hists = new List<CourseSMSHistory>();
            foreach (var p in people)
            {
                var rs = m.enqueue(1, p.Mobile, text)[0];
                var hist = new CourseSMSHistory()
                {
                    CourseId = cid,
                    DateSent = DateTime.Now,
                    Mobil = p.Mobile,
                    Msg = text,
                    PersonId = p.EmployeeId,
                    PersonName = p.Name,
                    TypeId = 1,
                    RefId = rs,



                };
                hists.Add(hist);
                context.CourseSMSHistories.Add(hist);
                res.Add(rs);
            }
            await Task.Delay(10000);


            var sts = m.getStatus(res);
            int c = 0;
            foreach (var st in sts)
            {
                hists[c].DateStatus = DateTime.Now;
                hists[c].Statu = st;
                c++;
            }
            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = hists,
                IsSuccess = true,
            };

        }


        public async Task<DataResponse> NotifyCourseTeachers(int cid)
        {
            var course = await context.ViewCourseNews.Where(q => q.Id == cid).FirstOrDefaultAsync();
            var crs = await context.Courses.Where(q => q.Id == cid).FirstOrDefaultAsync();
            Person ins1 = null;
            Person ins2 = null;
            if (course.CurrencyId != null)
                ins1 = await context.People.FirstOrDefaultAsync(q => q.Id == course.CurrencyId);
            if (course.Instructor2Id != null)
                ins2 = await context.People.FirstOrDefaultAsync(q => q.Id == course.Instructor2Id);

            List<Person> people = new List<Person>();
            if (ins1 != null)
                people.Add(ins1);
            if (ins2 != null)
                people.Add(ins2);
            var sessions = await context.CourseSessions.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();
            List<string> strs = new List<string>();
            strs.Add("COURSE NOTIFICATION");
            strs.Add(course.Title.ToUpper());
            if (!string.IsNullOrEmpty(course.Organization))
                strs.Add(course.Organization);
            if (!string.IsNullOrEmpty(course.Location))
                strs.Add(course.Location);
            if (!string.IsNullOrEmpty(course.HoldingType))
                strs.Add(course.HoldingType);
            strs.Add(course.DateStart.ToString("ddd, dd MMM yyyy"));
            if (course.DateEnd != null)
                strs.Add(((DateTime)course.DateEnd).ToString("ddd, dd MMM yyyy"));

            if (sessions.Count > 0)
            {
                strs.Add("Sessions");
                foreach (var x in sessions)
                {
                    if (x.DateStart != null && x.DateEnd != null)
                    {
                        var dt = ((DateTime)x.DateStart).ToString("ddd, dd MMM yyyy");
                        strs.Add(dt + " " + ((DateTime)x.DateStart).ToString("HH:mm") + "-" + ((DateTime)x.DateEnd).ToString("HH:mm"));
                    }

                }
            }
            strs.Add("TRAINING DEPARTMENT");

            var text = String.Join("\n", strs);
            Magfa m = new Magfa();

            var res = new List<long>();
            var hists = new List<CourseSMSHistory>();
            foreach (var p in people)
            {
                var rs = m.enqueue(1, p.Mobile, text)[0];
                var hist = new CourseSMSHistory()
                {
                    CourseId = cid,
                    DateSent = DateTime.Now,
                    Mobil = p.Mobile,
                    Msg = text,
                    PersonId = p.Id,
                    PersonName = p.FirstName + " " + p.LastName,
                    TypeId = 1,
                    RefId = rs,



                };
                hists.Add(hist);
                context.CourseSMSHistories.Add(hist);
                res.Add(rs);
            }
            await Task.Delay(10000);


            var sts = m.getStatus(res);
            int c = 0;
            foreach (var st in sts)
            {
                hists[c].DateStatus = DateTime.Now;
                hists[c].Statu = st;
                if (c == 0)
                {
                    crs.SMSIns1 = text;
                    crs.SMSIns1Status = st;
                    crs.SMSInsDate = DateTime.Now;
                }
                else
                {
                    crs.SMSIns2 = text;
                    crs.SMSIns2Status = st;
                }
                c++;
            }
            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = hists,
                IsSuccess = true,
            };

        }


        public async Task<DataResponse> NotifyCourseRemaining(int dd)
        {
            Magfa m = new Magfa();
            List<string> nos = new List<string>() { "09128070746", "09122106372", "09124449584" };
            var courses = await context.ViewCourseRemainings.Where(q => q.Remaining == dd).ToListAsync();
            var jgs = courses.Select(q => q.JobGroupCode).ToList();
            var jobgroups = await context.ViewJobGroups.Where(q => jgs.Contains(q.FullCode) || jgs.Contains(q.FullCode2)).ToListAsync();

            var sent = new List<CourseRemainingNotification>();
            var hises = new List<CourseRemainingNotification>();
            var refs = new List<Int64>();
            foreach (var x in courses)
            {
                List<string> strs = new List<string>();
                List<string> mngnos = new List<string>();
                //var _mng = jobgroups.FirstOrDefault(q => q.FullCode == x.JobGroupCode || q.FullCode2 == x.JobGroupCode);
                //if (_mng!=null && _mng.Manager != null)
                //{
                //      mngnos = await context.ViewEmployees.Where(q => q.GroupId == _mng.Manager || q.IntervalNDT==_mng.Manager).Select(q => q.Mobile).ToListAsync();
                //}
                strs.Add("EXPIRING NOTIFICATION");
                strs.Add("Dear " + x.Name + ",");
                strs.Add("Your certificate/licence will be expired in " + dd.ToString() + " day(s).");
                strs.Add(x.CourseType);
                strs.Add("Issued:" + ((DateTime)x.DateIssue).ToString("yyyy-MM-dd"));
                strs.Add("Expired:" + ((DateTime)x.DateExpire).ToString("yyyy-MM-dd"));
                strs.Add("TRAINING DEPARTMENT");
                var text = String.Join("\n", strs);
                var rs = m.enqueue(1, x.Mobile, text)[0];
                //foreach(var mo in nos)
                //{
                //    m.enqueue(1, /*x.Mobile*/mo, text) ;
                //}
                //foreach (var mo in mngnos)
                //{
                //    m.enqueue(1, /*x.Mobile*/mo, text);
                //}
                var his = new CourseRemainingNotification()
                {
                    CourseId = x.CourseId,
                    DateExpire = x.DateExpire,
                    DateIssue = x.DateIssue,
                    DateSent = DateTime.Now,
                    Message = text,
                    Mobile = x.Mobile,
                    Name = x.Name,
                    PersonId = x.PersonId,
                    Remaining = dd,
                    Title = x.Title,
                    RefId = rs,
                };
                hises.Add(his);
                //if (rs != -1)
                refs.Add(rs);
                context.CourseRemainingNotifications.Add(his);

            }


            await Task.Delay(5000);


            var sts = m.getStatus(refs);
            int c = 0;
            foreach (var st in sts)
            {

                hises[c].Status = st;

                c++;
            }
            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = hises,
                IsSuccess = true,
            };

        }


        public async Task<DataResponse> EmailCourseRemaining(int dd)
        {

            List<string> nos = new List<string>() { "09128070746", "09122106372", "09124449584" };
            var courses = await context.ViewCourseRemainings.Where(q => q.Remaining == dd).ToListAsync();
            var jgs = courses.Select(q => q.JobGroupCode).ToList();
            var jobgroups = await context.ViewJobGroups.Where(q => jgs.Contains(q.FullCode) || jgs.Contains(q.FullCode2)).ToListAsync();

            var sent = new List<CourseRemainingNotification>();
            var hises = new List<CourseRemainingNotification>();
            var refs = new List<Int64>();
            List<string> strs = new List<string>();
            strs.Add("<b>EXPIRING NOTIFICATION</b>");

            strs.Add("<div>The below certificates/licences will be expired in " + dd.ToString() + " day(s).</div>");
            foreach (var x in courses)
            {


                List<string> mngnos = new List<string>();
                // var _mng = jobgroups.FirstOrDefault(q => q.FullCode == x.JobGroupCode || q.FullCode2 == x.JobGroupCode);
                // if (_mng != null && _mng.Manager != null)
                // {
                //     mngnos = await context.ViewEmployees.Where(q => q.GroupId == _mng.Manager || q.IntervalNDT == _mng.Manager).Select(q => q.Mobile).ToListAsync();
                // }

                strs.Add("<div>" + x.CourseType + ", " + x.Name + ", " + "Issued:" + ((DateTime)x.DateIssue).ToString("yyyy-MM-dd") + ", " + "Expired:" + ((DateTime)x.DateExpire).ToString("yyyy-MM-dd") + "</di>");






            }
            var text = String.Join("", strs);
            var helper = new MailHelper();
            var result = helper.SendTest("v.moghaddam59@gmail.com", text, "EXPIRING NOTIFICATION", 25, 0);



            return new DataResponse()
            {
                Data = strs,
                IsSuccess = true,
            };

        }

        public async Task<DataResponse> GetCourseAttendance(int cid, int pid)
        {
            var attendance = await context.ViewCourseSessionPresenceDetails.Where(q => q.PersonId == pid && q.CourseId == cid).OrderBy(q => q.DateFrom).ToListAsync();

            return new DataResponse()
            {
                Data = attendance,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> SaveCourseAttendance(Attendance att)
        {
            var _dates = att.Date.Split('-').Select(q => Convert.ToInt32(q)).ToList();
            var _from = att.From.Split(':').Select(q => Convert.ToInt32(q)).ToList();
            var _to = att.To.Split(':').Select(q => Convert.ToInt32(q)).ToList();

            var dfrom = new DateTime(_dates[0], _dates[1], _dates[2], _from[0], _from[1], 0);
            var dto = new DateTime(_dates[0], _dates[1], _dates[2], _to[0], _to[1], 0);

            var entity = new CourseSessionPresenceDetail()
            {
                CourseId = att.CourseId,
                PersonId = att.PersonId,
                SessionKey = att.Key,
                DateFrom = dfrom,
                DateTo = dto,
                Remark = att.Remark,
                Date = DateTime.Now

            };
            context.CourseSessionPresenceDetails.Add(entity);

            var saveResult = await context.SaveAsync();

            var view = await this.context.ViewCourseSessionPresenceDetails.Where(q => q.Id == entity.Id).FirstOrDefaultAsync();
            return new DataResponse()
            {
                Data = view,
                IsSuccess = saveResult.IsSuccess,
            };


        }

        public async Task<DataResponse> DeleteCourseAttendance(int cid)
        {
            var obj = await this.context.CourseSessionPresenceDetails.Where(q => q.Id == cid).FirstOrDefaultAsync();
            if (obj != null)
            {
                context.CourseSessionPresenceDetails.Remove(obj);
                var saveResult = await context.SaveAsync();
            }
            return new DataResponse()
            {
                Data = true,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> SyncSessionsToRoster(int cid)
        {
            var sessions = await context.ViewCourseSessions.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();
            var cps = await context.CoursePeoples.Where(q => q.CourseId == cid).ToListAsync();
            var personIds = cps.Select(q => q.PersonId).ToList();
            var fltcrew = new List<string>() { "P1", "P2", "ISCCM", "SCCM", "CCM", "TRE", "TRI", "LTC" };
            var employees = await context.ViewEmployeeAbs.Where(q => personIds.Contains(q.PersonId) && fltcrew.Contains(q.JobGroup)).ToListAsync();
            var currents = await context.CourseSessionFDPs.Where(q => q.CourseId == cid).ToListAsync();
            var fdps = new List<FDP>();
            var errors = new List<object>();
            foreach (var session in sessions)
            {
                foreach (var emp in employees)
                {
                    var exist = currents.Where(q => q.EmployeeId == emp.Id && q.SessionKey == session.Key).FirstOrDefault();
                    if (exist == null)
                    {

                        var ofdp = (from x in context.ViewFDPIdeas.AsNoTracking()
                                    where x.CrewId == emp.Id && x.DutyType == 1165
                                    && (
                                      (session.DateStartUtc >= x.DutyStart && session.DateStartUtc <= x.RestUntil) || (session.DateEndUtc >= x.DutyStart && session.DateEndUtc <= x.RestUntil)
                                      || (x.DutyStart >= session.DateStartUtc && x.DutyStart <= session.DateEndUtc) || (x.RestUntil >= session.DateStartUtc && x.RestUntil <= session.DateEndUtc)
                                      )
                                    select x).FirstOrDefault();
                        if (ofdp == null)
                        {
                            var duty = new FDP();
                            duty.DateStart = session.DateStartUtc;
                            duty.DateEnd = session.DateEndUtc;

                            duty.CrewId = emp.Id;
                            duty.DutyType = 5000;
                            duty.GUID = Guid.NewGuid();
                            duty.IsTemplate = false;
                            duty.Remark = session.CT_Title + "\r\n" + session.Title;
                            duty.UPD = 1;

                            duty.InitStart = duty.DateStart;
                            duty.InitEnd = duty.DateEnd;
                            var rest = new List<int>() { 1167, 1168, 1170, 5000, 5001, 100001, 100003 };
                            duty.InitRestTo = rest.Contains(duty.DutyType) ? ((DateTime)duty.InitEnd).AddHours(12) : duty.DateEnd;
                            //rec.FDP = duty;
                            var csf = new CourseSessionFDP()
                            {
                                FDP = duty,
                                CourseId = session.CourseId,
                                SessionKey = session.Key,
                                EmployeeId = emp.Id,

                            };
                            context.FDPs.Add(duty);
                            context.CourseSessionFDPs.Add(csf);


                            fdps.Add(duty);
                        }
                        else
                        {
                            errors.Add(new
                            {
                                FDPId = ofdp.Id,
                                EmployeeId = emp.Id,
                                SessionItemId = session.Id,
                                Name = emp.Name,
                                Flights = ofdp.InitFlts,
                                Route = ofdp.InitRoute,
                                // DutyEnd = ofdp.DutyEndLocal,
                                DutyStart = ofdp.DutyStart,
                                RestUntil = ofdp.RestUntil,
                                CourseCode = session.CT_Title,
                                CourseTitle = session.Title,
                                SessionDateFrom = session.DateStart,
                                SessionDateTo = session.DateEnd,
                                DateCreate = DateTime.Now
                            });
                        }
                    }

                }
            }
            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = new
                {
                    fdps = fdps.Select(q => new
                    {
                        q.Id,
                        q.CrewId
                    }).ToList(),
                    errors,
                    saveErrors = saveResult.Errors,
                },
                IsSuccess = saveResult.IsSuccess,
            };
        }


        public async Task<DataResponse> SyncSessionsToRosterTeachers(int cid)
        {
            var sessions = await context.ViewCourseSessions.Where(q => q.CourseId == cid).OrderBy(q => q.DateStart).ToListAsync();
            var crs = await context.Courses.Where(q => q.Id == cid).FirstOrDefaultAsync();
            List<int> emps = new List<int>();
            if (crs.CurrencyId != null)
                emps.Add((int)crs.CurrencyId);
            if (crs.Instructor2 != null)
                emps.Add((int)crs.Instructor2);
            // var cps = await context.CoursePeoples.Where(q => q.CourseId == cid).ToListAsync();
            //var personIds = cps.Select(q => q.PersonId).ToList();
            var fltcrew = new List<string>() { "P1", "P2", "ISCCM", "SCCM", "CCM", "TRE", "TRI", "LTC" };
            var employees = await context.ViewEmployeeAbs.Where(q => emps.Contains(q.PersonId) && fltcrew.Contains(q.JobGroup)).ToListAsync();
            var currents = await context.CourseSessionFDPs.Where(q => q.CourseId == cid).ToListAsync();
            var fdps = new List<FDP>();
            var errors = new List<object>();
            foreach (var session in sessions)
            {
                foreach (var emp in employees)
                {
                    var exist = currents.Where(q => q.EmployeeId == emp.Id && q.SessionKey == session.Key).FirstOrDefault();
                    if (exist == null)
                    {

                        var ofdp = (from x in context.ViewFDPIdeas.AsNoTracking()
                                    where x.CrewId == emp.Id && x.DutyType == 1165
                                    && (
                                      (session.DateStartUtc >= x.DutyStart && session.DateStartUtc <= x.RestUntil) || (session.DateEndUtc >= x.DutyStart && session.DateEndUtc <= x.RestUntil)
                                      || (x.DutyStart >= session.DateStartUtc && x.DutyStart <= session.DateEndUtc) || (x.RestUntil >= session.DateStartUtc && x.RestUntil <= session.DateEndUtc)
                                      )
                                    select x).FirstOrDefault();
                        if (ofdp == null)
                        {
                            var duty = new FDP();
                            duty.DateStart = session.DateStartUtc;
                            duty.DateEnd = session.DateEndUtc;

                            duty.CrewId = emp.Id;
                            duty.DutyType = 5000;
                            duty.GUID = Guid.NewGuid();
                            duty.IsTemplate = false;
                            duty.Remark = session.CT_Title + "\r\n" + session.Title;
                            duty.UPD = 1;

                            duty.InitStart = duty.DateStart;
                            duty.InitEnd = duty.DateEnd;
                            var rest = new List<int>() { 1167, 1168, 1170, 5000, 5001, 100001, 100003 };
                            duty.InitRestTo = rest.Contains(duty.DutyType) ? ((DateTime)duty.InitEnd).AddHours(12) : duty.DateEnd;
                            //rec.FDP = duty;
                            var csf = new CourseSessionFDP()
                            {
                                FDP = duty,
                                CourseId = session.CourseId,
                                SessionKey = session.Key,
                                EmployeeId = emp.Id,

                            };
                            context.FDPs.Add(duty);
                            context.CourseSessionFDPs.Add(csf);


                            fdps.Add(duty);
                        }
                        else
                        {
                            errors.Add(new
                            {
                                FDPId = ofdp.Id,
                                EmployeeId = emp.Id,
                                SessionItemId = session.Id,
                                Name = emp.Name,
                                Flights = ofdp.InitFlts,
                                Route = ofdp.InitRoute,
                                // DutyEnd = ofdp.DutyEndLocal,
                                DutyStart = ofdp.DutyStart,
                                RestUntil = ofdp.RestUntil,
                                CourseCode = session.CT_Title,
                                CourseTitle = session.Title,
                                SessionDateFrom = session.DateStart,
                                SessionDateTo = session.DateEnd,
                                DateCreate = DateTime.Now
                            });
                        }
                    }

                }
            }
            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = new
                {
                    fdps = fdps.Select(q => new
                    {
                        q.Id,
                        q.CrewId
                    }).ToList(),
                    errors,
                    saveErrors = saveResult.Errors,
                },
                IsSuccess = saveResult.IsSuccess,
            };
        }

        public async Task<DataResponse> GetEmployeeCertificates(int id)
        {
            var certs = await context.AppCertificates.Where(q =>
                   q.CrewId == id

           ).OrderBy(q => q.StatusId).ThenBy(q => q.Remain).ToListAsync();



            return new DataResponse()
            {
                Data = certs,
                IsSuccess = true,
            };
        }


        public async Task<DataResponse> GetTeacherCourses(int id)
        {
            var certs = await context.ViewTeacherCourses.Where(q =>
                   q.Id == id

           ).OrderBy(q => q.DateStart).ToListAsync();



            return new DataResponse()
            {
                Data = certs,
                IsSuccess = true,
            };
        }


        public async Task<DataResponse> GetCertificateUrl(int person, int type)
        {
            var cp = await context.ViewCoursePeoplePassedRankeds.Where(q => q.PersonId == person && q.CertificateTypeId == type && q.RankLast == 1).FirstOrDefaultAsync();
            if (cp == null)
                return new DataResponse()
                {
                    Data = new ViewCoursePeoplePassedRanked() { Id = -1 },
                    IsSuccess = true,
                };
            if (string.IsNullOrEmpty(cp.ImgUrl))
                return new DataResponse()
                {
                    Data = new ViewCoursePeoplePassedRanked() { Id = -1 },
                    IsSuccess = true,
                };
            return new DataResponse()
            {
                Data = cp,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> GetPersonDocumentFile(int pid, int tid)
        {
            //var query = from x in context.ViewEmployeeTrainings select x;
            //if (root != "000")
            //    query = query.Where(q => q.JobGroupMainCode == root);
            //var result = await query.OrderByDescending(q => q.MandatoryExpired).ThenBy(q => q.JobGroup).ThenBy(q => q.LastName).ToListAsync();
            var obj = context.ViewPersonDocumentFileXes.Where(q => q.PersonId == pid && q.DocumentTypeId == tid).OrderByDescending(q => q.Id).FirstOrDefaultAsync();
            if (obj != null)
                return new DataResponse()
                {
                    Data = obj,
                    IsSuccess = true,
                };
            else
                return new DataResponse()
                {
                    Data = new ViewPersonDocumentFileX() { Id = -1, },
                    IsSuccess = true,
                };
        }

        public async Task<DataResponse> GetPersonCertificateDocument(int pid, int tid, string type)
        {
            //var query = from x in context.ViewEmployeeTrainings select x;
            //if (root != "000")
            //    query = query.Where(q => q.JobGroupMainCode == root);
            //var result = await query.OrderByDescending(q => q.MandatoryExpired).ThenBy(q => q.JobGroup).ThenBy(q => q.LastName).ToListAsync();
            //var obj =await context.ViewPersonDocumentFileXes.Where(q => q.PersonId == pid && q.DocumentTypeId == tid).OrderByDescending(q => q.Id).FirstOrDefaultAsync();
            string fileUrl = null;
            var pdoc = await context.EmployeeDocuments.FirstOrDefaultAsync(q => q.PersonId == pid && q.Type == type);
            if (pdoc == null)
            {
                var obj = await context.ViewPersonDocumentFileXes.Where(q => q.PersonId == pid && q.DocumentTypeId == tid).OrderByDescending(q => q.Id).FirstOrDefaultAsync();
                if (obj != null)
                    fileUrl = obj.FileUrl;
            }
            else
                fileUrl = pdoc.Url;

            var cp = await context.ViewCoursePeoplePassedRankeds.Where(q => q.PersonId == pid && q.CertificateTypeId == tid && q.RankLast == 1).FirstOrDefaultAsync();
            var emp = await context.ViewEmployees.Where(q => q.PersonId == pid).FirstOrDefaultAsync();
            return new DataResponse()
            {
                Data = new { certificate = cp, document = new { FileUrl = fileUrl }, employee = emp },
                IsSuccess = true,
            };

        }


        public async Task<DataResponse> GetTrnStatCoursePeople(DateTime df, DateTime dt, int? ct, int? status, int? cstatus, string cls, int? pid, int? inst1, int? inst2, int? rank, int? active, string grp)
        {
            var _df = df.Date;
            var _dt = dt.Date.AddDays(1);
            var query = from x in context.ViewCoursePeopleRankedByStarts
                        where x.DateStart >= _df && x.DateStart <= _dt
                        select x;
            if (ct != -1)
            {
                query = query.Where(q => q.CourseTypeId == ct);
            }
            if (status != -2)
            {
                query = query.Where(q => q.CoursePeopleStatusId == status);
            }
            if (cstatus != -1)
            {
                query = query.Where(q => q.StatusId == cstatus);
            }
            if (cls != "-1")
            {
                query = query.Where(q => q.No == cls);
            }
            if (inst1 != -1)
            {
                query = query.Where(q => q.Instructor1Id == inst1);
            }
            if (inst2 != -1)
            {
                query = query.Where(q => q.Instructor2Id == inst2);
            }
            if (rank != -1)
            {
                query = query.Where(q => q.RankLast == 1);
            }
            if (active != -1)
            {
                query = query.Where(q => q.CustomerId == 0);
            }
            else query = query.Where(q => q.CustomerId == 1);

            if (grp != "-1")
            {
                query = query.Where(q => q.JobGroupCode.StartsWith(grp));

            }


            var result = await query.OrderBy(q => q.LastName).ThenBy(q => q.FirstName).ThenBy(q => q.CourseType).ThenBy(q => q.RankLast).ToListAsync();

            return new DataResponse()
            {
                Data = result,
                IsSuccess = true,
            };
        }

        public async Task<DataResponse> SavePersonDoc(int personId, string type, string url)
        {
            EmployeeDocument doc = await context.EmployeeDocuments.FirstOrDefaultAsync(q => q.PersonId == personId && q.Type == type);
            if (doc == null)
            {
                doc = new EmployeeDocument() { PersonId = personId, Type = type };
                context.EmployeeDocuments.Add(doc);
            }
            doc.Url = url;


            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = doc,
                IsSuccess = saveResult.IsSuccess,
            };
        }

        public async Task<DataResponse> SaveCourseFP(int courseId, string url)
        {
            var crs = await context.Courses.FirstOrDefaultAsync(q => q.Id == courseId);
            crs.AttForm = url;


            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = crs,
                IsSuccess = saveResult.IsSuccess,
            };
        }

        public async Task<DataResponse> SaveGroupsManager(int managerId, List<int> ids)
        {
            var crs = await context.JobGroups.Where(q => ids.Contains(q.Id)).ToListAsync();
            foreach (var x in crs)
            {
                x.Manager = managerId;
            }


            var saveResult = await context.SaveAsync();
            return new DataResponse()
            {
                Data = crs,
                IsSuccess = saveResult.IsSuccess,
            };
        }


    }
}