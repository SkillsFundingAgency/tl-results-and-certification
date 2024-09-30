using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearPost
{
    public class When_Success : TestSetup
    {
        private AdminChangeStartYearViewModel MockResult = null;

        public override void Given()
        {
            AdminChangeStartYearViewModel = new AdminChangeStartYearViewModel
            {
                RegistrationPathwayId = 1,
            };

            MockResult = new AdminChangeStartYearViewModel
            {
                RegistrationPathwayId = 1,
                FirstName = "John",
                LastName = "Smith",
                Uln = 1100000001,
                ProviderName = "Barnsley College",
                ProviderUkprn = 10000536,
                TlevelName = "T Level in Digital Support Services",
                TlevelStartYear = 2021,
                AcademicYear = 2022,
                DisplayAcademicYear = "2021 to 2022",
                AcademicStartYearsToBe = new List<int>() { 2020 }
            };

            AdminDashboardLoader.GetAdminLearnerRecordChangeYearAsync(Arg.Any<int>()).Returns(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_AssessmentDetails()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(nameof(RouteConstants.ReviewChangeStartYear));
            route.RouteValues[Constants.PathwayId].Should().Be(AdminChangeStartYearViewModel.RegistrationPathwayId);
        }
    }
}
