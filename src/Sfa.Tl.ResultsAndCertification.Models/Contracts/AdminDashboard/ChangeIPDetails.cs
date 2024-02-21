using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ChangeIPDetails
    {
        public IndustryPlacementStatus IndustryPlacementStatusFrom { get; set; }

        public int? HoursSpentOnPlacementFrom { get; set; }

        public List<int?> SpecialConsiderationReasonsFrom { get; set; }

        public IndustryPlacementStatus IndustryPlacementStatusTo { get; set; }

        public int? HoursSpentOnPlacementTo { get; set; }

        public List<int?> SpecialConsiderationReasonsTo { get; set; }

    }
}
