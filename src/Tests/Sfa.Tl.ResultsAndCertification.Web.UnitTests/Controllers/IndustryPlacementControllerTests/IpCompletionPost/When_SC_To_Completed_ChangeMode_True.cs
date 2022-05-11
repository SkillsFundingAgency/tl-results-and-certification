using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionPost
{
    public class When_SC_To_Completed_ChangeMode_True : TestSetup
    {
        private IpCompletionViewModel _ipCompletionViewModel;
        private IndustryPlacementViewModel _cacheResult;

        public override void Given()
        {
            var isSuccess = true;

            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                IsChangeMode = true
            };

            ViewModel = new IpCompletionViewModel
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                SpecialConsideration = new SpecialConsiderationViewModel
                {
                    Hours = new SpecialConsiderationHoursViewModel
                    {
                        Hours = "50"
                    },
                    Reasons = null
                },
                IsChangeModeAllowed = true
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            IndustryPlacementLoader.ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheResult).Returns(isSuccess);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.IpModelUsed);
            route.RouteValues.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }        
    }
}
