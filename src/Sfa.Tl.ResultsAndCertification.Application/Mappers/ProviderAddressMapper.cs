using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ProviderAddressMapper : Profile
    {
        public ProviderAddressMapper()
        {
            CreateMap<AddAddressRequest, TlProviderAddress>()
                .ForMember(d => d.TlProviderId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items["providerId"]))
                .ForMember(d => d.DepartmentName, opts => opts.MapFrom(s => s.DepartmentName))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.AddressLine2))
                .ForMember(d => d.Town, opts => opts.MapFrom(s => s.Town))
                .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.Postcode))
                .ForMember(d => d.IsActive, opts => opts.MapFrom(s => true))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.PerformedBy))
                .ForAllOtherMembers(d => d.Ignore());
        }
    }
}