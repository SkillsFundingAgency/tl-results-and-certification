using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAddAssessmentEntry
{
    public class When_Called_With_Invalid_Learner : TestSetup
    {
        private LearnerRecord _expectedApiLearnerResult;
        private AvailableAssessmentSeries _expectedApiAvailableAssessmentSeries;

        public override void Given()
        {
            _expectedApiLearnerResult = null;
            _expectedApiAvailableAssessmentSeries = new AvailableAssessmentSeries();

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(_expectedApiLearnerResult);
            InternalApiClient.GetAvailableAssessmentSeriesAsync(AoUkprn, ProfileId, ComponentType.Core).Returns(_expectedApiAvailableAssessmentSeries);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
