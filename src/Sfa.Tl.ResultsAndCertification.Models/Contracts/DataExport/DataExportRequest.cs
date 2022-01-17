using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport
{
    public class DataExportRequest
    {
        // TODO: I don't thnink we need params as an object, can we remove??
        public long AoUkprn { get; set; }
        public DataExportType RequestType { get; set; }
        public string RequestedBy { get; set; }
    }
}
