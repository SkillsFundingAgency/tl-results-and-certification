using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AwardingOrganisationMappingProfile : Profile
    {
        public AwardingOrganisationMappingProfile()
        {
            CreateMap<TqAwardingOrganisation, AwardingOrganisationPathwayStatus>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.TlPathway.Id))
                .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.TlRoute.Name))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TlPathway.Name))
                .ForMember(d => d.StatusId, opts => opts.MapFrom(s => s.ReviewStatus));
        }
    }
}
