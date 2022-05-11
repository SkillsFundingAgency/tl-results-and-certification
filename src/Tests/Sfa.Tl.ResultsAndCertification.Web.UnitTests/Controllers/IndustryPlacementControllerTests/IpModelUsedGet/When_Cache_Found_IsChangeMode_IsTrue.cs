using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedGet
{
    public class When_Cache_Found_IsChangeMode_IsTrue : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelUsedViewModel _ipModelUsedViewModel;

        public override void Given()
        {            
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            _ipModelUsedViewModel = new IpModelUsedViewModel { ProfileId = 1, IsIpModelUsed = true, IsChangeMode = true };

            _cacheResult = new IndustryPlacementViewModel
            {
                IsChangeModeAllowed = true,
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = _ipModelUsedViewModel }
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<IndustryPlacementViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpModelUsedViewModel));
            var model = viewResult.Model as IpModelUsedViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_ipModelUsedViewModel.ProfileId);
            model.LearnerName.Should().Be(_ipModelUsedViewModel.LearnerName);
            model.IsIpModelUsed.Should().BeTrue();
            model.IsChangeMode.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCheckAndSubmit);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
