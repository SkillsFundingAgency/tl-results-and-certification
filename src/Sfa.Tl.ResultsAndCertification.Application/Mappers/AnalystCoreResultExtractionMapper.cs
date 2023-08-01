using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultsExtraction;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AnalystCoreResultExtractionMapper : Profile
    {
        public AnalystCoreResultExtractionMapper()
        {
            CreateMap<TqRegistrationPathway, AnalystCoreResultExtractionData>()
                .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.UkPrn, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => DateOnly.FromDateTime(s.TqRegistrationProfile.DateofBirth)))
                .ForMember(d => d.Gender, opts => opts.MapFrom(s => s.TqRegistrationProfile.Gender))
                .ForMember(d => d.TlevelTitle, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
                .ForMember(d => d.StartYear, opts => opts.ConvertUsing(new AcademicYearConverter(), s => s.AcademicYear))
                .ForMember(d => d.CoreComponent, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.CoreResult, opts => opts.ConvertUsing(new PathwayResultStringConverter(), p => p.TqPathwayAssessments));
        }
    }
}