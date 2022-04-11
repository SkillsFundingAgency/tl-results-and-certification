using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelRommUpdatePost
{
    public class When_AreYouSureToCancel_IsTrue : TestSetup
    {
        public override void Given()
        {
            ViewModel = new PrsCancelRommUpdateViewModel { ProfileId = 1, AreYouSureToCancel = true };            
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).RemoveAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
