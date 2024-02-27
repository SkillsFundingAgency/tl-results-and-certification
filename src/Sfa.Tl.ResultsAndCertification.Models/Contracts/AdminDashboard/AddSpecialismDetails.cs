using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AddSpecialismDetails
    {
        public string SpecialismAssessmentFrom { get;set; }
        public string SpecialismAssessmentTo { get; set; }
        public int AssessmentSeriesId { get; set; }
    }
}
