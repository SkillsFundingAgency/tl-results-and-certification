using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionGet
{
    public class When_Called_With_Invalid_IpStatus_For_Change_Journey : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;

        public override void Given()
        {
            SetRouteAttribute(RouteConstants.IpCompletionChange);
            ProfileId = 1;
            PathwayId = 1;

            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = ProfileId,
                PathwayId = PathwayId,
                LearnerName = "Test Test",
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.NotSpecified
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.DidNotReceive().GetLearnerRecordDetailsAsync<IpCompletionViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
