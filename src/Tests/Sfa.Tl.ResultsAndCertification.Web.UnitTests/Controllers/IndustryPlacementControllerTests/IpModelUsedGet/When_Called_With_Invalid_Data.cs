using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private IpModelUsedViewModel _ipModelUsedViewModel;
        private IpCompletionViewModel _ipCompletionViewModel;

        public override void Given()
        {

            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                LearnerName = "John Smith",
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };
            _ipModelUsedViewModel = new IpModelUsedViewModel { ProfileId = 1, LearnerName = "John Smith", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            IndustryPlacementLoader.TransformFromLearnerDetailsTo<IpModelUsedViewModel>(_ipCompletionViewModel).Returns(_ipModelUsedViewModel);
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(new IndustryPlacementViewModel());
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<IndustryPlacementViewModel>(CacheKey);
            
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
