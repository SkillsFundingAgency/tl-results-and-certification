using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class DocumentUploadHistoryDetails
    {
        public int TlAwardingOrganisationId { get; set; }
        public long AoUkprn { get; set; }
        public Guid BlobUniqueReference { get; set; }
        public string BlobFileName { get; set; }
        public int DocumentType { get; set; }
        public int FileType { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
    }
}
