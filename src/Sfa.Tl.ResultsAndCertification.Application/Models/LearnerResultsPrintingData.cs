using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Models
{
    public class LearnerResultsPrintingData
    {
        public TlProvider TlProvider { get; set; }
        public IList<OverallResult> OverallResults { get; set; }
    }
}
