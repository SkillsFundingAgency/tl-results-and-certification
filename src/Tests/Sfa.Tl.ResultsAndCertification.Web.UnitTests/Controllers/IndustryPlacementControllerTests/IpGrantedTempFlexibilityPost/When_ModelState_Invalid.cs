using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpGrantedTempFlexibilityPost
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
                TempFlexibility = new IpTempFlexibilityViewModel { IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel { IsBlendedPlacementUsed = false } }
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            ViewModel = new IpGrantedTempFlexibilityViewModel 
            { 
                LearnerName = "Test User",
                TemporaryFlexibilities = new List<IpLookupDataViewModel>
                {
                    new IpLookupDataViewModel { Id = 1, Name = "Temp Flex 1", IsSelected = false },
                    new IpLookupDataViewModel { Id = 1, Name = "Temp Flex 1", IsSelected = false },
                }
            };
            Controller.ModelState.AddModelError("IsTempFlexibilitySelected", Content.IndustryPlacement.IpGrantedTempFlexibility.Validation_Message);
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
            viewResult.Model.Should().BeOfType(typeof(IpGrantedTempFlexibilityViewModel));

            var model = viewResult.Model as IpGrantedTempFlexibilityViewModel;

            model.Should().NotBeNull();

            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.IsTempFlexibilitySelected.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpGrantedTempFlexibilityViewModel.IsTempFlexibilitySelected)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ViewModel.IsTempFlexibilitySelected)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpGrantedTempFlexibility.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpBlendedPlacementUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
