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
        }
    }
}
