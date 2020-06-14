using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class BulkRegistrationRequest
    {
        public long AoUkprn { get; set; }
        public string BlobFileName { get; set; }
        public FileType FileType { get; set; }
        public DocumentType DocumentType { get; set; }
        public string PerformedBy { get; set; }
    }
}
