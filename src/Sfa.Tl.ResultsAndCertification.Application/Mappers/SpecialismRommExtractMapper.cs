using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class SpecialismRommExtractMapper : Profile
    {
        public SpecialismRommExtractMapper()
        {
            CreateMap<TqRegistrationSpecialism, SpecialRommExtractionData>()
               .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
               .ForMember(d => d.StudentStartYear, opts => opts.MapFrom(s => s.TqRegistrationPathway.AcademicYear))
               .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => GetAssessmentSeriesYear(s.TqRegistrationPathway.TqRegistrationSpecialisms.SelectMany(p => p.TqSpecialismAssessments))))
               .ForMember(d => d.AoName, opts => opts.MapFrom(s => s.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name))
               .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => GetSpecialismCode(s)))
               .ForMember(d => d.CurrentSpecialismGrade, opts => opts.MapFrom(s => GetCurrentSpecialismGrade(s.TqSpecialismAssessments)))
               .ForMember(d => d.RommOpenedTimeStamp, opts => opts.MapFrom(s => GetRommOpenedTimeStamp(s.TqSpecialismAssessments)))
               .ForMember(d => d.RommGrade, opts => opts.MapFrom(s => GetRommGrade(s.TqSpecialismAssessments)))
               .ForMember(d => d.AppealOpenedTimeStamp, opts => opts.MapFrom(s => GetAppealOpenedTimeStamp(s.TqSpecialismAssessments)))
               .ForMember(d => d.AppealGrade, opts => opts.MapFrom(s => GetAppealGrade(s.TqSpecialismAssessments)));
        }

        private string GetSpecialismCode(TqRegistrationSpecialism tqRegistrationSpecialism) => tqRegistrationSpecialism.TlSpecialism.LarId.ToString();

        private string GetCurrentSpecialismGrade(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments)
        {
            var value = tqSpecialismAssessments?
                .FirstOrDefault().TqSpecialismResults
                .FirstOrDefault()?.TlLookup.Value;

            return value.IsNullOrEmpty() ? string.Empty : value;
        }

        private string GetAssessmentSeriesYear(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments)
        {
            return tqSpecialismAssessments.IsNullOrEmpty()
                ? string.Empty
                : tqSpecialismAssessments.First().AssessmentSeries.Name;
        }

        private DateTime? GetRommOpenedTimeStamp(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments)
        {
            return GetOpenedTimeStampByPrsStatus(tqSpecialismAssessments, new[] { PrsStatus.UnderReview, PrsStatus.Reviewed });
        }

        private DateTime? GetAppealOpenedTimeStamp(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments)
        {
            return GetOpenedTimeStampByPrsStatus(tqSpecialismAssessments, new[] { PrsStatus.BeingAppealed, PrsStatus.Final });
        }

        private DateTime? GetOpenedTimeStampByPrsStatus(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments, PrsStatus[] prsStatuses)
        {
            TqSpecialismResult[] results = GetPathwayResultsByPrsStatus(tqSpecialismAssessments, prsStatuses);

            return results.IsNullOrEmpty()
                ? null
                : results.MinBy(p => p.Id).CreatedOn;
        }

        private string GetRommGrade(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments)
        {
            return GetRommGradeByPrsStatus(tqSpecialismAssessments, PrsStatus.Reviewed);
        }

        private string GetAppealGrade(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments)
        {
            return GetRommGradeByPrsStatus(tqSpecialismAssessments, PrsStatus.Final);
        }

        private string GetRommGradeByPrsStatus(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments, PrsStatus prsStatus)
        {
            TqSpecialismResult[] results = GetPathwayResultsByPrsStatus(tqSpecialismAssessments, prsStatus);

            return results.IsNullOrEmpty()
                ? string.Empty
                : results[0].TlLookup.Value;
        }

        private TqSpecialismResult[] GetPathwayResultsByPrsStatus(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments, PrsStatus prsStatus)
        {
            return GetPathwayResultsByPrsStatus(tqSpecialismAssessments, new[] { prsStatus });
        }

        private TqSpecialismResult[] GetPathwayResultsByPrsStatus(IEnumerable<TqSpecialismAssessment> tqSpecialismAssessments, PrsStatus[] prsStatuses)
        {
            return tqSpecialismAssessments
                .SelectMany(p => p.TqSpecialismResults)
                    .Where(p => p.PrsStatus.HasValue && prsStatuses.Contains(p.PrsStatus.Value))
                    .ToArray();
        }
    }
}