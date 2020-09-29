using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ConfirmTlevel
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            Mapper.Received(1).Map<VerifyTlevelDetails>(ConfirmTlevelViewModel);
            InternalApiClient.Received(1).VerifyTlevelAsync(VerifyTlevelDetails);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
