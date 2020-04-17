using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<ProviderTlevels, ProviderTlevelsViewModel>()
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn))
               .ForMember(d => d.Tlevels, opts => opts.MapFrom(s => s.Tlevels))
               .ForMember(d => d.HasTlevelSelected, opts => opts.Ignore());

            CreateMap<ProviderTlevel, ProviderTlevelViewModel>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.IsSelected, opts => opts.Ignore());

            CreateMap<ProviderTlevelViewModel, ProviderTlevel>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<ProviderTlevelViewModel, ProviderTlevel>>());

            CreateMap<ProviderMetadata, ProviderLookupData>();

            CreateMap<ProviderTlevels, ProviderViewModel>()
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn))
               .ForMember(d => d.Tlevels, opts => opts.MapFrom(s => s.Tlevels));

            CreateMap<ProviderTlevel, TlevelViewModel>()
               .ForMember(d => d.TqProviderId, opts => opts.MapFrom(s => s.TqProviderId))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => $"{s.RouteName}: {s.PathwayName}"));

            CreateMap<ProviderDetails, ProviderDetailsViewModel>()
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn));

            CreateMap<ProviderTlevelDetails, ProviderTlevelDetailsViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => $"{s.ProviderTlevel.RouteName}: {s.ProviderTlevel.PathwayName}"))
               .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.ProviderTlevel.TlProviderId));
        }
    }
}
