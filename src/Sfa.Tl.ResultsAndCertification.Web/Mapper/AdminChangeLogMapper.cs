using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System;
using System.Text.Json.Nodes;

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
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime(s.DateAndTimeOfChange)))
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
                .ForMember(d => d.ChangeDateOfRequest, opts => opts.MapFrom(s => s.ChangeDateOfRequest))
                .ForMember(d => d.ReasonForChange, opts => opts.MapFrom(s => s.ReasonForChange))
                .ForMember(d => d.ZendeskTicketID, opts => opts.MapFrom(s => s.ZendeskTicketID));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordStartYearViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeStartYearDetails, opts => opts.MapFrom(s => GetDetails<ChangeStartYearDetails>(s.ChangeDetails)));

            CreateMap<AdminChangeLogRecord, AdminViewChangeRecordIndustryPlacementViewModel>()
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => FormatDateTime2(s.DateAndTimeOfChange)))
                .ForMember(d => d.ChangeIPDetails, opts => opts.MapFrom(s => GetDetails<ChangeIPDetails>(s.ChangeDetails)));
        }


        private T GetDetails<T>(string details)
            => JsonConvert.DeserializeObject<T>(details);

        private static string FormatDateTime(DateTime dateTime)
            => $"{dateTime:d MMMM yyyy h:mm}{dateTime.ToString("tt").ToLower()}";

        private static string FormatDateTime2(DateTime dateTime)
            => $"{dateTime:d MMMM yyyy, hh:mm}{dateTime.ToString("tt").ToLower()}";

    }
}