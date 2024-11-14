using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminRequestReplacementDocumentPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminRequestReplacementDocumentViewModel ViewModel = new()
        {
            RegistrationPathwayId = 1,
            ProviderUkprn = 10000536,
            ProviderAddress = new AddressViewModel
            {
                AddressId = 5000
            },
            PrintCertificateId = 4575
        };

        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminRequestReplacementDocumentAsync(ViewModel);
        }
    }
}