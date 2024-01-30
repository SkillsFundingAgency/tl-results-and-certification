using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminIpCompletionViewModel ViewModel;

        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminChangeIndustryPlacementAsync(ViewModel);
        }

        protected AdminIpCompletionViewModel CreateViewModel(IndustryPlacementStatus? industryPlacementStatus)
        {
            return new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 100,
                LearnerName = "Kevin Smith",
                Uln = 1234567890,
                Provider = "Barnsley College (10000536)",
                TlevelName = "Education and Early Years",
                AcademicYear = 2020,
                StartYear = "2021 to 2022",
                IndustryPlacementStatus = industryPlacementStatus,
                IndustryPlacementStatusTo = industryPlacementStatus
            };
        }
    }
}
