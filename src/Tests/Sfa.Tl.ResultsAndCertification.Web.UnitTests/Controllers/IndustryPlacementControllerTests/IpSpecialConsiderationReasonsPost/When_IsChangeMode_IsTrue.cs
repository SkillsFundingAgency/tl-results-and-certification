using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationReasonsPost
{
    public class When_IsChangeMode_IsTrue : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private SpecialConsiderationViewModel _specialConsiderationViewModel;
        private SpecialConsiderationReasonsViewModel _specialConsiderationReasonsViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration };

            _specialConsiderationReasonsViewModel = new SpecialConsiderationReasonsViewModel
            {
                AcademicYear = _ipCompletionViewModel.AcademicYear,
                LearnerName = _ipCompletionViewModel.LearnerName,
                ReasonsList = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Id = 1, Name = "Medical", IsSelected = true }, new IpLookupDataViewModel { Id = 2, Name = "Withdrawn", IsSelected = true } },
                IsChangeMode = true
            };

            _specialConsiderationViewModel = new SpecialConsiderationViewModel
            {
                Hours = new SpecialConsiderationHoursViewModel(),
                Reasons = _specialConsiderationReasonsViewModel
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                SpecialConsideration = _specialConsiderationViewModel
            };

            ViewModel = _specialConsiderationReasonsViewModel;
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
            routeName.Should().Be(RouteConstants.IpCheckAndSubmit);
        }
    }
}
