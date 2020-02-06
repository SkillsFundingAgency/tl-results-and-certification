using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Models;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AwardingOrganisationPathwayStatus, YourTlevelsViewModel>()
                .ForMember(d => d.PathId, opts => opts.MapFrom(s => s.TlPathwayId))
                .ForMember(d => d.RouteId, opts => opts.MapFrom(s => s.TlRouteId))
                .ForMember(d => d.TLevelDescription, opts => opts.MapFrom(s => $"{s.Route.Name}: {s.Pathway.Name}"))
                .ForMember(d => d.TLevelStatus, opts => opts.MapFrom(s => (TlevelReviewStatus)s.ReviewStatus));
        }
    }
}
