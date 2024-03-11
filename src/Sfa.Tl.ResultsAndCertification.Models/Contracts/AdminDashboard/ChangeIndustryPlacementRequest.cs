using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ChangeIndustryPlacementRequest
    {
        public string IndustryPlacementStatus { get; set; }

        public int? HoursSpentOnPlacement { get; set; }

        public List<int?> SpecialConsiderationReasons { get; set; }
    }
}
