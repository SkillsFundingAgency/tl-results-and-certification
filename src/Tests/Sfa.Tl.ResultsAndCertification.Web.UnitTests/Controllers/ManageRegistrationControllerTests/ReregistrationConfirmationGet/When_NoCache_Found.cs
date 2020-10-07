using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregistrationConfirmationGet
{
    public class When_NoCache_Found : TestSetup
    {
        public override void Given()
        {
            MockResult = null;
            CacheService.GetAndRemoveAsync<ReregistrationResponse>(Arg.Any<string>()).Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }

        [Fact]
        public void Then_CacheService_Called()
        {
            CacheService.Received().GetAndRemoveAsync<ReregistrationResponse>(Arg.Any<string>());
        }
    }
}
