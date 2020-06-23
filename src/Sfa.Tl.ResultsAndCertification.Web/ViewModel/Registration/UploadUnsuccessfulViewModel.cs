using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration
{
    public class UploadUnsuccessfulViewModel
    {
        public Guid BlobUniqueReference { get; set; }
        public string FileType { get; set; }
        public double FileSize {get;set;}
    }
}
