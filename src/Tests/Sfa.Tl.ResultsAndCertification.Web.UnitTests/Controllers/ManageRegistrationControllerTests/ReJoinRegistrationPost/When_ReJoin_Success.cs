using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.RejoinRegistrationPost
{
    public class When_Rejoin_Success : TestSetup
    {
        private RejoinRegistrationResponse mockResponse = null;
        public override void Given()
        {
            ViewModel.CanRejoin = true;
            mockResponse = new RejoinRegistrationResponse
            {
                ProfileId = 1,
                Uln = 7654332198,
                IsSuccess = true,
            };
            RegistrationLoader.RejoinRegistrationAsync(AoUkprn, ViewModel).Returns(mockResponse);
        }

        [Fact]
        public void Then_Redirected_To_RejoinRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RejoinRegistrationConfirmation);
        }
    }
}
