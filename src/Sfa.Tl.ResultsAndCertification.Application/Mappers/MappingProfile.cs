using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TlAwardingOrganisation, AwardingOrganisation>();
            CreateMap<TqAwardingOrganisation, AwardingOrganisationPathwayStatus>()
                .ForMember(d => d.Route, opts => opts.MapFrom(s => s.TlRoute))
                .ForMember(d => d.Pathway, opts => opts.MapFrom(s => s.TlPathway))
                .ForMember(d => d.AwardingOrganisaton, opts => opts.MapFrom(s => s.TlAwardingOrganisaton));
            CreateMap<TlPathway, Pathway>();
            CreateMap<TlRoute, Route>();
        }
    }
}
