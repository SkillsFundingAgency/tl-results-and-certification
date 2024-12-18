﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using PrsStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class CommonHelper
    {
        public static string GetResourceMessage(string errorResourceName, Type errorResourceType)
        {
            return (string)errorResourceType?.GetProperty(errorResourceName)?.GetValue(null, null);
        }

        public static bool IsSoaAlreadyRequested(int reRequestAllowedInDays, DateTime? requestedDate)
        {
            return requestedDate.HasValue && DateTime.Today < requestedDate.Value.Date.AddDays(reRequestAllowedInDays);
        }

        public static bool IsDocumentRerequestEligible(int documentRerequestInDays, int? printCertificateId, DateTime? lastPrintRequestedDate)
        {
            return printCertificateId.HasValue && (!lastPrintRequestedDate.HasValue || DateTime.Today > lastPrintRequestedDate.Value.Date.AddDays(documentRerequestInDays));
        }

        public static bool IsRommAllowed(DateTime? rommEndDate)
        {
            return rommEndDate.HasValue && DateTime.Today <= rommEndDate.Value;
        }

        public static bool IsAppealsAllowed(DateTime? appealsEndDate)
        {
            return IsAppealsAllowed(appealsEndDate, DateTime.Today);
        }

        public static bool IsAppealsAllowed(DateTime? appealsEndDate, DateTime today)
        {
            return appealsEndDate.HasValue && today <= appealsEndDate.Value;
        }

        public static string GetPrsStatusDisplayText(PrsStatus? prsStatus, DateTime? rommEndDate, DateTime? appealsEndDate)
        {
            if ((prsStatus == null || prsStatus == PrsStatus.NotSpecified) && rommEndDate.HasValue && IsRommAllowed(rommEndDate) == false)
                return FormatPrsStatusDisplayHtml(Constants.RedTagClassName, PrsStatusContent.Final_Display_Text);

            if (prsStatus == PrsStatus.UnderReview)
                return FormatPrsStatusDisplayHtml(Constants.BlueTagClassName, PrsStatusContent.Under_Review_Display_Text);

            if (prsStatus == PrsStatus.BeingAppealed)
                return FormatPrsStatusDisplayHtml(Constants.PurpleTagClassName, PrsStatusContent.Being_Appealed_Display_Text);

            if (prsStatus == PrsStatus.Final || !IsAppealsAllowed(appealsEndDate))
                return FormatPrsStatusDisplayHtml(Constants.RedTagClassName, PrsStatusContent.Final_Display_Text);

            return string.Empty;
        }

        public static IList<AssessmentSeriesDetails> GetValidAssessmentSeries(IList<AssessmentSeriesDetails> assessmentSeries, int academicYear, int tlevelStartYear, ComponentType componentType, bool includePreviousSeries = false)
        {
            var currentDate = DateTime.UtcNow.Date;
            int startYearOffset = GetStartYearOffset(academicYear, tlevelStartYear, componentType);

            var series = assessmentSeries?.Where(s => s.ComponentType == componentType &&
                                                 s.Year >= academicYear + startYearOffset &&
                                                 s.Year <= academicYear + Constants.AssessmentEndInYears &&
                                                 currentDate >= s.StartDate && currentDate <= s.EndDate)
                                         ?.OrderBy(a => a.Id)?.ToList();

            if (series != null && series.Count > 0 && includePreviousSeries)
            {
                var previousAssessmentDate = series.Select(e => e.StartDate).FirstOrDefault().AddDays(-1);

                var prevSeries = assessmentSeries?.Where(s => s.ComponentType == componentType &&
                                                         s.Year >= academicYear + startYearOffset &&
                                                         s.Year <= academicYear + Constants.AssessmentEndInYears &&
                                                         s.EndDate == previousAssessmentDate).FirstOrDefault();

                if (prevSeries != null)
                {
                    series.Add(prevSeries);
                }
            }

            return series;
        }

        public static AssessmentSeriesDetails GetNextAvailableAssessmentSeries(IList<AssessmentSeriesDetails> assessmentSeries, int academicYear, int tlevelStartYear, ComponentType componentType)
        {
            int startYearOffset = GetStartYearOffset(academicYear, tlevelStartYear, componentType);

            var series = assessmentSeries?.OrderBy(a => a.Id)
                                         ?.FirstOrDefault(s => s.ComponentType == componentType &&
                                                          s.Year >= academicYear + startYearOffset &&
                                                          s.Year <= academicYear + Constants.AssessmentEndInYears &&
                                                          DateTime.UtcNow.Date <= s.EndDate);
            return series;
        }

        public static bool IsValidGradeForPrsJourney(string gradeCode, ComponentType componentType)
        {
            return componentType == ComponentType.Core && !gradeCode.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase) ||
                   componentType == ComponentType.Specialism && !gradeCode.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsValidGradeForChangeResult(string gradeCode, ComponentType componentType)
        {
            return !string.IsNullOrWhiteSpace(gradeCode) &&
                   (componentType == ComponentType.Core && gradeCode.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase) ||
                    componentType == ComponentType.Specialism && gradeCode.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase));
        }

        private static string FormatPrsStatusDisplayHtml(string tagClassName, string statusText) => string.Format(PrsStatusContent.PrsStatus_Display_Html, tagClassName, statusText);

        private static int GetStartYearOffset(int academicYear, int tlevelStartYear, ComponentType componentType)
        {
            var isTlevelStartYearSameAsAcademicYear = academicYear == tlevelStartYear;
            var startYearOffset = componentType == ComponentType.Specialism
                                ? (isTlevelStartYearSameAsAcademicYear ? Constants.SpecialismAssessmentStartInYears : 0)
                                : Constants.CoreAssessmentStartInYears;
            return startYearOffset;
        }
    }
}