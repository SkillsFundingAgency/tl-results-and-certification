using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AppealGradeAfterDeadlineViewModel _appealGradeAfterDeadlineViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _appealGradeAfterDeadlineViewModel = new AppealGradeAfterDeadlineViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                ResultId = ResultId,
                AppealEndDate = DateTime.UtcNow.AddDays(-3),
                PathwayPrsStatus = null,
                Status = RegistrationPathwayStatus.Active
            };

            Loader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealGradeAfterDeadlineViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineViewModel>(AoUkprn, ProfileId, AssessmentId);
        }


        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AppealGradeAfterDeadlineViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealGradeAfterDeadlineViewModel.ProfileId);
            model.AssessmentId.Should().Be(_appealGradeAfterDeadlineViewModel.AssessmentId);
            model.ResultId.Should().Be(_appealGradeAfterDeadlineViewModel.ResultId);
            model.AppealEndDate.Should().Be(_appealGradeAfterDeadlineViewModel.AppealEndDate);
            model.PathwayPrsStatus.Should().Be(_appealGradeAfterDeadlineViewModel.PathwayPrsStatus);
            model.Status.Should().Be(_appealGradeAfterDeadlineViewModel.Status);
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(2);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
        }
    }
}
