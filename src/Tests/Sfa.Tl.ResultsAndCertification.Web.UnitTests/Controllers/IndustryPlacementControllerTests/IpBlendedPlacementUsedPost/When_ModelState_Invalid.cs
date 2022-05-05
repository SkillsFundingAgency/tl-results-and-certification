using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpBlendedPlacementUsedPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpTempFlexNavigation _ipTempFlexNavigation;

        public override void Given()
        {
            // Cache object
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel(),
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } },
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            // Loader navigation
            _ipTempFlexNavigation = new IpTempFlexNavigation { AskBlendedPlacement = true, AskTempFlexibility = true };
            IndustryPlacementLoader.GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear).Returns(_ipTempFlexNavigation);


            ViewModel = new IpBlendedPlacementUsedViewModel { LearnerName = "Test User" };
            Controller.ModelState.AddModelError("IsBlendedPlacementUsed", Content.IndustryPlacement.IpBlendedPlacementUsed.Validation_Message);
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
            viewResult.Model.Should().BeOfType(typeof(IpBlendedPlacementUsedViewModel));

            var model = viewResult.Model as IpBlendedPlacementUsedViewModel;

            model.Should().NotBeNull();

            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.IsBlendedPlacementUsed.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpBlendedPlacementUsedViewModel.IsBlendedPlacementUsed)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ViewModel.IsBlendedPlacementUsed)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpBlendedPlacementUsed.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpTempFlexibilityUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
