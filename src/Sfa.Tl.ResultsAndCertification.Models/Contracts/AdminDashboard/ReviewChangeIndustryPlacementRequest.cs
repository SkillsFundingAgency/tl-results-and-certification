using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewChangeIndustryPlacementRequest : ReviewChangeRequest
    {
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }

        public int? HoursSpentOnPlacement { get; set; }

        public List<int> SpecialConsiderationReasons { get; set; }

        public override ChangeType ChangeType => ChangeType.IndustryPlacement;
    }
}