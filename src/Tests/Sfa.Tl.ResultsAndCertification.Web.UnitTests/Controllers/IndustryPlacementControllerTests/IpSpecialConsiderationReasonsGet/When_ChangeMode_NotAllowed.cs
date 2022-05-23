using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationReasonsGet
{
    public class When_ChangeMode_NotAllowed : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private SpecialConsiderationViewModel _specialConsiderationViewModel;

        public override void Given()
        {
            IsChangeMode = true;

            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration };
            _specialConsiderationViewModel = new SpecialConsiderationViewModel
            {
                Hours = new SpecialConsiderationHoursViewModel { ProfileId = _ipCompletionViewModel.ProfileId, Hours = "999", LearnerName = _ipCompletionViewModel.LearnerName },
                Reasons = new SpecialConsiderationReasonsViewModel { LearnerName = "First Last", IsChangeMode = true, ReasonsList = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Id = 1, Name = "Test 1", IsSelected = true } } }
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IsChangeModeAllowed = false,
                IpCompletion = _ipCompletionViewModel,
                SpecialConsideration = _specialConsiderationViewModel
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
            viewResult.Model.Should().BeOfType(typeof(SpecialConsiderationReasonsViewModel));

            var model = viewResult.Model as SpecialConsiderationReasonsViewModel;
            model.Should().NotBeNull();
            model.LearnerName.Should().Be(_ipCompletionViewModel.LearnerName);
            model.IsChangeMode.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpSpecialConsiderationHours);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
