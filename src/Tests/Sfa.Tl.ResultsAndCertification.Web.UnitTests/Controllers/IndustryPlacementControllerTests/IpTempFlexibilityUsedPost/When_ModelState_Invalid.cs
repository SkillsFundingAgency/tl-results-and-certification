using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpTempFlexibilityUsedPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;

        public override void Given()
        {
            // Cache object
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel(),
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } },
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);


            ViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = "Test User" };
            Controller.ModelState.AddModelError("IsTempFlexibilityUsed", Content.IndustryPlacement.IpTempFlexibilityUsed.Validation_Message);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, _cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpTempFlexibilityUsedViewModel));

            var model = viewResult.Model as IpTempFlexibilityUsedViewModel;

            model.Should().NotBeNull();

            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.IsTempFlexibilityUsed.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpTempFlexibilityUsedViewModel.IsTempFlexibilityUsed)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ViewModel.IsTempFlexibilityUsed)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpTempFlexibilityUsed.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpModelUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
