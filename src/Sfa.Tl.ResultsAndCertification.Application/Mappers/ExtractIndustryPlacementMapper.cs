using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.ExtractIndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ExtractIndustryPlacementMapper : Profile
    {
        public ExtractIndustryPlacementMapper()
        {
            CreateMap<IndustryPlacement, ExtractData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Firstname))
                .ForMember(d => d.UKPRN, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.Status))
                .ForMember(d => d.PendingWithdrawnFlag, opts => opts.MapFrom(s => s.TqRegistrationPathway.IsPendingWithdrawal.ToString()));
                //.ForMember(d => d.Details, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details)));
        }
    }
}