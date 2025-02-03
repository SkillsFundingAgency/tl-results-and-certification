using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.SubmitRommsGeneratingDownload
{
    public class When_Called_With_DataNotFound : TestSetup
    {
        public override void Given()
        {
            DataExportResponse response = new()
            {
                IsDataFound = false
            };

            PostResultsServiceLoader.GenerateRommsDataExportAsync(AoUkprn, UserEmail).Returns(new List<DataExportResponse> { response });
        }

        [Fact]
        public void Then_Redirected_To_PostResultsServicesNoRecordsFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RommsNoRecordsFound);
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            CacheService.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<RommsNoRecordsFoundViewModel>(),
                Arg.Any<CacheExpiryTime>());
        }
    }
}
