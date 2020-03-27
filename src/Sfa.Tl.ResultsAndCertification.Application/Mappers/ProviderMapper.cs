using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<ProviderTlevelDetails, TqProvider>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.ProviderId))
                .ForMember(d => d.TlPathwayId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy));

            CreateMap<TqProvider, ProviderDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.TlProvider.Id))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.TlProvider.DisplayName))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.TlProvider.UkPrn));

            CreateMap<TlProvider, ProviderMetadata>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName));
        }
    }
}
