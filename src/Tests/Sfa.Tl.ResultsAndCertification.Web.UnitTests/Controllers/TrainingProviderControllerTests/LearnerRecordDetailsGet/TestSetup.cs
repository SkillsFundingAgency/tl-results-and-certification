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
        public LearnerRecordDetailsViewModel1 LearnerRecordDetailsViewModel;
        protected LearnerRecordDetailsViewModel1 Mockresult = null;

        public async override Task When()
        {
            Result = await Controller.LearnerRecordDetailsAsync(ProfileId);
        }

        protected string GetLrsEnglishAndMathsStatusDisplayText
        {
            get
            {
                return string.Empty;

                //if (!Mockresult.HasLrsEnglishAndMaths)
                //    return null;

                //if (Mockresult.IsEnglishAndMathsAchieved && Mockresult.IsSendLearner == true)
                //{
                //    return LearnerRecordDetailsContent.English_And_Maths_Achieved_With_Send_Lrs_Text;
                //}
                //else
                //{
                //    return Mockresult.IsEnglishAndMathsAchieved && !Mockresult.IsSendLearner.HasValue
                //        ? LearnerRecordDetailsContent.English_And_Maths_Achieved_Lrs_Text
                //        : LearnerRecordDetailsContent.English_And_Maths_Not_Achieved_Lrs_Text;
                //}
            }
        }
    }
}
