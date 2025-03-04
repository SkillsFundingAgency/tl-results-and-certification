using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminSelectAwardingOrganisationGet
{
    public abstract class AdminSelectAwardingOrganisationBaseTest : AdminAwardingOrganisationControllerBaseTest
    {
        protected readonly AdminSelectAwardingOrganisationViewModel ViewModel = new()
        {
            AwardingOrganisations = GetAwardingOrganisations()
        };

        protected IActionResult Result { get; private set; }

        public override async Task When()
        {
            Result = await Controller.AdminSelectAwardingOrganisationAsync();
        }
    }
}