using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.AdminDownloadLearnerResultsCsvGet
{
    public class When_File_Not_Generated : AdminDownloadLearnerResultsControllerBaseTest
    {
        private const long ProviderUkprn = 10000536;
        private const string Email = "test@email.com";

        private IActionResult _result;

        public override void Given()
        {
            DownloadOverallResultsLoader
                .DownloadOverallResultsDataAsync(ProviderUkprn, Email)
                .Returns(null as FileStream);
        }

        public override async Task When()
        {
            _result = await Controller.AdminDownloadLearnerResultsCsvAsync(ProviderUkprn);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            DownloadOverallResultsLoader.Received(1).DownloadOverallResultsDataAsync(ProviderUkprn, Email);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (_result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}