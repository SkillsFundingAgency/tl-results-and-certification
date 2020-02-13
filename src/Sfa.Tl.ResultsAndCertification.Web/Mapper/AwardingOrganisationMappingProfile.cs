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
                .ForMember(d => d.StatusId, opts => opts.MapFrom(s => s.StatusId))
                .ForMember(d => d.PageTitle, opts => opts.MapFrom(s => "Your T Levels")); // TODO: map this dynamically.

            CreateMap<TlevelPathwayDetails, YourTLevelDetailsViewModel>()
               .ForMember(d => d.PageTitle, opts => opts.MapFrom(s => "T Level details"))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.RouteName))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms));
        }
    }
}
