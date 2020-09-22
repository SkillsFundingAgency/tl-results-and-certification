using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReJoinRegistrationPost
{
    public class When_ReJoin_Failed : TestSetup
    {
        private ReJoinRegistrationResponse mockResponse = null;

        public override void Given()
        {
            ViewModel.CanReJoin = true;
            mockResponse = new ReJoinRegistrationResponse
            {
                IsSuccess = false
            };
            RegistrationLoader.ReJoinRegistrationAsync(AoUkprn, ViewModel).Returns(mockResponse);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
