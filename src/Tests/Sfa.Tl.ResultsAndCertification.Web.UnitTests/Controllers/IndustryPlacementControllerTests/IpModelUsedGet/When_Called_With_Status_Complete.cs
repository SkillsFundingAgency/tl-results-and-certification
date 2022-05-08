using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedGet
{
    public class When_Called_With_Status_Complete : TestSetup
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
            _ipModelUsedViewModel = new IpModelUsedViewModel { ProfileId = 1, LearnerName = "John Smith" };
            IndustryPlacementLoader.TransformIpCompletionDetailsTo<IpModelUsedViewModel>(_ipCompletionViewModel).Returns(_ipModelUsedViewModel);
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(new IndustryPlacementViewModel()
            {
                IpCompletion = _ipCompletionViewModel
            });
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<IndustryPlacementViewModel>(CacheKey);
            IndustryPlacementLoader.Received(1).TransformIpCompletionDetailsTo<IpModelUsedViewModel>(_ipCompletionViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as IpModelUsedViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_ipModelUsedViewModel.ProfileId);
            model.LearnerName.Should().Be(_ipModelUsedViewModel.LearnerName);
            model.IsIpModelUsed.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCompletion);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(_ipCompletionViewModel.ProfileId.ToString());
        }
    }
}
