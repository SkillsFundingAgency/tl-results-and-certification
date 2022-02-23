using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AssessmentMapper : Profile
    {
        public AssessmentMapper()
        {
            CreateMap<AssessmentSeries, AvailableAssessmentSeries>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items["profileId"]))
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.Name));

            CreateMap<TqPathwayAssessment, AssessmentEntryDetails>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Id))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.AssessmentSeries.Id))
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.AssessmentSeries.Name));

            CreateMap<TqSpecialismAssessment, AssessmentEntryDetails>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.Id))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.AssessmentSeries.Id))
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.AssessmentSeries.Name));

            CreateMap<AssessmentSeries, AssessmentSeriesDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Description, opts => opts.MapFrom(s => s.Description))
                .ForMember(d => d.Year, opts => opts.MapFrom(s => s.Year))
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate));
        }
    }
}
