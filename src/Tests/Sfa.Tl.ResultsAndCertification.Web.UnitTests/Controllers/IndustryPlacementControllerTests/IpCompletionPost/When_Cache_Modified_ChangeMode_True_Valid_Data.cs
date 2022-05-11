using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionPost
{
    public class When_Cache_Modified_ChangeMode_True_Valid_Data : TestSetup
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
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
            };

            ViewModel = new IpCompletionViewModel
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                IsChangeMode = true
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
                    Reasons = new SpecialConsiderationReasonsViewModel 
                    {
                        ReasonsList = new List<IpLookupDataViewModel> 
                        { 
                            new IpLookupDataViewModel { Id = 1, Name = "Test 1" }
                        }
                    }
                },
                IsChangeModeAllowed = true
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.IpCheckAndSubmit);
            route.RouteValues.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).GetAsync<IndustryPlacementViewModel>(CacheKey);
        }
    }
}
