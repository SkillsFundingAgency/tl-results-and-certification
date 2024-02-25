using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewAddCoreAssessmentRequest: ReviewChangeRequest
    {
        public AddCoreAssessmentDetails AddCoreAssessmentDetails { get; set; }
        public override ChangeType ChangeType { get; set; } = ChangeType.AddAssessment;
    }
}
