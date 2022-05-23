using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerOtherPost
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelUsedViewModel _ipModelUsedViewModel;
        private IpMultiEmployerUsedViewModel _ipMultiEmployerUsedViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, PathwayId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus =IndustryPlacementStatus.Completed };
            _ipModelUsedViewModel = new IpModelUsedViewModel { IsIpModelUsed = true };
            _ipMultiEmployerUsedViewModel = new IpMultiEmployerUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsMultiEmployerModelUsed = true };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel 
                { 
                    IpModelUsed = _ipModelUsedViewModel, IpMultiEmployerUsed = _ipMultiEmployerUsedViewModel
                }
            };

            ViewModel = new IpMultiEmployerOtherViewModel
            {
                LearnerName = _ipCompletionViewModel.LearnerName,
                OtherIpPlacementModels = new List<IpLookupDataViewModel>
                {
                    new IpLookupDataViewModel { Id = 1, Name = "Test 1", IsSelected = false },
                    new IpLookupDataViewModel { Id = 2, Name = "Test 2", IsSelected = true }
                }
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.IpTempFlexibilityUsed);
        }
    }
}
