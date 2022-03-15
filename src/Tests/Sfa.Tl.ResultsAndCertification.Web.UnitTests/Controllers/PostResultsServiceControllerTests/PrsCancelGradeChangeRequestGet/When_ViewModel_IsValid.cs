using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelGradeChangeRequestGet
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private PrsCancelGradeChangeRequestViewModel _mockCancelGradeChangeRequestViewModel;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;

            _mockCancelGradeChangeRequestViewModel = new PrsCancelGradeChangeRequestViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Status = RegistrationPathwayStatus.Active,
                PrsStatus = null,
                AppealEndDate = DateTime.Now.AddDays(-5)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsCancelGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core).Returns(_mockCancelGradeChangeRequestViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsCancelGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType.Core);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsCancelGradeChangeRequestViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_mockCancelGradeChangeRequestViewModel.ProfileId);
            model.AssessmentId.Should().Be(_mockCancelGradeChangeRequestViewModel.AssessmentId);
            model.Status.Should().Be(_mockCancelGradeChangeRequestViewModel.Status);
            model.PrsStatus.Should().Be(_mockCancelGradeChangeRequestViewModel.PrsStatus);
            model.IsResultJourney.Should().BeFalse();
            model.AreYouSureToCancel.Should().BeNull();

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsGradeChangeRequest);
            model.BackLink.RouteAttributes.Count.Should().Be(2);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileId);
            profileId.Should().Be(_mockCancelGradeChangeRequestViewModel.ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentId);
            assessmentId.Should().Be(_mockCancelGradeChangeRequestViewModel.AssessmentId.ToString());
        }
    }
}
