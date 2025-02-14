using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Web.FileResult
{
    public class CsvFileStreamResult : FileStreamResult
    {
        public CsvFileStreamResult(Stream fileStream, string fileDownloadName)
            : base(fileStream, "text/csv")
        {
            FileDownloadName = fileDownloadName;
        }
    }
}