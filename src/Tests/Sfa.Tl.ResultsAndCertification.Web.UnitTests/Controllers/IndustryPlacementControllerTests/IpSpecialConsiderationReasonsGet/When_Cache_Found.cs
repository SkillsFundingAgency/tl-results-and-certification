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
    public class When_Called_With_Valid_Data1 : TestSetup
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
                ReasonsList = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Id = 1, Name = "Medical", IsSelected = true }, new IpLookupDataViewModel { Id = 2, Name = "Withdrawn", IsSelected = true } }
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

            IndustryPlacementLoader.GetSpecialConsiderationReasonsListAsync(2020).Returns(_specialConsiderationReasonsViewModel.ReasonsList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.DidNotReceive().TransformIpCompletionDetailsTo<SpecialConsiderationReasonsViewModel>(Arg.Any<IpCompletionViewModel>());
            IndustryPlacementLoader.Received(1).GetSpecialConsiderationReasonsListAsync(2020);
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
            model.AcademicYear.Should().Be(_ipCompletionViewModel.AcademicYear);
            model.IsReasonSelected.Should().BeTrue();
            model.ReasonsList.Should().BeEquivalentTo(_specialConsiderationReasonsViewModel.ReasonsList);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpSpecialConsiderationHours);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
