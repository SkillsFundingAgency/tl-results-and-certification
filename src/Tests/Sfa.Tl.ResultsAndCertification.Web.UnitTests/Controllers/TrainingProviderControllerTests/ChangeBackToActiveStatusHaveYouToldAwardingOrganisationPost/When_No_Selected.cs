using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveStatusHaveYouToldAwardingOrganisationPost
{
    public class When_No_Selected : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel
            {
                HaveYouToldAwardingOrganisation = false,
                ProfileId = 1,
                AwardingOrganisationName = "test-ao-name",
                ProviderUkprn = 1123456789,
                LearnerName = "test-learner-name",
                AcademicYear = 2020
            };

            TrainingProviderLoader.ReinstateRegistrationFromPendingWithdrawalAsync(ViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).ReinstateRegistrationFromPendingWithdrawalAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_LearnerRecordDetails()
        {
            var result = Result as RedirectToRouteResult;

            result.RouteName.Should().Be(RouteConstants.ChangeBackToActiveAOMessage);

            result.RouteValues.Should().HaveCount(1);
            result.RouteValues.Should().ContainKey("profileId");
            result.RouteValues["profileId"].Should().Be(ViewModel.ProfileId);
        }
    }
}