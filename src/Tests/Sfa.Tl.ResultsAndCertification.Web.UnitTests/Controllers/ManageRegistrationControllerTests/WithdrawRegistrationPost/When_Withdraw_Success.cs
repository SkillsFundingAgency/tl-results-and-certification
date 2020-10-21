using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationPost
{
    public class When_Withdraw_Success : TestSetup
    {
        private WithdrawRegistrationResponse mockResponse = null;
        public override void Given()
        {
            ViewModel.CanWithdraw = true;
            mockResponse = new WithdrawRegistrationResponse
            {
                ProfileId = 1,
                Uln = 7654332198,
                IsSuccess = true,
                IsRequestFromProviderAndCorePage = false
            };
            RegistrationLoader.WithdrawRegistrationAsync(AoUkprn, ViewModel).Returns(mockResponse);
        }

        [Fact]
        public void Then_Redirected_To_WithdrawRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.WithdrawRegistrationConfirmation);
        }
    }
}
