using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasData
    {
        public UcasDataHeader Header { get; set; }
        public IEnumerable<UcasDataRecord> UcasDataRecords { get; set; }
        public UcasDataTrailer Trailer { get; set; }
    }

    public class UcasBaseData
    {
        public char UcasRecordType { get; set; }
        public int SendingOrganisation { get; set; }
        public int ReceivingOrganisation { get; set; }
    }

    public class UcasDataHeader : UcasBaseData
    {
        public char UcasDataType { get; set; }
        public string ExamMonth { get; set; }
        public string ExamYear { get; set; }
        public string DateCreated { get; set; }
    }

    public enum UcasRecordType
    {
        Header = 'H', 
        Subject = 'S', 
        Trailer = 'T'
    }

    public enum UcasDataType
    {
        Entries = 'E',
        Results = 'R',
        Amendments = 'A'
    }

    public enum Gender
    {
        Male = 'M',
        Female = 'F'
    }

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

    public class UcasDataTrailer : UcasBaseData
    {
        public int Count { get; set; }
        public string ExamDate { get; set; }
    }
}