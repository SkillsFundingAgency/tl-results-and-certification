using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class SpecialismRommExtractMapper : Profile
    {
        public SpecialismRommExtractMapper()
        {
            CreateMap<TqRegistrationPathway, SpecialRommExtractionData>()
               .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
               .ForMember(d => d.StudentStartYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => GetAssessmentSeriesYear(s.TqRegistrationSpecialisms.SelectMany(p => p.TqSpecialismAssessments))))
               .ForMember(d => d.AoName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name))
               .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
               .ForMember(d => d.CurrentSpecialismGrade, opts => opts.MapFrom(s => GetCurrentSpecialismGrade(s)))
               .ForMember(d => d.RommOpenedTimeStamp, opts => opts.MapFrom(s => GetRommOpenedTimeStamp(s)))
               .ForMember(d => d.RommGrade, opts => opts.MapFrom(s => GetRommGrade(s)))
               .ForMember(d => d.AppealOpenedTimeStamp, opts => opts.MapFrom(s => GetAppealOpenedTimeStamp(s)))
               .ForMember(d => d.AppealGrade, opts => opts.MapFrom(s => GetAppealGrade(s)));
        }

        private string GetAssessmentSeriesYear(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments)
        {
            return tqSpecialismAssessments.IsNullOrEmpty()
                ? string.Empty
                : tqSpecialismAssessments.First().AssessmentSeries.Name;
        }

        private string GetCurrentSpecialismGrade(TqRegistrationPathway tqRegistrationPathway)
        {
            var specialismGradeResults = tqRegistrationPathway.TqRegistrationSpecialisms
                                        .SelectMany(p => p.TqSpecialismAssessments)
                                        .SelectMany(p => p.TqSpecialismResults)
                                        .Where(p => !p.PrsStatus.HasValue);

            return specialismGradeResults.IsNullOrEmpty()
                ? string.Empty
                : specialismGradeResults.MaxBy(p => p.Id).TlLookup.Value;
        }

        private DateTime? GetRommOpenedTimeStamp(TqRegistrationPathway tqRegistrationPathway)
        {
            return GetOpenedTimeStampByPrsStatus(tqRegistrationPathway, new[] { PrsStatus.UnderReview, PrsStatus.Reviewed });
        }

        private DateTime? GetAppealOpenedTimeStamp(TqRegistrationPathway tqRegistrationPathway)
        {
            return GetOpenedTimeStampByPrsStatus(tqRegistrationPathway, new[] { PrsStatus.BeingAppealed, PrsStatus.Final });
        }

        private DateTime? GetOpenedTimeStampByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus[] prsStatuses)
        {
            TqSpecialismResult[] results = GetPathwayResultsByPrsStatus(tqRegistrationPathway, prsStatuses);

            return results.IsNullOrEmpty()
                ? null
                : results.MinBy(p => p.Id).CreatedOn;
        }

        private string GetRommGrade(TqRegistrationPathway tqRegistrationPathway)
        {
            return GetRommGradeByPrsStatus(tqRegistrationPathway, PrsStatus.Reviewed);
        }

        private string GetAppealGrade(TqRegistrationPathway tqRegistrationPathway)
        {
            return GetRommGradeByPrsStatus(tqRegistrationPathway, PrsStatus.Final);
        }

        private string GetRommGradeByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus prsStatus)
        {
            TqSpecialismResult[] results = GetPathwayResultsByPrsStatus(tqRegistrationPathway, prsStatus);

            return results.IsNullOrEmpty()
                ? string.Empty
                : results[0].TlLookup.Value;
        }

        private TqSpecialismResult[] GetPathwayResultsByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus prsStatus)
        {
            return GetPathwayResultsByPrsStatus(tqRegistrationPathway, new[] { prsStatus });
        }

        private TqSpecialismResult[] GetPathwayResultsByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus[] prsStatuses)
        {
            return tqRegistrationPathway.TqRegistrationSpecialisms
                    .SelectMany(p => p.TqSpecialismAssessments)
                    .SelectMany(p => p.TqSpecialismResults)
                    .Where(p => p.PrsStatus.HasValue && prsStatuses.Contains(p.PrsStatus.Value))
                    .ToArray();
        }
    }
}