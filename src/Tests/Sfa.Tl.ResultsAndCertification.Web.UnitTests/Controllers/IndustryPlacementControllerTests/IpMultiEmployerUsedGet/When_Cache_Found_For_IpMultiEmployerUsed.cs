using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerUsedGet
{
    public class When_Cache_Found_For_IpMultiEmployerUsed : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelUsedViewModel _ipModelUsedViewModel;
        private IpMultiEmployerUsedViewModel _ipMultiEmployerUsedViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            _ipModelUsedViewModel = new IpModelUsedViewModel { ProfileId = 1, IsIpModelUsed = true };
            _ipMultiEmployerUsedViewModel = new IpMultiEmployerUsedViewModel { LearnerName = "First Last", IsMultiEmployerModelUsed = true };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = _ipModelUsedViewModel, IpMultiEmployerUsed = _ipMultiEmployerUsedViewModel }
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
            viewResult.Model.Should().BeOfType(typeof(IpMultiEmployerUsedViewModel));
            var model = viewResult.Model as IpMultiEmployerUsedViewModel;

            model.Should().NotBeNull();
            model.LearnerName.Should().Be(_ipMultiEmployerUsedViewModel.LearnerName);
            model.IsMultiEmployerModelUsed.Should().BeTrue();
            model.IsChangeMode.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpModelUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
