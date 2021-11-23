using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner
{
    public class Specialism
    {
        public Specialism()
        {
            Assessments = new List<Assessment>();
        }

        public int Id { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Assessment> Assessments { get; set; }
    }
}
