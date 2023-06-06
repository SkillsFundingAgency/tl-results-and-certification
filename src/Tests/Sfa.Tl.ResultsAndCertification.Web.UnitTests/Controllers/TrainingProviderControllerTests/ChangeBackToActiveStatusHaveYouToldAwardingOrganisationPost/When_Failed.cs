using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveStatusHaveYouToldAwardingOrganisationPost
{
    public class When_Failed : TestSetup
    {

        public override void Given()
        {
            ProfileId = 1;

            ViewModel = new ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel
            {
                HaveYouToldAwardingOrganisation = true,
                ProfileId = 1,
                AwardingOrganisationName = "test-ao-name",
                ProviderUkprn = 1123456789,
                LearnerName = "test-learner-name",
                AcademicYear = 2020
            };

            TrainingProviderLoader.ReinstateRegistrationFromPendingWithdrawalAsync(ViewModel).Returns(false);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).ReinstateRegistrationFromPendingWithdrawalAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            Result.Should().BeOfType(typeof(RedirectToRouteResult));
            
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}