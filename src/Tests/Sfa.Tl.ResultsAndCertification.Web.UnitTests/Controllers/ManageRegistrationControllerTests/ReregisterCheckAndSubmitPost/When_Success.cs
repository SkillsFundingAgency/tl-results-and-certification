using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCheckAndSubmitPost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            MockResult.IsSuccess = true;

            var cacheResult = new ReregisterViewModel();
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.ReregistrationAsync(AoUkprn, cacheResult).Returns(MockResult);            
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).RemoveAsync<ReregisterViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.ReregistrationConfirmationViewModel), Arg.Any<ReregistrationResponse>(), Common.Enum.CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_ReregistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ReregistrationConfirmation);
        }
    }
}
