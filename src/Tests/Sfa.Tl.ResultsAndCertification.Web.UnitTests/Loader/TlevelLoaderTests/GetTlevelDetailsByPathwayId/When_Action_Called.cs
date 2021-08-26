using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelDetailsByPathwayId
{
    public class When_Action_Called : TestSetup
    {
        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
            InternalApiClient.GetTlevelDetailsByPathwayIdAsync(Ukprn, Id).Returns(ApiClientResponse);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetTlevelDetailsByPathwayIdAsync(Ukprn, Id);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetTlevelDetailsByPathwayIdAsync(Ukprn, Id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.PathwayId.Should().Be(ExpectedResult.PathwayId);
            ActualResult.IsValid.Should().Be(ExpectedResult.IsValid);
        }
    }
}
