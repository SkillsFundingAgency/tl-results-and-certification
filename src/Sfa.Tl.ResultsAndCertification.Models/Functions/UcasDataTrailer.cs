namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataTrailer
    {
        public char UcasRecordType { get; set; }
        public int SendingOrganisation { get; set; }
        public int ReceivingOrganisation { get; set; }
        public int Count { get; set; }
        public string ExamDate { get; set; }
        public string RecordTerminator { get { return string.Empty; } }
    }
}