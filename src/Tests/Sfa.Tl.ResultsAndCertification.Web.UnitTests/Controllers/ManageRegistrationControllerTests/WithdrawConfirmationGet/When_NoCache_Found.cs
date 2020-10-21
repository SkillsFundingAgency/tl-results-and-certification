using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawConfirmationGet
{
    public class When_NoCache_Found : TestSetup
    {
        public override void Given()
        {
            MockResult = null;
            CacheService.GetAndRemoveAsync<WithdrawRegistrationResponse>(CacheKey).Returns(MockResult);
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
            CacheService.Received(1).GetAndRemoveAsync<WithdrawRegistrationResponse>(CacheKey);
        }
    }
}
