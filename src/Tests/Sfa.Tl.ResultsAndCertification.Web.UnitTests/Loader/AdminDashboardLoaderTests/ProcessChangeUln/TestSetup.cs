using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.ProcessChangeUln
{
    public abstract class TestSetup : AdminDashboardLoaderTestsBase
    {
        protected ReviewChangeUlnViewModel Model;
        protected bool Result;

        public override void Given()
        {
            ApiClient.ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnRequest>()).Returns(true);
            Model = new ReviewChangeUlnViewModel
            {
                RegistrationPathwayId = 3, Uln = 9999999990, UlnTo = "9999999991",
                ContactName = "Contact", ChangeReason = "Correction", ZendeskId = "123",
                Day = "1", Month = "2", Year = "2026"
            };
        }

        public override async Task When()
        {
            Result = await Loader.ProcessChangeUlnAsync(Model);
        }
    }
}
