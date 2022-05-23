using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpBlendedPlacementUsedPost
{
    public class When_No_Selection_Changed_ChangeMode_True : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;
        private IpBlendedPlacementUsedViewModel _ipBlendedPlacementUsedViewModel;

        public override void Given()
        {
            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipTempFlexibilityUsedViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsTempFlexibilityUsed = true };
            _ipBlendedPlacementUsedViewModel = new IpBlendedPlacementUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsBlendedPlacementUsed = false };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true } },
                TempFlexibility = new IpTempFlexibilityViewModel
                {
                    IpTempFlexibilityUsed = _ipTempFlexibilityUsedViewModel,
                    IpGrantedTempFlexibility = new IpGrantedTempFlexibilityViewModel
                    {
                        TemporaryFlexibilities = new List<IpLookupDataViewModel>
                        {
                            new IpLookupDataViewModel
                            {
                                Id = 1,
                                Name = "Test 1",
                                IsSelected = true
                            }
                        }
                    },
                    IpBlendedPlacementUsed = _ipBlendedPlacementUsedViewModel
                },
                IsChangeModeAllowed = true
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            ViewModel = new IpBlendedPlacementUsedViewModel { IsBlendedPlacementUsed = false, IsChangeMode = true };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.IpCheckAndSubmit);
            route.RouteValues.Should().BeNullOrEmpty();
        }
    }
}
