using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Helpers
{
    public static class CommonHelper
    {
        public static int GetStartYearOffset(int regAcademicYear, int tlevelStartYear, ComponentType componentType)
        {
            var isTlevelStartYearSameAsAcademicYear = regAcademicYear == tlevelStartYear;
            var startYearOffset = componentType == ComponentType.Specialism 
                                ? (isTlevelStartYearSameAsAcademicYear ? Constants.SpecialismAssessmentStartInYears : 0) 
                                : Constants.CoreAssessmentStartInYears;
            return startYearOffset;
        }

        public static bool IsValidNextAssessmentSeries(string assessmentEntryName, int regAcademicYear, int tlevelStartYear, ComponentType componentType, IList<AssessmentSeries> dbAssessmentSeries)
        {
            var currentDate = DateTime.UtcNow.Date;

            var startYearOffset = GetStartYearOffset(regAcademicYear, tlevelStartYear, componentType);

            var isValidNextAssessmentSeries = dbAssessmentSeries.Any(s => s.ComponentType == componentType &&
                                                                     s.Name.Equals(assessmentEntryName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                     currentDate >= s.StartDate &&
                                                                     currentDate <= s.EndDate &&
                                                                     s.Year > regAcademicYear + startYearOffset &&
                                                                     s.Year <= regAcademicYear + Constants.AssessmentEndInYears);

            return isValidNextAssessmentSeries;
        }        

        public static IList<AssessmentSeries> GetValidAssessmentSeries(IList<AssessmentSeries> assessmentSeries, TqRegistrationPathway tqRegistrationPathway, ComponentType componentType)
        {
            var currentDate = DateTime.UtcNow.Date;
            var startYearOffset = GetStartYearOffset(tqRegistrationPathway.AcademicYear, tqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.StartYear, componentType);
            var series = assessmentSeries?.Where(s => s.ComponentType == componentType &&
                                                 s.Year > tqRegistrationPathway.AcademicYear + startYearOffset &&
                                                 s.Year <= tqRegistrationPathway.AcademicYear + Constants.AssessmentEndInYears &&
                                                 currentDate >= s.StartDate && currentDate <= s.EndDate)
                                         ?.OrderBy(a => a.Id)?.ToList();

            return series;
        }
    }
}