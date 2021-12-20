using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetActiveAssessmentEntryDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private LearnerRecord _expectedApiLearnerResult;

        public override void Given()
        {
            _expectedApiLearnerResult = new LearnerRecord();
            ExpectedApiResult = null;

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, AssessmentId).Returns(_expectedApiLearnerResult);
            InternalApiClient.GetActiveAssessmentEntryDetailsAsync(AoUkprn, AssessmentId, componentType).Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
