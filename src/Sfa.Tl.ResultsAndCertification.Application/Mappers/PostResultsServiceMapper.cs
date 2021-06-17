using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class PostResultsServiceMapper : Profile
    {
        public PostResultsServiceMapper()
        {
            CreateMap<TqRegistrationPathway, FindPrsLearnerRecord>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.TqRegistrationProfile.Id))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname + " " + s.TqRegistrationProfile.Lastname))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name + " (" + s.TqProvider.TlProvider.UkPrn + ")"))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
               .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status));
        }
    }
}
