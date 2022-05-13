using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpTempFlexibilityUsedGet
{
    public class When_Cache_Found_IsChangeMode_IsTrue : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;
        private IpTempFlexNavigation _ipTempFlexNavigation;

        public override void Given()
        {
            IsChangeMode = true;
            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipTempFlexibilityUsedViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsTempFlexibilityUsed = true };
            
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

            // Loader navigation
            _ipTempFlexNavigation = new IpTempFlexNavigation { AskBlendedPlacement = true, AskTempFlexibility = true };
            IndustryPlacementLoader.GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear).Returns(_ipTempFlexNavigation);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear);
            IndustryPlacementLoader.DidNotReceive().TransformIpCompletionDetailsTo<IpTempFlexibilityUsedViewModel>(Arg.Any<IpCompletionViewModel>());
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpTempFlexibilityUsedViewModel));

            var model = viewResult.Model as IpTempFlexibilityUsedViewModel;
            model.Should().NotBeNull();
            model.LearnerName.Should().Be(_ipTempFlexibilityUsedViewModel.LearnerName);
            model.IsTempFlexibilityUsed.Should().BeTrue();
            model.IsChangeMode.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCheckAndSubmit);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
