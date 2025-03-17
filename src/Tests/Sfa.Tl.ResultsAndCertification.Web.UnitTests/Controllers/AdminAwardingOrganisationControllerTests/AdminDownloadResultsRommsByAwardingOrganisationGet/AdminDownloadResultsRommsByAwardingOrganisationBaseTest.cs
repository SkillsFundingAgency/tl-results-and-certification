using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadResultsRommsByAwardingOrganisationGet
{
    public class AdminDownloadResultsRommsByAwardingOrganisationBaseTest : AdminAwardingOrganisationControllerBaseTest
    {
        protected const long Ukprn = 10009696;
        protected const string DisplayName = "NCFE";

        protected IActionResult Result;

        public override async Task When()
        {
            Result = await Controller.AdminDownloadResultsRommsByAwardingOrganisationAsync(Ukprn);
        }

        protected static DataExportResponse CreateDataExportResponse(ComponentType componentType, bool isDataFound = true)
            => new()
            {
                BlobUniqueReference = new Guid(),
                ComponentType = componentType,
                IsDataFound = isDataFound,
                FileSize = 10
            };

        protected void CallExpectedMethods()
        {
            Loader.Received(1).GetAwardingOrganisationDisplayName(Ukprn);
            ResultLoader.Received(1).GenerateResultsExportAsync(Ukprn, UserEmail);
            PostResultsLoader.Received(1).GenerateRommsDataExportAsync(Ukprn, UserEmail);
        }

        protected static void AssertDownloadLink(DownloadLinkViewModel downloadLink, DataExportResponse response)
        {
            downloadLink.Should().NotBeNull();
            downloadLink.BlobUniqueReference.Should().Be(response.BlobUniqueReference);
            downloadLink.FileSize.Should().Be(response.FileSize);
            downloadLink.FileType.Should().Be("CSV");
        }
    }
}
