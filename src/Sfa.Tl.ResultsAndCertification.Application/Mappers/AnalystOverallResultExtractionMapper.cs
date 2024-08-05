using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Action;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AnalystOverallResultExtractionMapper : Profile
    {
        public AnalystOverallResultExtractionMapper()
        {
            CreateMap<TqRegistrationPathway, AnalystOverallResultExtractionData>()
                .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.TqProvider.TlProvider.UkPrn))
                .ForMember(d => d.ProviderName, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TlProvider.Name))
                .ForMember(d => d.LastName, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.FirstName, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => DateOnly.FromDateTime(s.TqRegistrationProfile.DateofBirth)))
                .ForMember(d => d.Gender, opts => opts.MapFrom(s => s.TqRegistrationProfile.Gender))
                .ForMember(d => d.TlevelTitle, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle))
                .ForMember(d => d.StartYear, opts => opts.ConvertUsing(new AcademicYearConverter(), s => s.AcademicYear))
                .ForMember(d => d.CoreComponent, opts => opts.ConvertUsing(new DoubleQuotedStringConverter(), s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name))
                .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
                .ForMember(d => d.CoreResult, opts => opts.ConvertUsing(new PathwayResultStringConverter(), p => p.TqPathwayAssessments))
                .ForMember(d => d.OccupationalSpecialism, opts => opts.ConvertUsing(new SpecialismNameConverter(), p => p.TqRegistrationSpecialisms.Where(w => w.IsOptedin && w.EndDate == null)))
                .ForMember(d => d.SpecialismCode, opts => opts.ConvertUsing(new SpecialismCodeConverter(), p => p.TqRegistrationSpecialisms.Where(w => w.IsOptedin && w.EndDate == null)))
                .ForMember(d => d.SpecialismResult, opts => opts.MapFrom(s => GetSpecialismResultAwarded(s)))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.ConvertUsing(new IndustryPlacementStatusStringConverter(), p => p.IndustryPlacements))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => GetResultAwarded(s)))
                .AfterMap<AnalystOverallResultExtractionEmptyToNullTextAction>();
        }

        private static string GetResultAwarded(TqRegistrationPathway registrationPathway)
        {
            OverallResult overallResult = GetOverallResult(registrationPathway);
            return overallResult != null ? overallResult.ResultAwarded : string.Empty;
        }

        private static string GetSpecialismResultAwarded(TqRegistrationPathway registrationPathway)
        {
            OverallResult overallResult = GetOverallResult(registrationPathway);

            if (overallResult == null)
                return string.Empty;

            if (!string.IsNullOrEmpty(overallResult.SpecialismResultAwarded))
                return overallResult.SpecialismResultAwarded;

            OverallResultDetail overallResultDetails = JsonConvert.DeserializeObject<OverallResultDetail>(overallResult.Details);
            return overallResultDetails.SpecialismDetails.FirstOrDefault()?.SpecialismResult;
        }

        private static OverallResult GetOverallResult(TqRegistrationPathway registrationPathway)
        {
            if (registrationPathway.OverallResults.IsNullOrEmpty())
                return null;

            return registrationPathway.OverallResults.OrderByDescending(r => r.Id).First();
        }
    }
}