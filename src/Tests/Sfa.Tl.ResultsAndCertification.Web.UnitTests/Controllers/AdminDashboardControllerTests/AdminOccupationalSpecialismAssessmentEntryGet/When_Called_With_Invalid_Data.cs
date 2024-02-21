using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminOccupationalSpecialismAssessmentEntry
{
    public class When_Called_With_Invalid_Data: TestSetup
    {
        protected AdminOccupationalSpecialismViewModel Mockresult = null;
        public override void Given()
        {
            RegistrationPathwayId = 0;
            AdminDashboardLoader.GetAdminLearnerRecordWithOccupationalSpecialism(RegistrationPathwayId, SpecialismId).Returns(Mockresult);
       }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordWithOccupationalSpecialism(RegistrationPathwayId, SpecialismId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
