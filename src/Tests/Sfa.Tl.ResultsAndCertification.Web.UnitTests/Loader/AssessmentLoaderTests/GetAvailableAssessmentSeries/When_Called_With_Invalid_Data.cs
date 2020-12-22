using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAvailableAssessmentSeries
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetAvailableAssessmentSeriesAsync(AoUkprn, ProfileId, assessmentEntryType).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
