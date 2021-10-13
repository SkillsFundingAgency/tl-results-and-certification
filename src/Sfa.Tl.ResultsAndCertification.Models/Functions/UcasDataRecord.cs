namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataRecord : UcasBaseData
    {
        public string CentreNumber { get; set; }
        public string CandidateNumber { get; set; }
        public string CandidateName { get; set; }
        public char Sex { get; set; }
        public string CandidateDateofBirth { get; set; }
        public string SubjectCode { get; set; }
        public string Grade { get; set; }
        public string PreviousGrade { get; set; }
    }
}