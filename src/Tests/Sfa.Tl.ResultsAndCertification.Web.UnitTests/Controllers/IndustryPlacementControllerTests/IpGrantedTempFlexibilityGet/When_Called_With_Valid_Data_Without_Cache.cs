using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpGrantedTempFlexibilityGet
{
    public class When_Called_With_Valid_Data_Without_Cache : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelUsedViewModel _ipModelUsedViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;
        private IpBlendedPlacementUsedViewModel _ipBlendedPlacementUsedViewModel;
        private IpGrantedTempFlexibilityViewModel _ipGrantedTempFlexibilityViewModel;

        public override void Given()
        {

            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipModelUsedViewModel = new IpModelUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsIpModelUsed = false };
            _ipTempFlexibilityUsedViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsTempFlexibilityUsed = true };
            _ipBlendedPlacementUsedViewModel = new IpBlendedPlacementUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsBlendedPlacementUsed = false };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = _ipModelUsedViewModel },
                TempFlexibility = new IpTempFlexibilityViewModel { IpTempFlexibilityUsed = _ipTempFlexibilityUsedViewModel, IpBlendedPlacementUsed = _ipBlendedPlacementUsedViewModel }
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            // Loader transformation
            _ipGrantedTempFlexibilityViewModel = new IpGrantedTempFlexibilityViewModel
            {
                LearnerName = _ipCompletionViewModel.LearnerName,
                TemporaryFlexibilities = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Id = 1, Name = "Temp Flex 1", IsSelected = false }, new IpLookupDataViewModel { Id = 2, Name = "Temp Flex 2", IsSelected = false } }
            };

            IndustryPlacementLoader.TransformIpCompletionDetailsTo<IpGrantedTempFlexibilityViewModel>(_cacheResult.IpCompletion).Returns(_ipGrantedTempFlexibilityViewModel);
            IndustryPlacementLoader.GetTemporaryFlexibilitiesAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear, false).Returns(_ipGrantedTempFlexibilityViewModel.TemporaryFlexibilities);

        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetTemporaryFlexibilitiesAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear, false);
            IndustryPlacementLoader.Received(1).TransformIpCompletionDetailsTo<IpGrantedTempFlexibilityViewModel>(_cacheResult.IpCompletion);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpGrantedTempFlexibilityViewModel));

            var model = viewResult.Model as IpGrantedTempFlexibilityViewModel;
            model.Should().NotBeNull();
            model.LearnerName.Should().Be(_ipTempFlexibilityUsedViewModel.LearnerName);
            model.IsTempFlexibilitySelected.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpBlendedPlacementUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
