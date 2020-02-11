using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Models;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AwardingOrganisationMappingProfile : Profile
    {
        public AwardingOrganisationMappingProfile()
        {
            CreateMap<AwardingOrganisationPathwayStatus, YourTlevelsViewModel>()
                .ForMember(d => d.PathId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.TLevelDescription, opts => opts.MapFrom(s => $"{s.RouteName}: {s.PathwayName}"))
                .ForMember(d => d.TLevelStatus, opts => opts.MapFrom(s => s.TlevelReviewStatus));
        }
    }
}
