using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<ProviderTlevels, SelectProviderTlevelViewModel>()
               .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
               .ForMember(d => d.ProviderDisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Ukprn))
               .ForMember(d => d.ProviderTlevels, opts => opts.MapFrom(s => s.Tlevels))
               .ForMember(d => d.HasTlevelSelected, opts => opts.Ignore());

            CreateMap<SelectProviderTlevel, ProviderTlevelsViewModel>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
               .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.TlProviderId))
               .ForMember(d => d.TlPathwayId, opts => opts.MapFrom(s => s.TlPathwayId))
               .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.RouteName))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.IsSelected, opts => opts.Ignore());
        }
    }
}
