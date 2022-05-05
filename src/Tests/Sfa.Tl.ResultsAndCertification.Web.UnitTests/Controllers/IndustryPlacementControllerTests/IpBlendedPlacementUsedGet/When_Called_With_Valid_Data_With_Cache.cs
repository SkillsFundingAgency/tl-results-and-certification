using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpBlendedPlacementUsedGet
{
    public class When_Called_With_Valid_Data_With_Cache : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpTempFlexibilityUsedViewModel  _ipTempFlexibilityUsedViewModel;
        private IpTempFlexNavigation _ipTempFlexNavigation;

        public override void Given()
        {
            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipTempFlexibilityUsedViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName };
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } },
                TempFlexibility = new IpTempFlexibilityViewModel { IpTempFlexibilityUsed = _ipTempFlexibilityUsedViewModel, IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel { IsBlendedPlacementUsed = true } } 
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
            IndustryPlacementLoader.DidNotReceive().TransformIpCompletionDetailsTo<IpBlendedPlacementUsedViewModel>(Arg.Any<IpCompletionViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpBlendedPlacementUsedViewModel));

            var model = viewResult.Model as IpBlendedPlacementUsedViewModel;
            model.Should().NotBeNull();
            model.LearnerName.Should().Be(_cacheResult.TempFlexibility.IpBlendedPlacementUsed.LearnerName);
            model.IsBlendedPlacementUsed.Should().Be(_cacheResult.TempFlexibility.IpBlendedPlacementUsed.IsBlendedPlacementUsed);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpTempFlexibilityUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
