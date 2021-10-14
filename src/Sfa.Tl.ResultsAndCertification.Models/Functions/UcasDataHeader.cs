using CsvHelper.Configuration.Attributes;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataHeader : UcasBaseData
    {
        [Index(3)]
        public char UcasDataType { get; set; }
        [Index(4)]
        public string ExamMonth { get; set; }
        [Index(5)]
        public string ExamYear { get; set; }
        [Index(6)]
        public string DateCreated { get; set; }
    }
}