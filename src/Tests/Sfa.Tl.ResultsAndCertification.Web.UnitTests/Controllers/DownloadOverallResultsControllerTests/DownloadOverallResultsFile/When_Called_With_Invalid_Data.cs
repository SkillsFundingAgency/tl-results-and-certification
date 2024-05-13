using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.DownloadOverallResultsFile
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private FileStream _stream;

        public override void Given()
        {
            DownloadOverallResultsLoader.DownloadOverallResultsDataAsync(ProviderUkprn, Email).Returns(_stream);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            DownloadOverallResultsLoader.Received(1).DownloadOverallResultsDataAsync(ProviderUkprn, Email);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
