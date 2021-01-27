using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetAddCoreResultViewModel
{
    public class When_Assessment_NotFound : TestSetup
    {
        public override void Given()
        {
            expectedApiResultDetails = new Models.Contracts.ResultDetails { PathwayAssessmentId = 999 };
            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
