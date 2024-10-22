using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminRequestReplacementDocumentGet
{
    public class When_Called_With_Non_Existing_Registration: TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader.
                GetAdminLearnerRecordAsync<AdminRequestReplacementDocumentViewModel>(RegistrationPathwayId)
                .Returns(null as AdminRequestReplacementDocumentViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}