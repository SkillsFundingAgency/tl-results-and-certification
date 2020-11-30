using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAvailableAssessmentSeries
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new AvailableAssessmentSeries
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021"
            };

            InternalApiClient.GetAvailableAssessmentSeriesAsync(AoUkprn, ProfileId, assessmentEntryType).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.AssessmentSeriesId.Should().Be(expectedApiResult.AssessmentSeriesId);
            ActualResult.AssessmentSeriesName.Should().Be(expectedApiResult.AssessmentSeriesName);
        }
    }
}
