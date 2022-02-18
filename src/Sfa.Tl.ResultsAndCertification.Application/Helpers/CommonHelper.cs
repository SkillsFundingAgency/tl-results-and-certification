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
        public static int GetStartInYear(int regAcademicYear, int tlevelStartYear, ComponentType componentType)
        {
            var isTlevelStartYearSameAsAcademicYear = regAcademicYear == tlevelStartYear;
            var startInYear = componentType == ComponentType.Specialism ? (isTlevelStartYearSameAsAcademicYear ? Constants.SpecialismAssessmentStartInYears : 0) : Constants.CoreAssessmentStartInYears;
            return startInYear;
        }

        public static bool IsValidNextAssessmentSeries(IList<AssessmentSeries> dbAssessmentSeries, int regAcademicYear, string assessmentEntryName, int tlevelStartYear, ComponentType componentType)
        {
            var currentDate = DateTime.UtcNow.Date;

            var startYearOffset = GetStartInYear(regAcademicYear, tlevelStartYear, componentType);

            var isValidNextAssessmentSeries = dbAssessmentSeries.Any(s => s.ComponentType == componentType &&
                s.Name.Equals(assessmentEntryName, StringComparison.InvariantCultureIgnoreCase) &&
                currentDate >= s.StartDate && currentDate <= s.EndDate &&
                s.Year > regAcademicYear + startYearOffset && s.Year <= regAcademicYear + Constants.AssessmentEndInYears);

            return isValidNextAssessmentSeries;
        }        

        public static IList<AssessmentSeries> GetValidAssessmentSeries(IList<AssessmentSeries> assessmentSeries, TqRegistrationPathway tqRegistrationPathway, ComponentType componentType)
        {
            var currentDate = DateTime.UtcNow.Date;
            var startInYear = GetStartInYear(tqRegistrationPathway.AcademicYear, tqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.StartYear, componentType);
            var series = assessmentSeries?.Where(s => s.ComponentType == componentType && s.Year > tqRegistrationPathway.AcademicYear + startInYear &&
                                        s.Year <= tqRegistrationPathway.AcademicYear + Constants.AssessmentEndInYears &&
                                        currentDate >= s.StartDate && currentDate <= s.EndDate)?.OrderBy(a => a.Id)?.ToList();

            return series;
        }

        public static AssessmentSeries GetNextAvailableAssessmentSeries(IList<AssessmentSeries> assessmentSeries, TqRegistrationPathway tqRegistrationPathway, ComponentType componentType)
        {
            var startInYear = GetStartInYear(tqRegistrationPathway.AcademicYear, tqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.StartYear, componentType);
            var series = assessmentSeries?.OrderBy(a => a.Id)?.FirstOrDefault(s => s.ComponentType == componentType && s.Year > tqRegistrationPathway.AcademicYear + startInYear &&
                                        s.Year <= tqRegistrationPathway.AcademicYear + Constants.AssessmentEndInYears && DateTime.UtcNow.Date <= s.EndDate);
            return series;
        }
    }
}