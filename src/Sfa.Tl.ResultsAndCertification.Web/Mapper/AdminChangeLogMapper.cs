using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;

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
                .ForMember(d => d.DateAndTimeOfChange, opts => opts.MapFrom(s => s.DateAndTimeOfChange))
                .ForMember(d => d.ZendeskTicketID, opts => opts.MapFrom(s => s.ZendeskTicketID))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.LearnerFirstname} {s.LearnerLastname} ({s.Uln})"))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.ProviderName} ({s.ProviderUkprn})"))
                .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.LastUpdatedBy));
        }
    }
}