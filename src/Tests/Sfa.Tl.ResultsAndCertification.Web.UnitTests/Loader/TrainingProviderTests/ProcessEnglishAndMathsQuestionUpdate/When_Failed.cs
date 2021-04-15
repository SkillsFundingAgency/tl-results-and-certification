using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessEnglishAndMathsQuestionUpdate
{
    public class When_Failed : TestSetup
    {
        private LearnerRecordDetails _expectedApiResult;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;
            ProfileId = 1;

            _expectedApiResult = new LearnerRecordDetails { ProfileId = ProfileId, Uln = 1234567890, Name = "Test User", IsLearnerRecordAdded = true, HasLrsEnglishAndMaths = false, IsEnglishAndMathsAchieved = true, IsSendLearner = false };
            ViewModel = new UpdateEnglishAndMathsQuestionViewModel { ProfileId = ProfileId, EnglishAndMathsStatus = EnglishAndMathsStatus.AchievedWithSend };

            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
            InternalApiClient.UpdateLearnerRecordAsync(Arg.Is<UpdateLearnerRecordRequest>
                (x => x.ProfileId == ViewModel.ProfileId &&
                x.Ukprn == ProviderUkprn &&
                x.Uln == _expectedApiResult.Uln &&
                x.EnglishAndMathsStatus == ViewModel.EnglishAndMathsStatus &&
                x.PerformedBy == $"{Givenname} {Surname}"))
                .Returns(false);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
            InternalApiClient.Received(1).UpdateLearnerRecordAsync(Arg.Is<UpdateLearnerRecordRequest>
                (x => x.ProfileId == ViewModel.ProfileId &&
                x.Ukprn == ProviderUkprn &&
                x.Uln == _expectedApiResult.Uln &&
                x.EnglishAndMathsStatus == ViewModel.EnglishAndMathsStatus &&
                x.PerformedBy == $"{Givenname} {Surname}"));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsModified.Should().BeTrue();
            ActualResult.IsSuccess.Should().BeFalse();

            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Name.Should().Be(_expectedApiResult.Name);
        }
    }
}
