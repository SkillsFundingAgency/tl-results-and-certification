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
            CreateMap<TqAwardingOrganisation, AwardingOrganisationPathwayStatus>();
            CreateMap<TlPathway, Pathway>();
            CreateMap<TlRoute, Route>();
        }
    }
}
