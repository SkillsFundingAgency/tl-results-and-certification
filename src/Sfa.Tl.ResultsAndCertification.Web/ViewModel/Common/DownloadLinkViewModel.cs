using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common
{
    public class DownloadLinkViewModel
    {
        public Guid BlobUniqueReference { get; set; }
        public string FileType { get; set; }
        public double FileSize { get; set; }
    }
}
