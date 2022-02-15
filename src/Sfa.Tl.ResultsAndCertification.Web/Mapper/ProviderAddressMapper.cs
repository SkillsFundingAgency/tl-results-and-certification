using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class ProviderAddressMapper : Profile
    {
        public ProviderAddressMapper()
        {
            CreateMap<PostcodeLookupResult, AddAddressSelectViewModel>()
                .ForMember(d => d.AddressSelectList, opts => opts.MapFrom(s => s.AddressResult));

            CreateMap<AddressResult, SelectListItem>()
                .ForMember(m => m.Text, o => o.MapFrom(s => s.DeliveryPointAddress.FormattedAddress))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.DeliveryPointAddress.Uprn));

            CreateMap<AddressResult, AddressViewModel>()
                .ForMember(d => d.Udprn, opts => opts.MapFrom(s => s.DeliveryPointAddress.Uprn))
                .ForMember(d => d.FormattedAddress, opts => opts.MapFrom(s => s.DeliveryPointAddress.FormattedAddress))
                .ForMember(d => d.OrganisationName, opts => opts.MapFrom(s => s.DeliveryPointAddress.OrganisationName))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.DeliveryPointAddress.FormattedBuildingName) ? s.DeliveryPointAddress.FormattedBuildingName : s.DeliveryPointAddress.FormattedBuildingNumberAndThroughfare))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.DeliveryPointAddress.FormattedBuildingName) ? s.DeliveryPointAddress.FormattedBuildingNumberAndThroughfare : null))
                .ForMember(d => d.Town, opts => opts.MapFrom(s => s.DeliveryPointAddress.Town))
                .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.DeliveryPointAddress.Postcode));

            CreateMap<AddAddressViewModel, AddAddressRequest>()
               .ForMember(d => d.Ukprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
               .ForMember(d => d.DepartmentName, opts => opts.MapFrom(s => s.AddAddressSelect != null ? s.AddAddressSelect.DepartmentName : s.AddAddressManual.DepartmentName))
               .ForMember(d => d.OrganisationName, opts => opts.MapFrom(s => s.AddAddressSelect != null ? s.AddAddressSelect.SelectedAddress.OrganisationName : s.AddAddressManual.OrganisationName))
               .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.AddAddressSelect != null ? s.AddAddressSelect.SelectedAddress.AddressLine1 : s.AddAddressManual.AddressLine1))
               .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.AddAddressSelect != null ? s.AddAddressSelect.SelectedAddress.AddressLine2 : s.AddAddressManual.AddressLine2))
               .ForMember(d => d.Town, opts => opts.MapFrom(s => s.AddAddressSelect != null ? s.AddAddressSelect.SelectedAddress.Town : s.AddAddressManual.Town))
               .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.AddAddressSelect != null ? s.AddAddressSelect.SelectedAddress.Postcode : s.AddAddressManual.Postcode))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddAddressViewModel, AddAddressRequest>>());

            CreateMap<Address, ManagePostalAddressViewModel>()
                .ForMember(d => d.DepartmentName, opts => opts.MapFrom(s => s.DepartmentName))
                .ForMember(d => d.OrganisationName, opts => opts.MapFrom(s => s.OrganisationName))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.AddressLine2))
                .ForMember(d => d.Town, opts => opts.MapFrom(s => s.Town))
                .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.Postcode));
        }
    }
}