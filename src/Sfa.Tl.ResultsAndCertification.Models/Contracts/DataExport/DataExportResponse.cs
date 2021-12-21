using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport
{
    public class DataExportResponse
    {
        public bool IsDataFound { get; set; }
        public Guid BlobUniqueReference { get; set; }
        public double FileSize { get; set; }
        public ComponentType ComponentType { get; set; }
    }
}
