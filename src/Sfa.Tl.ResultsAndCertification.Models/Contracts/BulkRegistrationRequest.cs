using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class BulkRegistrationRequest
    {
        public long AoUkprn { get; set; }
        public string BlobFileName { get; set; }
        public string PerformedBy { get; set; }
    }
}
