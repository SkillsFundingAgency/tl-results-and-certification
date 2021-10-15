using CsvHelper.Configuration.Attributes;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasDataTrailer : UcasBaseData
    {
        [Index(3)]
        public int Count { get; set; }
        [Index(5)]
        public string ExamDate { get; set; }
    }
}