using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ResultMapper : Profile
    {
        public ResultMapper()
        {
            CreateMap<TqRegistrationPathway, ResultDetails>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.TqRegistrationProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.PathwayLarId, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.TqPathwayAssessments.Any() ? s.TqPathwayAssessments.FirstOrDefault().AssessmentSeries.Name : null))
                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TlSpecialism.LarId : null))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => s.TqRegistrationSpecialisms.Any() ? s.TqRegistrationSpecialisms.FirstOrDefault().TlSpecialism.Name : null))
                .ForMember(d => d.PathwayResult, opts => 
                                opts.MapFrom(s => s.TqPathwayAssessments.Any() && s.TqPathwayAssessments.FirstOrDefault().TqPathwayResults.Any() ?
                                s.TqPathwayAssessments.FirstOrDefault().TqPathwayResults.FirstOrDefault().TlLookup.Value : null))
                .ForMember(d => d.PathwayResultId, opts =>
                                opts.MapFrom(s => s.TqPathwayAssessments.Any() && s.TqPathwayAssessments.FirstOrDefault().TqPathwayResults.Any() ?
                                s.TqPathwayAssessments.FirstOrDefault().TqPathwayResults.FirstOrDefault().Id : (int?)null))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status));
        }
    }
}