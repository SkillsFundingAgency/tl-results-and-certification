using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.UploadSuccessful
{
    public class When_NoCache_Found : TestSetup
    {
        public override void Given() { }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received().GetAndRemoveAsync<UploadSuccessfulViewModel>(Arg.Any<string>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            // Controller
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
