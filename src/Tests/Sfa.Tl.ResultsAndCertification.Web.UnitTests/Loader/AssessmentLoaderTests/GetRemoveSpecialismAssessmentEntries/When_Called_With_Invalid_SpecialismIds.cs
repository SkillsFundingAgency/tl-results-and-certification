using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetRemoveSpecialismAssessmentEntries
{
    public class When_Called_With_Invalid_SpecialismIds : TestSetup
    {
        public override void Given()
        {
            SpecialismAssessmentIds = "Hello|5";
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            ActualResult.Should().BeNull();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.DidNotReceive().GetLearnerRecordAsync(AoUkprn, ProfileId);
            InternalApiClient.DidNotReceive().GetActiveSpecialismAssessmentEntriesAsync(Arg.Any<long>(), Arg.Any<string>());
        }
    }
}
