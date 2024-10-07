using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class AdminAssessmentResultHelper
    {
        public static AdminAssessmentResultStatus GetAdminAssessmentResultStatus(this Assessment assessment, DateTime today)
        {
            if (assessment == null)
            {
                return AdminAssessmentResultStatus.NotSpecified;
            }

            Result result = assessment.Result;

            bool hasGrade = result != null && !string.IsNullOrWhiteSpace(result.Grade);
            if (!hasGrade)
            {
                return AdminAssessmentResultStatus.WithoutGrade;
            }

            PrsStatus? prsStatus = result.PrsStatus;

            bool isOpenRommAllowed = (!prsStatus.HasValue || prsStatus == PrsStatus.NotSpecified) && CommonHelper.IsAppealsAllowed(assessment.AppealEndDate, today);
            if (isOpenRommAllowed)
            {
                return AdminAssessmentResultStatus.OpenRommAllowed;
            }

            bool isAddRommOutcomeAllowed = prsStatus == PrsStatus.UnderReview;
            if (isAddRommOutcomeAllowed)
            {
                return AdminAssessmentResultStatus.AddRommOutcomeAllowed;
            }

            bool isOpenAppealAllowed = prsStatus == PrsStatus.Reviewed && CommonHelper.IsAppealsAllowed(assessment.AppealEndDate, today);
            if (isOpenAppealAllowed)
            {
                return AdminAssessmentResultStatus.OpenAppealAllowed;
            }

            bool isAddAppealOutcomeAllowed = prsStatus == PrsStatus.BeingAppealed;
            if (isAddAppealOutcomeAllowed)
            {
                return AdminAssessmentResultStatus.AddAppealOutcomeAllowed;
            };

            bool isFinal = prsStatus == PrsStatus.Final;
            if (isFinal)
            {
                return AdminAssessmentResultStatus.Final;
            }

            return AdminAssessmentResultStatus.NotSpecified;
        }
    }
}