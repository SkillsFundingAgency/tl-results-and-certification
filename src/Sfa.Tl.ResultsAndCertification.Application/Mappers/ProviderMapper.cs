using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<ProviderTlevel, TqProvider>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy));

            CreateMap<TlProvider, ProviderDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.UkPrn));

            CreateMap<TlProvider, ProviderMetadata>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName));

            CreateMap<TqProvider, ProviderTlevelDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.TlProvider.DisplayName))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.TlProvider.UkPrn))
                .ForMember(d => d.ProviderTlevel, opts => opts.MapFrom(s => s));

            CreateMap<TqProvider, ProviderTlevel>()
                .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TqAwardingOrganisation.TlPathway.TlevelTitle))
                .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TlPathway, PathwayDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Code, opts => opts.MapFrom(s => s.LarId))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.TlevelTitle));
        }
    }
}
