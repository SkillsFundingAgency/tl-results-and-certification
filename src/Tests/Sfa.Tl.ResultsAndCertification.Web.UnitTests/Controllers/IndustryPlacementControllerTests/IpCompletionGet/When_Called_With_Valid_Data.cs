using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IpCompletionViewModel _ipCompletionViewModel;

        public override void Given()
        {
            ProfileId = 1;
            PathwayId = 1;

            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = ProfileId, PathwayId = PathwayId, AcademicYear = 2020, LearnerName = "Test Test", IndustryPlacementStatus = null };

            IndustryPlacementLoader.GetLearnerRecordDetailsAsync<IpCompletionViewModel>(ProviderUkprn, ProfileId).Returns(_ipCompletionViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetLearnerRecordDetailsAsync<IpCompletionViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as IpCompletionViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_ipCompletionViewModel.ProfileId);
            model.PathwayId.Should().Be(_ipCompletionViewModel.PathwayId);
            model.AcademicYear.Should().Be(_ipCompletionViewModel.AcademicYear);
            model.LearnerName.Should().Be(_ipCompletionViewModel.LearnerName);
            model.IndustryPlacementStatus.Should().BeNull();
            model.IsValid.Should().BeTrue();
            model.IsChangeMode.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
