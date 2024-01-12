using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            AdminChangeIndustryPlacementViewModel = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 1,
                LearnerName = "firstname lastname",
                Uln = 1100000001,
                Provider = "provider-name (10000536)",
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                StartYear = "2021 to 2022",
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };
        }

        [Fact]
        public void Then_Redirected_To_AdminLearnerRecord()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(nameof(RouteConstants.AdminLearnerRecord));
            route.RouteValues[Constants.PathwayId].Should().Be(AdminChangeIndustryPlacementViewModel.RegistrationPathwayId);
        }
    }
}