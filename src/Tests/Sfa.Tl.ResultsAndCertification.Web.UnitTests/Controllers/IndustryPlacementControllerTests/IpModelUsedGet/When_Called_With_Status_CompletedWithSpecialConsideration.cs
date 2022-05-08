using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedGet
{
    public class When_Called_With_Status_CompletedWithSpecialConsideration : TestSetup
    {
        private IpModelUsedViewModel _ipModelUsedViewModel;
        private IpCompletionViewModel _ipCompletionViewModel;
        private SpecialConsiderationViewModel _specialConsiderationViewModel;
        private SpecialConsiderationReasonsViewModel _specialConsiderationReasonsViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                LearnerName = "John Smith",
                AcademicYear = 2020,
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
            };

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
            
            _ipModelUsedViewModel = new IpModelUsedViewModel { ProfileId = 1, LearnerName = "John Smith" };
            IndustryPlacementLoader.TransformIpCompletionDetailsTo<IpModelUsedViewModel>(_ipCompletionViewModel).Returns(_ipModelUsedViewModel);
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(new IndustryPlacementViewModel()
            {
                IpCompletion = _ipCompletionViewModel,
                SpecialConsideration = _specialConsiderationViewModel

            });
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<IndustryPlacementViewModel>(CacheKey);
            IndustryPlacementLoader.Received(1).TransformIpCompletionDetailsTo<IpModelUsedViewModel>(_ipCompletionViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as IpModelUsedViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_ipModelUsedViewModel.ProfileId);
            model.LearnerName.Should().Be(_ipModelUsedViewModel.LearnerName);
            model.IsIpModelUsed.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpSpecialConsiderationReasons);
        }
    }
}
