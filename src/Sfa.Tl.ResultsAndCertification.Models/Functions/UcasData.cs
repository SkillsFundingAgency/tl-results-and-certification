using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class UcasData
    {
        public UcasDataHeader Header { get; set; }
        public IEnumerable<UcasDataRecord> UcasDataRecords { get; set; }
        public UcasDataTrailer Trailer { get; set; }
    }
}