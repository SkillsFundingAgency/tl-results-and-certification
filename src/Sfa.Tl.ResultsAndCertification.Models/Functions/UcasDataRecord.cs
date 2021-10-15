using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataRecord
    {
        public char UcasRecordType { get; set; }
        public int SendingOrganisation { get; set; }
        public int ReceivingOrganisation { get; set; }
        public string CentreNumber { get; set; }
        public string CandidateNumber { get; set; }
        public string CandidateName { get; set; }
        public char Sex { get; set; }
        public string CandidateDateofBirth { get; set; }
        public IEnumerable<UcasDataComponent> UcasDataComponents { get; set; }
        public string UcaDataComponentRecord
        {
            get
            {
                var result = new List<string>();
                UcasDataComponents.ToList().ForEach(r =>
                {
                    var data = $"_|{r.SubjectCode}|{r.Grade}|{r.PreviousGrade}";
                    result.Add(data);
                });
                return $"{string.Join("|", result)}|_|";
            }
        }
    }
}