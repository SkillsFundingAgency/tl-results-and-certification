using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AddCoreAssessmentDetails
    {
        public string CoreAssessmentFrom { get; set; }
        public string CoreAssessmentTo { get; set; }
        public int AssessmentSeriesId { get; set; }
    }
}
