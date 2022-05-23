using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionPost
{
    public class When_IpStatus_CompletedWithSpecialConsideration : TestSetup
    {
        private IpCompletionViewModel _ipCompletionViewModel;
        private IndustryPlacementViewModel _cacheResult;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = null
            };

            ViewModel = new IpCompletionViewModel
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.IpSpecialConsiderationHours);
            route.RouteValues.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }
    }
}
