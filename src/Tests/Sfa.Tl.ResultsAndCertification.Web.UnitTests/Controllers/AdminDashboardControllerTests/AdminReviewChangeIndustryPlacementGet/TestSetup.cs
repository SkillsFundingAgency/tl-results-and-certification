using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {   
        protected AdminReviewChangesIndustryPlacementViewModel ReviewChangesIndustryPlacementViewModel;
        public IActionResult Result { get; private set; }
        protected int PathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.AdminReviewChangesIndustryPlacementAsync(PathwayId);
        }
    }
}
