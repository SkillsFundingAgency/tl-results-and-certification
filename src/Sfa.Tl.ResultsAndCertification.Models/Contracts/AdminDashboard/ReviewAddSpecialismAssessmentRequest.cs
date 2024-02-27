using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewAddSpecialismAssessmentRequest:ReviewChangeRequest
    {
        public AddSpecialismDetails AddSpecialismDetails { get; set; }
        public override ChangeType ChangeType => ChangeType.AssessmentEntryAdd;
        public int SpecialismId { get; set; }
    }
}
