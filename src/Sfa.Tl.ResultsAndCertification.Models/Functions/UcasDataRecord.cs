using System.Collections.Generic;

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
        public IEnumerable<UcasDataResult> UcasDataResults { get; set; }
    }

    public class UcasDataResult
    {
        public string SubjectCode { get; set; }
        public string Grade { get; set; }
        public string PreviousGrade { get; set; }
    }
}