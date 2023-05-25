using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SubmitWithdrawnStatusPost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            ProfileId = 1;

            ViewModel = new WithdrawnConfirmationViewModel
            {
                AwardingOrganisationName = "awarding-organisation-name",
                IsPendingWithdrawl = true,
                IsWithdrawnConfirmed = true,
                ProfileId = 1
            };

            TrainingProviderLoader.UpdateLearnerWithdrawnStatusAsync(ProviderUkprn, ViewModel).Returns(false);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).UpdateLearnerWithdrawnStatusAsync(ProviderUkprn, ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.Should().BeOfType(typeof(RedirectToRouteResult));

            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
