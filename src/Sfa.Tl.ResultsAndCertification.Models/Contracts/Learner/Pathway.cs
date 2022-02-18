using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner
{
    public class Pathway
    {
        public Pathway()
        {
            PathwayAssessments = new List<Assessment>();
            Specialisms = new List<Specialism>();
            IndustryPlacements = new List<IndustryPlacement>();
        }
        public int Id { get; set; }
        public string LarId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public Provider Provider { get; set; }
        public IEnumerable<Assessment> PathwayAssessments { get; set; }
        public IEnumerable<Specialism> Specialisms { get; set; }
        public IEnumerable<IndustryPlacement> IndustryPlacements { get; set; }
    }
}