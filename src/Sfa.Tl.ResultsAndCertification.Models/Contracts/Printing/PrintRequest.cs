using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class PrintRequest
    {
        public PrintBatch Batch { get; set; }
        public List<PrintData> PrintData { get; set; }
    }
}
