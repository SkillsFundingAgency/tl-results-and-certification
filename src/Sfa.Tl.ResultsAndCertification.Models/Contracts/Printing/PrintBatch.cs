namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class PrintBatch
    {
        public int BatchNumber { get; set; }
        public string BatchDate { get; set; }
        public int PostalContactCount { get; set; }
        public int TotalCertificateCount { get; set; }
    }
}
