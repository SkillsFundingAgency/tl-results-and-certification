using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitGet
{
    public class When_LearrnerDetails_Not_Found : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private readonly IpCheckAndSubmitViewModel _learnerDetails = null;

        public override void Given()
        {
            // Cache object
            _cacheResult = new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { PathwayId = 1 } };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            // LearnerDetails
            IndustryPlacementLoader.GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(ProviderUkprn, _cacheResult.IpCompletion.PathwayId).Returns(_learnerDetails);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(ProviderUkprn, _cacheResult.IpCompletion.PathwayId);
            IndustryPlacementLoader.DidNotReceive().GetTempFlexNavigationAsync(Arg.Any<int>(), Arg.Any<int>());
            IndustryPlacementLoader.DidNotReceive().GetIpSummaryDetailsListAsync(Arg.Any<IndustryPlacementViewModel>(), Arg.Any<IpTempFlexNavigation>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
