using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveStatusHaveYouToldAwardingOrganisationGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ProfileId = 0;

            TrainingProviderLoader
                .GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel>(ProviderUkprn, ProfileId)
                .Returns(null as ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
