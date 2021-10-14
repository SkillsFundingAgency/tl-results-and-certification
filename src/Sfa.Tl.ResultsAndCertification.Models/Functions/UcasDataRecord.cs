using CsvHelper.Configuration.Attributes;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataRecord : UcasBaseData
    {
        [Index(3)]
        public string CentreNumber { get; set; }
        [Index(4)]
        public string CandidateNumber { get; set; }
        [Index(5)]
        public string CandidateName { get; set; }
        [Index(6)]
        public char Sex { get; set; }
        [Index(7)]
        public string CandidateDateofBirth { get; set; }
        [Index(8)]
        public string SubjectCode { get; set; }
        [Index(9)]
        public string Grade { get; set; }
        [Index(10)]
        public string PreviousGrade { get; set; }
    }
}