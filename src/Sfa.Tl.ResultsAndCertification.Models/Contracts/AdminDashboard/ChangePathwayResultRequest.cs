using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ChangePathwayResultRequest : ReviewChangeRequest
    {
        public ChangePathwayDetails ChangePathwayDetails { get; set; }
        public int SelectedGradeId { get; set; }
        public int PathwayResultId { get; set; }
        public override ChangeType ChangeType => ChangeType.ChangePathwayResult;
    }
}

