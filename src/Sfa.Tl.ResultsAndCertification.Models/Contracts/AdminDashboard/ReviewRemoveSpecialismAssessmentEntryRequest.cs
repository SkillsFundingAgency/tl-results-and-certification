﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewRemoveSpecialismAssessmentEntryRequest : ReviewChangeRequest
    {
        public int AssessmentId { get; set; }

        public DetailsSpecialismAssessmentRemove ChangeSpecialismAssessmentDetails { get; set; }

        public ComponentType ComponentType { get; set; }

        public override ChangeType ChangeType => ChangeType.RemoveSpecialismAssessment;
    }
}
