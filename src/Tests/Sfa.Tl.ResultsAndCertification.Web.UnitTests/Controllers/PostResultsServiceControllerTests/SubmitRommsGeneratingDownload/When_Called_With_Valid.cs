using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.SubmitRommsGeneratingDownload
{
    public class When_Called_With_Valid : TestSetup
    {
        private readonly DataExportResponse _rommsResponse = new()
        {
            BlobUniqueReference = new Guid("f47d7a4e-9b8c-4a6f-8e4d-2e3b1a5c9f0d"),
            FileSize = 100,
            ComponentType = ComponentType.NotSpecified,
            IsDataFound = true
        };

        public override void Given()
        {
            PostResultsServiceLoader.GenerateRommsDataExportAsync(AoUkprn, UserEmail).Returns(new List<DataExportResponse> { _rommsResponse });
        }

        [Fact]
        public void Then_Redirected_To_PostResultsServicesDownloadData()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RommsDownloadData);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            string fileType = FileType.Csv.ToString().ToUpperInvariant();

            CacheService.Received(1).SetAsync(
                CacheKey,
                Arg.Is<RommsDownloadViewModel>(x =>
                    x.RommsDownloadLinkViewModel.BlobUniqueReference == _rommsResponse.BlobUniqueReference
                    && x.RommsDownloadLinkViewModel.FileSize == _rommsResponse.FileSize
                    && x.RommsDownloadLinkViewModel.FileType == fileType),
                CacheExpiryTime.XSmall);
        }
    }
}
