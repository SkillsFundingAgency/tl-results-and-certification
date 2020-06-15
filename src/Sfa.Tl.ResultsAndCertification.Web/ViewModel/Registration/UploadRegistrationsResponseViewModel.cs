using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration
{
    public class UploadRegistrationsResponseViewModel
    {
        public bool IsSuccess { get; set; }
        public Guid BlobUniqueReference { get; set; }
        public long ErrorFileSize { get; set; }
        public StatsViewModel Stats { get; set; }
    }
}
