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
               .ForMember(d => d.AoName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name))
               .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
               .ForMember(d => d.CurrentCoreGrade, opts => opts.MapFrom(s => GetCurrentCoreGrade(s)))
               .ForMember(d => d.RommOpenedTimeStamp, opts => opts.MapFrom(s => GetRommOpenedTimeStamp(s)))
               .ForMember(d => d.RommGrade, opts => opts.MapFrom(s => GetRommGrade(s)))
               .ForMember(d => d.AppealOpenedTimeStamp, opts => opts.MapFrom(s => GetAppealOpenedTimeStamp(s)))
               .ForMember(d => d.AppealGrade, opts => opts.MapFrom(s => GetAppealGrade(s)));
        }

        private string GetCurrentCoreGrade(TqRegistrationPathway tqRegistrationPathway)
        {
            string coreGrade = string.Empty;

            var coreGradeResults = tqRegistrationPathway.TqPathwayAssessments
                                        .SelectMany(p => p.TqPathwayResults)
                                        .Where(p => !p.PrsStatus.HasValue);

            if (!coreGradeResults.IsNullOrEmpty())
            {
                int coreGradeResultId = coreGradeResults.Max(p => p.Id);
                coreGrade = coreGradeResults.Single(p => p.Id == coreGradeResultId).TlLookup.Value;
            }

            return coreGrade;
        }

        private DateTime? GetRommOpenedTimeStamp(TqRegistrationPathway tqRegistrationPathway)
        {
            return GetOpenedTimeStampByPrsStatus(tqRegistrationPathway, PrsStatus.UnderReview, PrsStatus.Reviewed);
        }

        private DateTime? GetAppealOpenedTimeStamp(TqRegistrationPathway tqRegistrationPathway)
        {
            return GetOpenedTimeStampByPrsStatus(tqRegistrationPathway, PrsStatus.BeingAppealed, PrsStatus.Final);
        }

        private DateTime? GetOpenedTimeStampByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus prsStatus, params PrsStatus[] prsStatusParams)
        {
            DateTime? openedTimeStamp = null;

            IReadOnlyList<TqPathwayResult> results = GetPathwayResultsByPrsStatus(tqRegistrationPathway, prsStatus, prsStatusParams);

            if (!results.IsNullOrEmpty())
            {
                int appealOpenedResultId = results.Min(p => p.Id);
                openedTimeStamp = results.Single(p => p.Id == appealOpenedResultId).CreatedOn;
            }

            return openedTimeStamp;
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
            string rommGrade = string.Empty;

            IReadOnlyList<TqPathwayResult> results = GetPathwayResultsByPrsStatus(tqRegistrationPathway, prsStatus);

            if (!results.IsNullOrEmpty())
            {
                rommGrade = results.Single().TlLookup.Value;
            }

            return rommGrade;
        }

        private IReadOnlyList<TqPathwayResult> GetPathwayResultsByPrsStatus(TqRegistrationPathway tqRegistrationPathway, PrsStatus prsStatus, params PrsStatus[] prsStatusParams)
        {
            var prsStatuses = new List<PrsStatus> { prsStatus };

            if (!prsStatusParams.IsNullOrEmpty())
            {
                prsStatuses.AddRange(prsStatusParams);
            }

            return tqRegistrationPathway.TqPathwayAssessments
                    .SelectMany(p => p.TqPathwayResults)
                    .Where(p => p.PrsStatus.HasValue && prsStatuses.Contains(p.PrsStatus.Value))
                    .ToList();
        }
    }
}