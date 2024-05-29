using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog
{
    public class AdminChangeLogPathway
    {
        public AdminChangeLogPathway()
        {
            PathwayAssessments = new List<AdminChangeLogAssessment>();
            Specialisms = new List<AdminChangeLogSpecialism>();
        }
        public int Id { get; set; }
        public string LarId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public Provider Provider { get; set; }
        public IEnumerable<AdminChangeLogAssessment> PathwayAssessments { get; set; }
        public IEnumerable<AdminChangeLogSpecialism> Specialisms { get; set; }
    }
}