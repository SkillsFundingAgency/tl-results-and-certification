namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataHeader : UcasBaseData
    {
        public char UcasDataType { get; set; }
        public string ExamMonth { get; set; }
        public string ExamYear { get; set; }
        public string DateCreated { get; set; }
    }
}