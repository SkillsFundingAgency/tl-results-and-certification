using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AvailableAssessmentSeries
    {
        public int ProfileId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public string AssessmentSeriesName { get; set; }
    }
}
