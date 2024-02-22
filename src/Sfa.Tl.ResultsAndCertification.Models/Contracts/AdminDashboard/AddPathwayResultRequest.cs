using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    internal class AddPathwayResultRequest : ReviewChangeRequest
    {
        public int PathwayAssessmentId { get; set; }

        public override ChangeType ChangeType => ChangeType.AddPathwayResult;
    }
}