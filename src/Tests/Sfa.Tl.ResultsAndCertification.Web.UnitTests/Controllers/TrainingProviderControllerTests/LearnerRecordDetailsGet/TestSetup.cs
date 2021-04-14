using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public int ProfileId { get; set; }
        public IActionResult Result { get; private set; }
        public LearnerRecordDetailsViewModel LearnerRecordDetailsViewModel;
        protected LearnerRecordDetailsViewModel mockresult = null;

        public async override Task When()
        {
            Result = await Controller.LearnerRecordDetailsAsync(ProfileId);
        }

        protected string GetLrsEnglishAndMathsStatusDisplayText
        {
            get
            {
                if (!mockresult.HasLrsEnglishAndMaths)
                    return null;

                if (mockresult.IsEnglishAndMathsAchieved && mockresult.IsSendLearner == true)
                {
                    return LearnerRecordDetailsContent.English_And_Maths_Achieved_With_Send_Lrs_Text;
                }
                else
                {
                    return mockresult.IsEnglishAndMathsAchieved && !mockresult.IsSendLearner.HasValue
                        ? LearnerRecordDetailsContent.English_And_Maths_Achieved_Lrs_Text
                        : LearnerRecordDetailsContent.English_And_Maths_Not_Achieved_Lrs_Text;
                }
            }
        }
    }
}
