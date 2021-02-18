using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DeleteRegistrationGet
{
    public class When_Registration_HasResult : TestSetup
    {
        public override void Given()
        {
            var mockresult = new AssessmentDetailsViewModel { ProfileId = 99, IsResultExist = true };
            RegistrationLoader.GetRegistrationAssessmentAsync(Ukprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(Ukprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
