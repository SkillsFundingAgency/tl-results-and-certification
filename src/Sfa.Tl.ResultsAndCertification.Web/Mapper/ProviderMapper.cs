﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;

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
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<ProviderDetails, ProviderDetailsViewModel>()
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn));

            CreateMap<IList<ProviderDetailsViewModel>, YourProvidersViewModel>()
               .ForMember(d => d.Providers, opts => opts.MapFrom(s => s));

            CreateMap<ProviderTlevelDetails, ProviderTlevelDetailsViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.Ukprn))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.ProviderTlevel.TlevelTitle))
               .ForMember(d => d.TlProviderId, opts => opts.MapFrom(s => s.ProviderTlevel.TlProviderId));

            CreateMap<ProviderDetails, SelectListItem>()
                .ForMember(m => m.Text, o => o.MapFrom(s => $"{s.DisplayName} ({s.Ukprn})"))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.Ukprn.ToString()));

            CreateMap<IList<ProviderDetails>, SelectProviderViewModel>()
               .ForMember(d => d.ProvidersSelectList, opts => opts.MapFrom(s => s));

            CreateMap<PathwayDetails, SelectListItem>()
                .ForMember(m => m.Text, o => o.MapFrom(s => $"{s.Name} ({s.Code})"))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.Code.ToString()));

            CreateMap<IList<PathwayDetails>, SelectCoreViewModel>()
               .ForMember(d => d.CoreSelectList, opts => opts.MapFrom(s => s));
        }
    }
}
