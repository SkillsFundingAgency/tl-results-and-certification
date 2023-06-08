using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeWithdrawnStatusHaveYouToldAwardingOrganisationPost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            ProfileId = 1;

            ViewModel = new ChangeWithdrawnStatusHaveYouToldAwardingOrganisationViewModel
            {
                HaveYouToldAwardingOrganisation = true,
                ProfileId = 1,
                AwardingOrganisationName = "test-ao-name",
                ProviderUkprn = 1123456789,
                LearnerName = "test-learner-name",
                AcademicYear = 2020
            };

            TrainingProviderLoader.UpdateLearnerWithdrawnStatusAsync(ProviderUkprn, ViewModel).Returns(false);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).UpdateLearnerWithdrawnStatusAsync(ProviderUkprn, ViewModel);
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
