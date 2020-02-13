﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class PathwayMappingProfile : Profile
    {
        public PathwayMappingProfile()
        {
            CreateMap<TlPathway, TlevelPathwayDetails>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.RouteId, opts => opts.MapFrom(s => s.TlRouteId))
                .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.TlRoute.Name))
                .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.TlSpecialisms.Select(x => x.Name)));
        }
    }
}
