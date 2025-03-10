using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadRommsDataLinkAsyncGet
{
    public class AdminDownloadRommsDataLinkAsyncGetBaseTest : AdminAwardingOrganisationControllerBaseTest
    {
        protected const long Ukprn = 10009696;
        protected const string FileId = "8494672a-0c5a-4d31-96d5-4e42c670ad23";

        protected readonly Guid FileGuid = new(FileId);
        protected readonly Stream Stream = new MemoryStream(new byte[] { 1, 2, 3 });

        protected IActionResult Result;

        public override async Task When()
        {
            Result = await Controller.AdminDownloadRommsDataLinkAsync(Ukprn, FileId);
        }
    }
}
