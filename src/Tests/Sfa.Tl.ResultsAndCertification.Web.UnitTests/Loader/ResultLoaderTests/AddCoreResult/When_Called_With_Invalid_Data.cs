using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.AddCoreResult
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            InternalApiClient.AddResultAsync(Arg.Any<AddResultRequest>()).Returns(ActualResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
