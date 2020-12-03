using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.AddAssessmentEntry
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            InternalApiClient.AddAssessmentEntryAsync(Arg.Any<AddAssessmentEntryRequest>())
                .Returns(ActualResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
