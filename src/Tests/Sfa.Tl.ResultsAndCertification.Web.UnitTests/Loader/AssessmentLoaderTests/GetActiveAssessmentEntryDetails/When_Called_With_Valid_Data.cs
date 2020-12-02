using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetActiveAssessmentEntryDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new AssessmentEntryDetails
            {
                ProfileId = 1,
                AssessmentId = 5,
                AssessmentSeriesName = "Summer 2021"
            };

            InternalApiClient.GetActiveAssessmentEntryDetailsAsync(AoUkprn, ProfileId, assessmentEntryType).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.AssessmentId.Should().Be(expectedApiResult.AssessmentId);
            ActualResult.AssessmentSeriesName.Should().Be(expectedApiResult.AssessmentSeriesName);
        }
    }
}
