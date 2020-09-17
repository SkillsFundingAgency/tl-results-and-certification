using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeProviderAndCoreNeedToWithdrawGet
{
    public class When_NoCache_Found : TestSetup
    {
        private ChangeProviderCoreNotSupportedViewModel cacheResult;
        public override void Given()
        {
            cacheResult = null;
            CacheService.GetAsync<ChangeProviderCoreNotSupportedViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<ChangeProviderCoreNotSupportedViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }        
    }
}
