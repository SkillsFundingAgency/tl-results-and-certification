using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadSpecialismResultsDataLinkGet
{
    public class AdminDownloadSpecialismResultsDataLinkGetBaseTest : AdminAwardingOrganisationControllerBaseTest
    {
        protected const long Ukprn = 10009696;
        protected const string FileId = "07e27f07-ff67-46b0-b93f-66106b40aeb8";

        protected readonly Guid FileGuid = new(FileId);
        protected readonly Stream Stream = new MemoryStream(new byte[] { 1, 2, 3 });

        protected IActionResult Result;

        public override async Task When()
        {
            Result = await Controller.AdminDownloadSpecialismResultsDataLinkAsync(Ukprn, FileId);
        }
    }
}