using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class SearchLearnerRequest
    {
        public IList<int> AcademicYear { get; set; }
        public long Ukprn { get; set; }
    }
}
