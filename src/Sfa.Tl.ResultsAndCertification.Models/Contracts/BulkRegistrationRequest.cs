using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class BulkRegistrationRequest
    {
        public string BlobReferencePath { get; set; }
        public long Ukprn { get; set; }
        public string performedBy { get; set; }


        // Temp properties.
        public virtual Stream FileStream { get; set; }
    }
}
