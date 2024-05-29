using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminChangeLogMapper : Profile
    {
        public AdminChangeLogMapper()
        {
            CreateMap<PagedResponse<AdminSearchChangeLog>, AdminSearchChangeLogViewModel>()
                .ForMember(d => d.TotalRecords, opts => opts.MapFrom(s => s.TotalRecords))
                .ForMember(d => d.ChangeLogDetails, opts => opts.MapFrom(s => s.Records))
                .ForMember(d => d.PagerInfo, opts => opts.MapFrom(s => s.PagerInfo))
                .ForMember(d => d.SearchCriteriaViewModel, opts => opts.MapFrom((src, dest, destMember, context) =>
                    new AdminSearchChangeLogCriteriaViewModel
                    {
                        SearchKey = (string)context.Items["searchKey"],
                        PageNumber = (int?)context.Items["pageNumber"]
                    }));

            CreateMap<AdminSearchChangeLog, AdminSearchChangeLogDetailsViewModel>()
                .ForMember(d => d.ChangeLogId, opts => opts.MapFrom(s => s.ChangeLogId))
                .ForMember(d => d.ChangeType, opts => opts.MapFrom(s => s.ChangeType))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeRecordLink, opts => opts.MapFrom(s => GetViewChangeRecordLink(s.DateAndTimeOfChange, s.ChangeLogId, (ChangeType)s.ChangeType)))
                .ForMember(d => d.ZendeskTicketID, opts => opts.MapFrom(s => s.ZendeskTicketID))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.LearnerFirstname} {s.LearnerLastname} ({s.Uln})"))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.ProviderName} ({s.ProviderUkprn})"))
                .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.LastUpdatedBy));


            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordViewModel>()
                .ForMember(d => d.ChangeLogId, opts => opts.MapFrom(s => s.ChangeLogId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.ChangeType, opts => opts.MapFrom(s => s.ChangeType))
                .ForMember(d => d.ChangeDetails, opts => opts.MapFrom(s => s.ChangeDetails))
                .ForMember(d => d.ChangeRequestedBy, opts => opts.MapFrom(s => s.ChangeRequestedBy))
                .ForMember(d => d.ReasonForChange, opts => opts.MapFrom(s => s.ReasonForChange))
                .ForMember(d => d.ZendeskTicketID, opts => opts.MapFrom(s => s.ZendeskTicketID));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordStartYearViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.ChangeStartYearDetails, opts => opts.MapFrom(s => GetDetails<ChangeStartYearDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordIndustryPlacementViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.ChangeIPDetails, opts => opts.MapFrom(s => GetDetails<ChangeIndustryPlacementRequest>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordCoreAssessmentViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.CoreAssessmentDetails, opts => opts.MapFrom(s => GetDetails<AddCoreAssessmentDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordSpecialismAssessmentViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => s.SpecialismCode))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.SpecialismName} ({s.SpecialismCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom(s => GetDetails<AddSpecialismDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordAddPathwayResultViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetAssessmentExamPeriod(s.Pathway.PathwayAssessments, GetDetails<AddPathwayResultRequest>, s.ChangeDetails, "PathwayAssessmentId")))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.PathwayResultDetails, opts => opts.MapFrom(s => GetDetails<AddPathwayResultRequest>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordAddSpecialismResultViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetSpecialismExamPeriod(s.Pathway.Specialisms, GetDetails<OpenSpecialismAppealRequest>, s.ChangeDetails, Constants.SpecialismResultId)))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.SpecialismName} ({s.SpecialismCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom(s => GetDetails<AddSpecialismResultRequest>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordRemoveCoreAssessmentViewModel>()
               .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
               .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
               .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
               .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
               .ForMember(d => d.DetailsChangeAssessment, opts => opts.MapFrom(s => GetDetails<DetailsChangeAssessmentRemove>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordRemoveSpecialismAssessmentViewModel>()
               .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
               .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
               .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
               .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
               .ForMember(d => d.DetailsChangeAssessment, opts => opts.MapFrom(s => GetDetails<DetailsSpecialismAssessmentRemove>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordPathwayResultViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetResultExamPeriod(s.Pathway.PathwayAssessments, GetDetails<ChangePathwayDetails>, s.ChangeDetails, Constants.PathwayResultId)))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.PathwayResultDetails, opts => opts.MapFrom(s => GetDetails<ChangePathwayDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordSpecialismResultViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.SpecialismExamPeriod))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.SpecialismName} ({s.SpecialismCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom(s => GetDetails<ChangeSpecialismDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordOpenPathwayRommViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => GetResultGrade(s.Pathway.PathwayAssessments, GetDetails<OpenPathwayRommRequest>, s.ChangeDetails, Constants.PathwayResultId)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetResultExamPeriod(s.Pathway.PathwayAssessments, GetDetails<OpenPathwayRommRequest>, s.ChangeDetails, Constants.PathwayResultId)))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordOpenSpecialismRommViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => GetSpecialismResultGrade(s.Pathway.Specialisms, GetDetails<OpenSpecialismRommRequest>, s.ChangeDetails, Constants.SpecialismResultId)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.SpecialismExamPeriod))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.SpecialismName} ({s.SpecialismCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordPathwayRommOutcomeViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetAssessmentExamPeriod(s.Pathway.PathwayAssessments, GetDetails<PathwayRommOutcomeDetails>, s.ChangeDetails, Constants.PathwayAssessmentId)))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.RommOutcomeDetails, opts => opts.MapFrom(s => GetDetails<PathwayRommOutcomeDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordSpecialismRommOutcomeViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.SpecialismExamPeriod))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.SpecialismName} ({s.SpecialismCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.RommOutcomeDetails, opts => opts.MapFrom(s => GetDetails<SpecialismRommOutcomeDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordOpenPathwayAppealViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => GetResultGrade(s.Pathway.PathwayAssessments, GetDetails<OpenCoreAppealRequest>, s.ChangeDetails, Constants.PathwayResultId)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetResultExamPeriod(s.Pathway.PathwayAssessments, GetDetails<OpenCoreAppealRequest>, s.ChangeDetails, Constants.PathwayResultId)))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordOpenSpecialismAppealViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.SpecialismName} ({s.SpecialismCode})"))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => GetSpecialismResultGrade(s.Pathway.Specialisms, GetDetails<OpenSpecialismAppealRequest>, s.ChangeDetails, Constants.SpecialismResultId)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetSpecialismExamPeriod(s.Pathway.Specialisms, GetDetails<OpenSpecialismAppealRequest>, s.ChangeDetails, Constants.SpecialismResultId)))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordPathwayAppealOutcomeViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.CoreCode))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.CoreGrade))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetResultExamPeriod(s.Pathway.PathwayAssessments, GetDetails<PathwayAppealOutcomeDetails>, s.ChangeDetails, Constants.PathwayResultId)))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.CoreCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.AppealOutcomeDetails, opts => opts.MapFrom(s => GetDetails<PathwayAppealOutcomeDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordSpecialismAppealOutcomeViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => s.SpecialismCode))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => GetSpecialismResultGrade(s.Pathway.Specialisms, GetDetails<SpecialismAppealOutcomeDetails>, s.ChangeDetails, Constants.SpecialismResultId)))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => GetSpecialismExamPeriod(s.Pathway.Specialisms, GetDetails<SpecialismAppealOutcomeDetails>, s.ChangeDetails, Constants.SpecialismResultId)))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.SpecialismName} ({s.SpecialismCode})"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest.ToDobFormat()))
                .ForMember(d => d.AppealOutcomeDetails, opts => opts.MapFrom(s => GetDetails<SpecialismAppealOutcomeDetails>(s.ChangeDetails)));
        }

        private T GetDetails<T>(string details)
            => JsonConvert.DeserializeObject<T>(details);

        private static string FormatDateTime(DateTime dateTime)
            => $"{dateTime:d MMMM yyyy h:mm}{dateTime.ToString("tt").ToLower()}";

        private static string FormatDateTime2(DateTime dateTime)
            => $"{dateTime:d MMMM yyyy, h:mm}{dateTime.ToString("tt").ToLower()}";

        private AdminChangeLogAssessment GetPathwayAssessment(IEnumerable<AdminChangeLogAssessment> source, Expression<Func<AdminChangeLogAssessment, bool>> expression)
            => source.AsQueryable().Where(expression).SingleOrDefault();

        private AdminChangeLogAssessment GetSpecialismAssessment(IEnumerable<AdminChangeLogSpecialism> source, Expression<Func<AdminChangeLogAssessment, bool>> expression)
            => source.AsQueryable().SelectMany(e => e.Assessments).First(expression);

        private ChangeRecordModel GetViewChangeRecordLink(DateTime text, int changeLogId, ChangeType changeType) => new ChangeRecordModel()
        {
            Text = FormatDateTime(text).ToString(),
            Route = GetRoute(changeType),
            RouteAttributes = new System.Collections.Generic.Dictionary<string, string>()
            {
                {
                    Constants.ChangeLogId, changeLogId.ToString()
                }
            }
        };

        private string GetSpecialismResultGrade<TResultType>(IEnumerable<AdminChangeLogSpecialism> specialisms, Func<string, TResultType> getResultObject, string jsonString, string specialismResultId)
        {
            var value = getPropValue(getResultObject, jsonString, specialismResultId);
            return GetSpecialismAssessment(specialisms, e => e.Results.Count() > 0).Results.First(r => r.Id == value).Grade;
        }

        private string GetSpecialismExamPeriod<TResultType>(IEnumerable<AdminChangeLogSpecialism> specialisms, Func<string, TResultType> getResultObject, string jsonString, string specialismResultId)
        {
            var value = getPropValue(getResultObject, jsonString, specialismResultId);
            return GetSpecialismAssessment(specialisms, e => e.Results.Any(r => r.Id == value)).SeriesName;
        }

        private string GetResultGrade<TResultType>(IEnumerable<AdminChangeLogAssessment> assessments, Func<string, TResultType> getResultObject, string jsonString, string resultId)
        {
            var value = getPropValue(getResultObject, jsonString, resultId);
            return GetPathwayAssessment(assessments.Where(p => p.Results.Count() > 0), p => p.Results.Any(r => r.Id == value)).Results.First(r => r.Id == value).Grade;
        }

        private string GetResultExamPeriod<TResultType>(IEnumerable<AdminChangeLogAssessment> assessments, Func<string, TResultType> getResultObject, string jsonString, string resultId)
        {
            var value = getPropValue(getResultObject, jsonString, resultId);
            return GetPathwayAssessment(assessments.Where(p => p.Results.Count() > 0), p => p.Results.Any(r => r.Id == value)).SeriesName;
        }

        private string GetAssessmentExamPeriod<TResultType>(IEnumerable<AdminChangeLogAssessment> assessments, Func<string, TResultType> getResultObject, string jsonString, string assessmentId)
        {
            var value = getPropValue(getResultObject, jsonString, assessmentId);
            return GetPathwayAssessment(assessments.Where(p => p.Results.Count() > 0), e => e.Id == value).SeriesName;
        }

        private int getPropValue<T>(Func<string, T> getobject, string jsonstr, string prop)
        {
            var resobj = getobject(jsonstr);

            if (resobj == null)
                return default;

            return Convert.ToInt32(getValue(resobj, resobj.GetType(), prop));
        }

        private object getValue(Object obj, Type type, string property) => type.GetProperty(property).GetValue(obj, null);

        private string GetRoute(ChangeType changeType) => changeType switch
        {
            ChangeType.StartYear => RouteConstants.AdminViewChangeStartYearRecord,
            ChangeType.IndustryPlacement => RouteConstants.AdminViewChangeIPRecord,
            ChangeType.AddPathwayAssessment => RouteConstants.AdminViewChangeCoreAssessmentRecord,
            ChangeType.AddSpecialismAssessment => RouteConstants.AdminViewChangeSpecialismAssessmentRecord,
            ChangeType.RemovePathwayAssessment => RouteConstants.AdminViewChangeRemoveCoreAssessmentRecord,
            ChangeType.RemoveSpecialismAssessment => RouteConstants.AdminViewChangeRemoveSpecialismAssessmentRecord,
            ChangeType.AddPathwayResult => RouteConstants.AdminViewChangeAddPathwayResultRecord,
            ChangeType.AddSpecialismResult => RouteConstants.AdminViewChangeAddSpecialismResultRecord,
            ChangeType.ChangePathwayResult => RouteConstants.AdminViewChangePathwayResultRecord,
            ChangeType.ChangeSpecialismResult => RouteConstants.AdminViewChangeSpecialismResultRecord,
            ChangeType.OpenPathwayRomm => RouteConstants.AdminViewOpenPathwayRommRecord,
            ChangeType.OpenSpecialismRomm => RouteConstants.AdminViewOpenSpecialismRommRecord,
            ChangeType.PathwayRommOutcome => RouteConstants.AdminViewPathwayRommOutcomeRecord,
            ChangeType.SpecialismRommOutcome => RouteConstants.AdminViewSpecialismRommOutcomeRecord,
            ChangeType.OpenPathwayAppeal => RouteConstants.AdminViewOpenPathwayAppealRecord,
            ChangeType.OpenSpecialismAppeal => RouteConstants.AdminViewOpenSpecialismAppealRecord,
            ChangeType.PathwayAppealOutcome => RouteConstants.AdminViewPathwayAppealOutcomeRecord,
            ChangeType.SpecialismAppealOutcome => RouteConstants.AdminViewSpecialismAppealOutcomeRecord,
            _ => string.Empty
        };
    }
}