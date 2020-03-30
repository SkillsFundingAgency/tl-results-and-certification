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
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn))
               .ForMember(d => d.Tlevels, opts => opts.MapFrom(s => s.Tlevels))
               .ForMember(d => d.HasTlevelSelected, opts => opts.Ignore());

            CreateMap<ProviderTlevelDetails, ProviderTlevelDetailsViewModel>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => $"{s.RouteName}: {s.PathwayName}"))
               .ForMember(d => d.IsSelected, opts => opts.Ignore());

            CreateMap<ProviderTlevelDetailsViewModel, ProviderTlevelDetails>()
               .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<ProviderTlevelDetailsViewModel, ProviderTlevelDetails>>());

            CreateMap<ProviderMetadata, ProviderLookupData>();

            CreateMap<ProviderTlevels, ProviderViewModel>()
               //.ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn))
               .ForMember(d => d.Tlevels, opts => opts.MapFrom(s => s.Tlevels));
            //.ForMember(d => d.HasTlevelSelected, opts => opts.Ignore());

            CreateMap<ProviderTlevelDetails, TlevelViewModel>()
               //.ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               //.ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
               //.ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.TqProviderId, opts => opts.MapFrom(s => s.TqProviderId.HasValue ? s.TqProviderId.Value : 0))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => $"{s.RouteName}: {s.PathwayName}"));
               //.ForMember(d => d.IsSelected, opts => opts.Ignore());


            CreateMap<ProviderDetails, ProviderDetailsViewModel>()
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn));
        }
    }
}
