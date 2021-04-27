using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsLrsQuestionPost
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public IActionResult Result { get; set; }
        protected AddLearnerRecordViewModel AddLearnerRecordViewModel;
        protected FindLearnerRecord LearnerRecord;
        protected EnterUlnViewModel EnterUlnViewModel;
        protected EnglishAndMathsLrsQuestionViewModel EnglishAndMathsLrsQuestionViewModel;
        protected AddLearnerRecordResponse AddLearnerRecordResponse;

        public async override Task When()
        {
            Result = await Controller.AddEnglishAndMathsLrsQuestionAsync(EnglishAndMathsLrsQuestionViewModel);
        }
    }
}
