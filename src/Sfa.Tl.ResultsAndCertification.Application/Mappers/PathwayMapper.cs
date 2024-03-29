﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class PathwayMapper : Profile
    {
        public PathwayMapper()
        {
            CreateMap<TlPathway, TlevelPathwayDetails>()
                .ForMember(d => d.TqAwardingOrganisationId, opts => opts.MapFrom(s => s.TqAwardingOrganisations.FirstOrDefault(x => x.TlPathwayId == s.Id).Id))
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.PathwayCode, opts => opts.MapFrom(s => s.LarId))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.RouteId, opts => opts.MapFrom(s => s.TlRouteId))
                .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.TlRoute.Name))
                .ForMember(d => d.PathwayStatusId, opts => opts.MapFrom(s => s.TqAwardingOrganisations.FirstOrDefault(x => x.TlPathwayId == s.Id).ReviewStatus))
                .ForMember(d => d.Specialisms, opts => opts.MapFrom(s => s.TlSpecialisms))
                .ForMember(d => d.VerifiedBy, opts => opts.MapFrom(s => s.TqAwardingOrganisations.FirstOrDefault(x => x.TlPathwayId == s.Id).ModifiedBy))
                .ForMember(d => d.VerifiedOn, opts => opts.MapFrom(s => s.TqAwardingOrganisations.FirstOrDefault(x => x.TlPathwayId == s.Id).ModifiedOn));

            CreateMap<TlSpecialism, SpecialismDetails>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Code, opts => opts.MapFrom(s => s.LarId))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name));
        }
    }
}
