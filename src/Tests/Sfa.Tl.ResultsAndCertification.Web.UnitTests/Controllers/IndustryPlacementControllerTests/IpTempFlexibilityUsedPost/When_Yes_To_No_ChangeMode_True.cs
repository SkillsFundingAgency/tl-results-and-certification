using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpTempFlexibilityUsedPost
{
    public class When_Yes_To_No_ChangeMode_True : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;

        public override void Given()
        {
            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipTempFlexibilityUsedViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsTempFlexibilityUsed = true, IsChangeMode = true };

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
                            new IpLookupDataViewModel { Id = 1, Name = "Test 1", IsSelected = true }
                        }
                    }
                },
                IsChangeModeAllowed = true
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            ViewModel = new IpTempFlexibilityUsedViewModel { IsTempFlexibilityUsed = false, IsChangeMode = true };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(2).SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
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
