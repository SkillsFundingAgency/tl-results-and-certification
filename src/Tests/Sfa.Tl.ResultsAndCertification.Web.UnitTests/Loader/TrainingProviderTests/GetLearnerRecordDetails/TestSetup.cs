using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetLearnerRecordDetails
{
    public abstract class TestSetup : TrainingProviderLoaderTestBase
    {
        protected long ProviderUkprn;
        protected int ProfileId;
        protected LearnerRecordDetailsViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(ProviderUkprn, ProfileId);
        }

        protected string GetLrsEnglishAndMathsStatusDisplayText
        {
            get
            {
                if (ActualResult == null || !ActualResult.HasLrsEnglishAndMaths)
                    return null;

                if (ActualResult.IsEnglishAndMathsAchieved && ActualResult.IsSendLearner == true)
                {
                    return LearnerRecordDetailsContent.English_And_Maths_Achieved_With_Send_Lrs_Text;
                }
                else
                {
                    return ActualResult.IsEnglishAndMathsAchieved && !ActualResult.IsSendLearner.HasValue
                        ? LearnerRecordDetailsContent.English_And_Maths_Achieved_Lrs_Text
                        : LearnerRecordDetailsContent.English_And_Maths_Not_Achieved_Lrs_Text;
                }
            }
        }
    }
}