using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedPost
{
    public class When_ModelState_Invalid_With_CompletedWithSpecialConsideration : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private SpecialConsiderationViewModel _specialConsiderationViewModel;
        private SpecialConsiderationReasonsViewModel _specialConsiderationReasonsViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1, 
                AcademicYear = 2020, 
                LearnerName = "First Last",
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
            };
            ViewModel = new IpModelUsedViewModel { IsIpModelUsed = true };

            _specialConsiderationReasonsViewModel = new SpecialConsiderationReasonsViewModel
            {
                AcademicYear = _ipCompletionViewModel.AcademicYear,
                LearnerName = _ipCompletionViewModel.LearnerName,
                ReasonsList = new List<IpLookupDataViewModel> { new() { Id = 1, Name = "Medical", IsSelected = true }, new() { Id = 2, Name = "Withdrawn", IsSelected = true } }
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

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            ProfileId = 1;
            ViewModel = new IpModelUsedViewModel { ProfileId = ProfileId, LearnerName = "John Smith", IsIpModelUsed = null };
            Controller.ModelState.AddModelError("IsIpModelUsed", Content.IndustryPlacement.IpModelUsed.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpModelUsedViewModel));

            var model = viewResult.Model as IpModelUsedViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.IsIpModelUsed.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpModelUsedViewModel.IsIpModelUsed)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(IpModelUsedViewModel.IsIpModelUsed)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpModelUsed.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpSpecialConsiderationReasons);
        }
    }
}
