using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmitPost
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        public int ProfileId { get; set; }
        public IActionResult Result { get; set; }
        protected SoaLearnerRecordDetailsViewModel SoaLearnerRecordDetailsViewModel;
        protected SoaPrintingResponse SoaPrintingResponse;

        public async override Task When()
        {
            Result = await Controller.SubmitRequestSoaCheckAndSubmitAsync(SoaLearnerRecordDetailsViewModel);
        }
    }
}
