using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class PrintRequestResponse
    {
        public int BatchNumber { get; set; }
        public string Status { get; set; }
        public List<Error> Errors { get; set; }
    }
}
