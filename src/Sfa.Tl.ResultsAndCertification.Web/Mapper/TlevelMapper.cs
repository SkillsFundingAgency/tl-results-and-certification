﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class TlevelMapper : Profile
    {
        public TlevelMapper()
        {
            CreateMap<AwardingOrganisationPathwayStatus, YourTlevelsViewModel>()
                .ForMember(d => d.PathId, opts => opts.MapFrom(s => s.PathwayId))
                .ForMember(d => d.TLevelDescription, opts => opts.MapFrom(s => $"{s.RouteName}: {s.PathwayName}"))
                .ForMember(d => d.StatusId, opts => opts.MapFrom(s => s.StatusId))
                .ForMember(d => d.PageTitle, opts => opts.MapFrom(s => "Your T Levels"));

            CreateMap<TlevelPathwayDetails, TLevelDetailsViewModel>()
               .ForMember(d => d.PageTitle, opts => opts.MapFrom(s => "T Level details"))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.PathwayId))
               .ForMember(d => d.ShowSomethingIsNotRight, opts => opts.MapFrom(s => s.PathwayStatusId == (int)TlevelReviewStatus.Confirmed))
               .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.RouteName))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.Specialisms));
        }
    }
}
