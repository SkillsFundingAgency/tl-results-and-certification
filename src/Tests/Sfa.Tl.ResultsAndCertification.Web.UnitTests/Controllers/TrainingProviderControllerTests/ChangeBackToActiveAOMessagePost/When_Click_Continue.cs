using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveAOMessagePost
{
    public class When_Click_Continue : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ChangeBackToActiveAOMessageViewModel
            {
                ProfileId = 1,
                AwardingOrganisationName = "test-ao-name",
                LearnerName = "test-learner-name",
                AcademicYear = 2020
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
