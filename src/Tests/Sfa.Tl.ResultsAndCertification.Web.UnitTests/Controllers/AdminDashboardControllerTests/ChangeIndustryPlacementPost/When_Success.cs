using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_Success : TestSetup
    {
        private AdminChangeIndustryPlacementViewModel MockResult = null;
        public override void Given()
        {
            AdminChangeIndustryPlacementViewModel = new AdminChangeIndustryPlacementViewModel()
            {
                PathwayId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Uln = 1100000001,
                ProviderName = "provider-name",
                ProviderUkprn = 10000536,
                TlevelName = "t-level-name",
                AcademicYear = 2022,
                DisplayAcademicYear = "2021 to 2022",
                IndustryPlacementStatus = IndustryPlacementStatus.Completed

            };
        }


        [Fact]
        public void Then_Redirected_To_AdminLearnerRecord()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(nameof(RouteConstants.AdminLearnerRecord));
            route.RouteValues[Constants.PathwayId].Should().Be(AdminChangeIndustryPlacementViewModel.PathwayId);
        }
    }
}