using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class PrintData
    {
        public PostalContact PostalContact { get; set; }
        public List<Certificate> Certificates { get; set; }
    }
}
