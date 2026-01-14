using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminAssessmentSeriesDatesMapper : Profile
    {
        public AdminAssessmentSeriesDatesMapper()
        {
            CreateMap<AssessmentSeries, GetAssessmentSeriesDatesDetailsResponse>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => (int)s.ComponentType))
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.ResultCalculationYear, opts => opts.MapFrom(s => s.ResultCalculationYear))
                .ForMember(d => d.EndDate, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom(s => s.RommEndDate))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.ResultPublishDate, opts => opts.MapFrom(s => s.ResultPublishDate))
                .ForMember(d => d.PrintAvailableDate, opts => opts.MapFrom(s => s.PrintAvailableDate));
        }
    }
}