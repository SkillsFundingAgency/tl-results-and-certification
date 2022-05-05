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
    public class When_Called_With_Valid_Data_Without_Cache : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpBlendedPlacementUsedViewModel _ipBlendedPlacementUsedViewModel;
        private IpTempFlexNavigation _ipTempFlexNavigation;

        public override void Given()
        {
            
            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } }
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            // Loader navigation
            _ipTempFlexNavigation = new IpTempFlexNavigation { AskBlendedPlacement = true, AskTempFlexibility = true };
            IndustryPlacementLoader.GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear).Returns(_ipTempFlexNavigation);

            // Loader transformation
            _ipBlendedPlacementUsedViewModel = new IpBlendedPlacementUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName };
            IndustryPlacementLoader.TransformIpCompletionDetailsTo<IpBlendedPlacementUsedViewModel>(_cacheResult.IpCompletion).Returns(_ipBlendedPlacementUsedViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear);
            IndustryPlacementLoader.Received(1).TransformIpCompletionDetailsTo<IpBlendedPlacementUsedViewModel>(_cacheResult.IpCompletion);
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
            model.LearnerName.Should().Be(_ipBlendedPlacementUsedViewModel.LearnerName);
            model.IsBlendedPlacementUsed.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpTempFlexibilityUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
