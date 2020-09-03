using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class RegistrationMapper : Profile
    {
        public RegistrationMapper()
        {
            CreateMap<TqRegistrationProfile, RegistrationProfile>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.Id))
                .ReverseMap();
        }
    }
}
