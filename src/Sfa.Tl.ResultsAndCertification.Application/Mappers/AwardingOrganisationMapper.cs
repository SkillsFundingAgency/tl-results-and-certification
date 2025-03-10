﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AwardingOrganisationMapper : Profile
    {
        public AwardingOrganisationMapper()
        {
            CreateMap<TqAwardingOrganisation, AwardingOrganisationPathwayStatus>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.TlPathway.Id))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlPathway.TlevelTitle))
                .ForMember(d => d.StatusId, opts => opts.MapFrom(s => s.ReviewStatus));

            CreateMap<VerifyTlevelDetails, TqAwardingOrganisation>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.TqAwardingOrganisationId))
                .ForMember(d => d.ReviewStatus, opts => opts.MapFrom(s => s.PathwayStatusId))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(s => s.ModifiedBy))
                .ForMember(d => d.ModifiedOn, opts => opts.MapFrom<DateTimeResolver<VerifyTlevelDetails, TqAwardingOrganisation>>());

            CreateMap<TlAwardingOrganisation, AwardingOrganisationMetadata>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.UkPrn))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName));
        }
    }
}
