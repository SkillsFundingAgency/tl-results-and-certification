using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReJoinRegistrationPost
{
    public class When_ReJoin_Success : TestSetup
    {
        private ReJoinRegistrationResponse mockResponse = null;
        public override void Given()
        {
            ViewModel.CanReJoin = true;
            mockResponse = new ReJoinRegistrationResponse
            {
                ProfileId = 1,
                Uln = 7654332198,
                IsSuccess = true,
            };
            RegistrationLoader.ReJoinRegistrationAsync(AoUkprn, ViewModel).Returns(mockResponse);
        }

        [Fact]
        public void Then_Redirected_To_ReJoinRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ReJoinRegistrationConfirmation);
        }
    }
}
