using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class AdminAssessmentResultHelper
    {
        public static AdminAssessmentResultStatus GetAdminAssessmentResultStatus(this Assessment assessment, DateTime today)
        {
            if (assessment?.Result == null)
            {
                return AdminAssessmentResultStatus.NotSpecified;
            }

            Result result = assessment.Result;
            
            bool hasGrade = !string.IsNullOrWhiteSpace(result.Grade);
            if (!hasGrade)
            {
                return AdminAssessmentResultStatus.WithoutGrade;
            }

            PrsStatus? prsStatus = result.PrsStatus;
            bool isValidGradeForPrsJourney = CommonHelper.IsValidGradeForPrsJourney(result.GradeCode, assessment.ComponentType);

            bool isOpenRommAllowed = (!prsStatus.HasValue || prsStatus == PrsStatus.NotSpecified) && CommonHelper.IsAppealsAllowed(assessment.AppealEndDate, today) && isValidGradeForPrsJourney;
            if (isOpenRommAllowed)
            {
                return AdminAssessmentResultStatus.OpenRommAllowed;
            }

            bool isAddRommOutcomeAllowed = prsStatus == PrsStatus.UnderReview && isValidGradeForPrsJourney;
            if (isAddRommOutcomeAllowed)
            {
                return AdminAssessmentResultStatus.AddRommOutcomeAllowed;
            }

            bool isOpenAppealAllowed = prsStatus == PrsStatus.Reviewed && CommonHelper.IsAppealsAllowed(assessment.AppealEndDate, today) && isValidGradeForPrsJourney;
            if (isOpenAppealAllowed)
            {
                return AdminAssessmentResultStatus.OpenAppealAllowed;
            }

            bool isAddAppealOutcomeAllowed = prsStatus == PrsStatus.BeingAppealed && isValidGradeForPrsJourney;
            if (isAddAppealOutcomeAllowed)
            {
                return AdminAssessmentResultStatus.AddAppealOutcomeAllowed;
            };

            return AdminAssessmentResultStatus.NotSpecified;
        }
    }
}