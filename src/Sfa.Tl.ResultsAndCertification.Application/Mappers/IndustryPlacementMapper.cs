﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class IndustryPlacementMapper : Profile
    {
        public IndustryPlacementMapper()
        {
            CreateMap<IpLookup, IpLookupData>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.ShowOption, opts => opts.MapFrom(s => s.ShowOption));
        }
    }
}