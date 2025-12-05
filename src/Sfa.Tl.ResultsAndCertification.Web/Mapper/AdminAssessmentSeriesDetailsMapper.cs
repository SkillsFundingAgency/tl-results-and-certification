using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminAssessmentSeriesDetailsMapper : Profile
    {
        public AdminAssessmentSeriesDetailsMapper()
        {
            CreateMap<GetAssessmentSeriesDatesResponse, AdminAssessmentSeriesDateDetailsViewModel>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComonentType))
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom(s => s.RommEndDate))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.ResultPublishDate, opts => opts.MapFrom(s => s.ResultPublishDate))
                .ForMember(d => d.PrintAvailableDate, opts => opts.MapFrom(s => s.PrintAvailableDate));

            CreateMap<AdminAssessmentSeriesDatesCriteriaViewModel, SearchAssessmentSeriesDatesRequest>()
                .ForMember(d => d.SelectedFilters, opts => opts.MapFrom(s => s.ActiveFilters.Where(f => f.IsSelected).Select(f => f.Id).ToList()));
        }
    }
}