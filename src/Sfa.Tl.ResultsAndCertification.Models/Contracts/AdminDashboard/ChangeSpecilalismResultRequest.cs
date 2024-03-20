using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ChangeSpecialismResultRequest : ReviewChangeRequest
    {
        public ChangeSpecialismDetails ChangeSpecialismDetails { get; set; }
        public int SelectedGradeId { get; set; }
        public int SpecialismResultId { get; set; }
        public override ChangeType ChangeType => ChangeType.ChangePathwayResult;
    }
}

