using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class BulkRegistrationResponse
    {
        public bool IsSuccess { get; set; }
        public Guid BlobUniqueReference { get; set; }
        public double ErrorFileSize { get; set; }
        public BulkUploadStats Stats { get; set; }
    }
}
