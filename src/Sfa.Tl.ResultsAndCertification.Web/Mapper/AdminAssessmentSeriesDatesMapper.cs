using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminAssessmentSeriesDateDetails;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminAssessmentSeriesDatesMapper : Profile
    {
        public AdminAssessmentSeriesDatesMapper()
        {
            CreateMap<GetAssessmentSeriesDatesDetailsResponse, AdminAssessmentSeriesDetailsViewModel>()
              .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
              .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
              .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => CreateSummary("component", AdminAssessmentSeriesDateDetails.Label_Component, s.ComonentType.ToString())))
              .ForMember(d => d.SummaryResultCalculationYear, opts => opts.MapFrom(s => CreateSummary("resultcalculationyear", AdminAssessmentSeriesDateDetails.Label_Start_Date, s.StartDate.ToDobFormat())))
              .ForMember(d => d.SummaryStartDate, opts => opts.MapFrom(s => CreateSummary("startdate", AdminAssessmentSeriesDateDetails.Label_Start_Date, s.StartDate.ToDobFormat())))
              .ForMember(d => d.SummaryEndDate, opts => opts.MapFrom(s => CreateSummary("enddate", AdminAssessmentSeriesDateDetails.Label_End_Date, s.EndDate.ToDobFormat())))
              .ForMember(d => d.SummaryRommEndDate, opts => opts.MapFrom(s => CreateSummary("rommenddate", AdminAssessmentSeriesDateDetails.Label_Romm_End_Date, s.RommEndDate.ToDobFormat())))
              .ForMember(d => d.SummaryAppealEndDate, opts => opts.MapFrom(s => CreateSummary("appealenddate", AdminAssessmentSeriesDateDetails.Label_Appeal_End_Date, s.AppealEndDate.ToDobFormat())))
              .ForMember(d => d.SummaryResultPublishDate, opts => opts.MapFrom(s => CreateSummary("resultspublishdate", AdminAssessmentSeriesDateDetails.Label_Result_Publish_Date, s.ResultPublishDate.ToDobFormat())))
              .ForMember(d => d.SummaryPrintAvailableDate, opts => opts.MapFrom(s => CreateSummary("printavailabledate", AdminAssessmentSeriesDateDetails.Label_Print_Available_Date, s.PrintAvailableDate.ToDobFormat())));

            CreateMap<GetAssessmentSeriesDatesDetailsResponse, AdminAssessmentSeriesViewModel>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComonentType))
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, opts => opts.MapFrom(s => s.EndDate));

            CreateMap<AdminAssessmentSeriesDatesCriteriaViewModel, SearchAssessmentSeriesDatesRequest>()
                .ForMember(d => d.SelectedFilters, opts => opts.MapFrom(s => s.ActiveFilters.Where(f => f.IsSelected).Select(f => f.Id).ToList()));

        }
        private static SummaryItemModel CreateSummary(string id, string title, string value)
              => new()
              {
                  Id = id,
                  Title = title,
                  Value = value
              };
    }
}