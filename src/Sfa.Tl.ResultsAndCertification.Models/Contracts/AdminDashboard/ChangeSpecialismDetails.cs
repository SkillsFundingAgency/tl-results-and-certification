using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ChangeSpecialismDetails
    {
        public int SpecialismAssessmentId { get; set; }

        public string SelectedGradeFrom { get; set; }

        public string SelectedGradeTo { get; set; }
    }
}
