using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadCoreResultsDataLinkGet
{
    public class AdminDownloadCoreResultsDataLinkGetBaseTest : AdminAwardingOrganisationControllerBaseTest
    {
        protected const long Ukprn = 10009696;
        protected const string FileId = "8d0ed880-6cde-4853-85d8-5f6057ea5d5c";

        protected readonly Guid FileGuid = new(FileId);
        protected readonly Stream Stream = new MemoryStream(new byte[] { 1, 2, 3 });

        protected IActionResult Result;

        public override async Task When()
        {
            Result = await Controller.AdminDownloadCoreResultsDataLinkAsync(Ukprn, FileId);
        }
    }
}