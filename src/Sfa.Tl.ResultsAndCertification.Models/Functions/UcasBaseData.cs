using CsvHelper.Configuration.Attributes;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasBaseData
    {
        [Index(0)]
        public char UcasRecordType { get; set; }
        [Index(1)]
        public int SendingOrganisation { get; set; }
        [Index(2)]
        public int ReceivingOrganisation { get; set; }
    }
}