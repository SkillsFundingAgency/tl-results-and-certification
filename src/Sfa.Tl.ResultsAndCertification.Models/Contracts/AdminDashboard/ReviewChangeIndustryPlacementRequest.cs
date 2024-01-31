using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewChangeIndustryPlacementRequest :ReviewChangeRequest
    {
        public ChangeIPDetails ChangeIPDetails { get; set; }
    }
}
