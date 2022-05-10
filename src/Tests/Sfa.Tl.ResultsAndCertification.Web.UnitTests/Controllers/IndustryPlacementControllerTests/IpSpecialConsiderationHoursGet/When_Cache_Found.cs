using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationHoursGet
{
    public class When_Called_With_Valid_Data1 : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private SpecialConsiderationViewModel _specialConsiderationViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration };

            _specialConsiderationViewModel = new SpecialConsiderationViewModel
            {
                Hours = new SpecialConsiderationHoursViewModel { Hours = "300", ProfileId = _ipCompletionViewModel.ProfileId, LearnerName = _ipCompletionViewModel.LearnerName },
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                SpecialConsideration = _specialConsiderationViewModel
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.DidNotReceive().TransformIpCompletionDetailsTo<SpecialConsiderationHoursViewModel>(Arg.Any<IpCompletionViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SpecialConsiderationHoursViewModel));

            var model = viewResult.Model as SpecialConsiderationHoursViewModel;
            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_ipCompletionViewModel.ProfileId);
            model.LearnerName.Should().Be(_ipCompletionViewModel.LearnerName);
            model.Hours.Should().Be(_specialConsiderationViewModel.Hours.Hours);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCompletion);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(_ipCompletionViewModel.ProfileId.ToString());
        }
    }
}
