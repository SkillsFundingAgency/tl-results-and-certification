using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class CoreRommExtractMapper : Profile
    {
        public CoreRommExtractMapper()
        {
            CreateMap<TqRegistrationPathway, CoreRommExtractData>()
               .ForMember(d => d.UniqueLearnerNumber, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
               .ForMember(d => d.StudentStartYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => GetAssessmentSeriesYear(s.TqPathwayAssessments)))
               .ForMember(d => d.AoName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name))
               .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
               .ForMember(d => d.CurrentCoreGrade, opts => opts.MapFrom(s => GetCurrentCoreGrade(s)))
               .ForMember(d => d.RommOpenedTimeStamp, opts => opts.MapFrom(s => GetRommOpenedTimeStamp(s)))
               .ForMember(d => d.RommGrade, opts => opts.MapFrom(s => GetRommGrade(s)))
               .ForMember(d => d.AppealOpenedTimeStamp, opts => opts.MapFrom(s => GetAppealOpenedTimeStamp(s)))
               .ForMember(d => d.AppealGrade, opts => opts.MapFrom(s => GetAppealGrade(s)));
        }

        private string GetAssessmentSeriesYear(ICollection<TqPathwayAssessment> tqPathwayAssessments)
        {
            return tqPathwayAssessments.IsNullOrEmpty()
                ? string.Empty
                : tqPathwayAssessments.First().AssessmentSeries.Name;
        }

        private string GetCurrentCoreGrade(TqRegistrationPathway tqRegistrationPathway)
        {
            string coreGrade = string.Empty;

            var coreGradeResults = tqRegistrationPathway.TqPathwayAssessments
                                        .SelectMany(p => p.TqPathwayResults)
                                        .Where(p => !p.PrsStatus.HasValue);

            return coreGradeResults.IsNullOrEmpty()
                ? string.Empty
                : coreGradeResults.MaxBy(p => p.Id).TlLookup.Value;
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
            TqPathwayResult[] results = GetPathwayResultsByPrsStatus(tqRegistrationPathway, prsStatuses);

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
            TqPathwayResult[] results = GetPathwayResultsByPrsStatus(tqRegistrationPathway, prsStatus);

            return results.IsNullOrEmpty()
                ? string.Empty
                : results[0].TlLookup.Value;
        }

        private TqPathwayResult[] GetPathwayResultsByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus prsStatus)
        {
            return GetPathwayResultsByPrsStatus(tqRegistrationPathway, new[] { prsStatus });
        }

        private TqPathwayResult[] GetPathwayResultsByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus[] prsStatuses)
        {
            return tqRegistrationPathway.TqPathwayAssessments
                    .SelectMany(p => p.TqPathwayResults)
                    .Where(p => p.PrsStatus.HasValue && prsStatuses.Contains(p.PrsStatus.Value))
                    .ToArray();
        }
    }
}