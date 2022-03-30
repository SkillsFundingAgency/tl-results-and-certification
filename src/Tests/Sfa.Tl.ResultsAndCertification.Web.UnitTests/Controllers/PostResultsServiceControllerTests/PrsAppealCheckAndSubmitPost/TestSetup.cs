using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCheckAndSubmitPost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public PrsAppealCheckAndSubmitViewModel ViewModel;
        public IActionResult Result;

        public async override Task When()
        {
            Result = await Controller.PrsAppealCheckAndSubmitAsync(ViewModel);
        }
    }
}