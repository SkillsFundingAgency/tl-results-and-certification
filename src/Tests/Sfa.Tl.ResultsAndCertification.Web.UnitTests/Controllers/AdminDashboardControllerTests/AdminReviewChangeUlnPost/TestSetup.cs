using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeUlnPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected ReviewChangeUlnViewModel Model;

        public IActionResult Result { get; private set; }

        public override async Task When()
        {
            Result = await Controller.ReviewChangeUlnAsync(3, Model);
        }

        protected AdminChangeUlnViewModel Entry() => new()
        {
            RegistrationPathwayId = 3, Uln = 9999999990, UlnTo = "9999999991"
        };

        protected ReviewChangeUlnViewModel Learner() => new()
        {
            RegistrationPathwayId = 3, Uln = 9999999990, UlnTo = "9999999991",
            FirstName = "Susan", LastName = "Altman", ContactName = "Contact",
            Day = "1", Month = "1", Year = "2026", ChangeReason = "Correction"
        };
    }
}
