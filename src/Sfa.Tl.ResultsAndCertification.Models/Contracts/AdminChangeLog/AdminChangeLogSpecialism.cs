using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog
{
    public class AdminChangeLogSpecialism
    {
        public AdminChangeLogSpecialism()
        {
            Assessments = new List<AdminChangeLogAssessment>();
            TlSpecialismCombinations = new List<KeyValuePair<int, string>>();
        }

        public int Id { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismCombinations { get; set; }
        public IEnumerable<AdminChangeLogAssessment> Assessments { get; set; }
    }
}
