using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessEnglishAndMathsQuestionUpdate
{
    public class When_EnglishAndMathsStatus_Unchanged : TestSetup
    {
        private LearnerRecordDetails _expectedApiResult;

        public override void Given()
        {
            ProviderUkprn = 87945612;
            ProfileId = 1;
            _expectedApiResult = new LearnerRecordDetails { ProfileId = 1, IsLearnerRecordAdded = true, HasLrsEnglishAndMaths = false, IsEnglishAndMathsAchieved = true, IsSendLearner = false };
            ViewModel = new UpdateEnglishAndMathsQuestionViewModel { ProfileId = 1, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved };
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsModified.Should().BeFalse();
        }
    }
}
