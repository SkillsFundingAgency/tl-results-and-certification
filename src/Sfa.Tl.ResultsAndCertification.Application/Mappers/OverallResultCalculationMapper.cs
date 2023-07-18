﻿using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class OverallResultCalculationMapper : Profile
    {
        public OverallResultCalculationMapper()
        {
            CreateMap<OverallResult, DownloadOverallResultsData>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.TqRegistrationPathway.AcademicYear))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => s.ResultAwarded))
                .ForMember(d => d.Details, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<OverallResultDetail>(s.Details)))
                .ForMember(d => d.SpecialismComponent, opts => opts.ConvertUsing<SpecialismNameConverter, TqRegistrationPathway>(s => s.TqRegistrationPathway))
                .ForMember(d => d.SpecialismCode, opts => opts.ConvertUsing<SpecialismCodeConverter, TqRegistrationPathway>(s => s.TqRegistrationPathway))
                .ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => s.SpecialismResultAwarded));
        }
    }
}