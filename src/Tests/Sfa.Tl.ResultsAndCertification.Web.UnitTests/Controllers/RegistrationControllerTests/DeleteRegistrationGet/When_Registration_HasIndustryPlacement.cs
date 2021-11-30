using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DeleteRegistrationGet
{
    public class When_Registration_HasIndustryPlacement : TestSetup
    {
        public override void Given()
        {
            var mockresult = new RegistrationAssessmentDetails { ProfileId = 99, IsCoreResultExist = false, IsIndustryPlacementExist = true };
            RegistrationLoader.GetRegistrationAssessmentAsync(Ukprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(Ukprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
