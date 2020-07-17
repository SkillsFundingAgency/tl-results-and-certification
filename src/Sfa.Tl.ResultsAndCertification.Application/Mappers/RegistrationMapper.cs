using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class RegistrationMapper : Profile
    {
        public RegistrationMapper()
        {
            CreateMap<TlPathway, PathwayDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Code, opts => opts.MapFrom(s => s.LarId))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.TlevelTitle));
        }
    }
}
