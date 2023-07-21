﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.AnalystResultsExtraction;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AnalystResultExtractionMapper : Profile
    {
        public AnalystResultExtractionMapper()
        {
            CreateMap<TqRegistrationPathway, AnalystOverallResultExtractionData>()
                .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => DateOnly.FromDateTime(s.TqRegistrationProfile.DateofBirth)))
                .ForMember(d => d.Gender, opts => opts.MapFrom(s => s.TqRegistrationProfile.Gender))
                .ForMember(d => d.TlevelTitle, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.StartYear))
                .ForMember(d => d.CoreComponent, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.CoreResult, opts => opts.ConvertUsing(new PathwayResultStringConverter(), p => p.TqPathwayAssessments))
                .ForMember(d => d.OccupationalSpecialism, opts => opts.ConvertUsing(new SpecialismNameConverter(), p => p.TqRegistrationSpecialisms))
                .ForMember(d => d.SpecialismCode, opts => opts.ConvertUsing(new SpecialismCodeConverter(), p => p.TqRegistrationSpecialisms))
                .ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => GetSpecialismResult(s)))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.ConvertUsing(new IndustryPlacementStatusStringConverter(), p => p.IndustryPlacements))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => GetOverallResult(s)));
        }

        private string GetSpecialismResult(TqRegistrationPathway registrationPathway)
        {
            return GetOverallResultProperty(registrationPathway, p => p.SpecialismResultAwarded);
        }

        private string GetOverallResult(TqRegistrationPathway registrationPathway)
        {
            return GetOverallResultProperty(registrationPathway, p => p.ResultAwarded);
        }

        private string GetOverallResultProperty(TqRegistrationPathway registrationPathway, Func<OverallResult, string> getPropertyValue)
        {
            if (registrationPathway.OverallResults.IsNullOrEmpty())
                return string.Empty;

            OverallResult overallResult = registrationPathway.OverallResults.OrderByDescending(r => r.Id).First();

            return overallResult != null ? getPropertyValue(overallResult) : string.Empty;
        }
    }
}