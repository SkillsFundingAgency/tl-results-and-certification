using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.RemoveTqProviderTlevel
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
