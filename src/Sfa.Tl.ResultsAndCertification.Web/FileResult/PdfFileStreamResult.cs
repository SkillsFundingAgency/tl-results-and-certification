using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Web.FileResult
{
    public class PdfFileStreamResult : FileStreamResult
    {
        public PdfFileStreamResult(Stream fileStream, string fileDownloadName)
            : base(fileStream, "application/pdf")
        {
            FileDownloadName = fileDownloadName;
        }
    }
}