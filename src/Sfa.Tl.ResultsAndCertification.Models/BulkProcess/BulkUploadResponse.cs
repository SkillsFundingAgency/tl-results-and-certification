using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.BulkProcess
{
    public class BulkUploadResponse
    {
        public BulkUploadResponse()
        {
            BulkUploadErrors = new List<BulkUploadError>();
        }

        public bool IsSuccess { get; set; }
        public BulkUploadStats BulkUploadStats { get; set; }
        public List<BulkUploadError> BulkUploadErrors { get; set; }

        public bool HasAnyErrors { get { return BulkUploadErrors.Any(); } }
    }
}
