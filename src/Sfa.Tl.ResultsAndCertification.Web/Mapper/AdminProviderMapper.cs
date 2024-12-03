using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminProviderMapper : Profile
    {
        public AdminProviderMapper()
        {
            CreateMap<GetProviderResponse, AdminProviderDetailsViewModel>()
                .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.SummaryUkprn, opts => opts.MapFrom(s => CreateSummary("ukprn", AdminProviderDetails.Label_Ukprn, s.UkPrn.ToString())))
                .ForMember(d => d.SummaryName, opts => opts.MapFrom(s => CreateSummary("name", AdminProviderDetails.Label_Name, s.Name)))
                .ForMember(d => d.SummaryDisplayName, opts => opts.MapFrom(s => CreateSummary("displayname", AdminProviderDetails.Label_Display_Name, s.DisplayName)))
                .ForMember(d => d.SummaryIsActive, opts => opts.MapFrom(s => CreateSummary("active", AdminProviderDetails.Label_Active, s.IsActive ? AdminProviderDetails.Label_Yes : AdminProviderDetails.Label_No)));

            CreateMap<GetProviderResponse, AdminEditProviderViewModel>()
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
               .ForMember(d => d.UkPrn, opts => opts.MapFrom(s => s.UkPrn.ToString()))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
               .ForMember(d => d.IsActive, opts => opts.MapFrom(s => s.IsActive));

            CreateMap<AdminEditProviderViewModel, UpdateProviderRequest>()
               .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.ProviderId))
               .ForMember(d => d.UkPrn, opts => opts.MapFrom(s => long.Parse(s.UkPrn)))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name.Trim()))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName.Trim()))
               .ForMember(d => d.IsActive, opts => opts.MapFrom(s => s.IsActive))
               .ForMember(d => d.ModifiedBy, opts => opts.MapFrom<UserNameResolver<AdminEditProviderViewModel, UpdateProviderRequest>>());

            CreateMap<AdminAddProviderViewModel, AddProviderRequest>()
               .ForMember(d => d.UkPrn, opts => opts.MapFrom(s => long.Parse(s.UkPrn)))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name.Trim()))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName.Trim()))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminAddProviderViewModel, AddProviderRequest>>());
        }

        private static SummaryItemModel CreateSummary(string id, string title, string value)
            => new()
            {
                Id = id,
                Title = title,
                Value = value
            };
    }
}