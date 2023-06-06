using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SubmitWithdrawnStatusPost
{
    public class When_No_Selected : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AddWithdrawnStatusViewModel
            {
                ProfileId = 1,
                LearnerName = "test-learner-name",
                AcademicYear = 2020,
                IsPendingWithdrawl = false
            };
        }

        [Fact]
        public void Then_Redirected_To_LearnerRecordDetails()
        {
            var result = Result as RedirectToRouteResult;

            result.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);

            result.RouteValues.Should().HaveCount(1);
            result.RouteValues.Should().ContainKey("profileId");
            result.RouteValues["profileId"].Should().Be(ViewModel.ProfileId);
        }
    }
}