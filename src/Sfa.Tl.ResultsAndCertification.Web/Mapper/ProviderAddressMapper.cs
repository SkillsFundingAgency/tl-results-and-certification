using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class ProviderAddressMapper : Profile
    {
        public ProviderAddressMapper()
        {
            CreateMap<PostcodeLookupResult, AddAddressSelectViewModel>()
                .ForMember(d => d.AddressSelectList, opts => opts.MapFrom(s => s.AddressResult))
                .ForAllOtherMembers(d => d.Ignore());

            CreateMap<AddressResult, SelectListItem>()
                .ForMember(m => m.Text, o => o.MapFrom(s => s.DeliveryPointAddress.FormattedAddress))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.DeliveryPointAddress.Uprn))
                .ForAllOtherMembers(s => s.Ignore());

            CreateMap<AddressResult, AddressViewModel>()
                .ForMember(d => d.Udprn, opts => opts.MapFrom(s => s.DeliveryPointAddress.Uprn))
                .ForMember(d => d.FormattedAddress, opts => opts.MapFrom(s => s.DeliveryPointAddress.FormattedAddress))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.DeliveryPointAddress.AddressLine1))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.DeliveryPointAddress.AddressLine2))
                .ForMember(d => d.Town, opts => opts.MapFrom(s => s.DeliveryPointAddress.Town))
                .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.DeliveryPointAddress.Postcode))
                .ForAllOtherMembers(d => d.Ignore());
        }
    }
}