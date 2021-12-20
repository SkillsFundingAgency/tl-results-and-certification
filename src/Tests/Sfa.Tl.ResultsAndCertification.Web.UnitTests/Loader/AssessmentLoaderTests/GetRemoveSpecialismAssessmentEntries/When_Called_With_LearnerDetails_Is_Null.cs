using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetRemoveSpecialismAssessmentEntries
{
    public class When_Called_With_LearnerDetails_Is_Null : TestSetup
    {
        private LearnerRecord _expectedApiLearnerResult;

        public override void Given()
        {
            _expectedApiLearnerResult = null;
            SpecialismAssessmentIds = "4|5";
            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(_expectedApiLearnerResult);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            ActualResult.Should().BeNull();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).GetLearnerRecordAsync(AoUkprn, ProfileId);
            InternalApiClient.DidNotReceive().GetActiveSpecialismAssessmentEntriesAsync(Arg.Any<long>(), Arg.Any<string>());
        }
    }
}
