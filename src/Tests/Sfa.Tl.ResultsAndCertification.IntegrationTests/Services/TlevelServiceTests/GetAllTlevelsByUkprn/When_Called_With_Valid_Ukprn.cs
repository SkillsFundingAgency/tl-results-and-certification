using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TlevelServiceTests.GetAllTlevelsByUkprnAsync
{
    public class When_Called_With_Valid_Ukprn : TlevelServiceBaseTest
    {
        public override void Given()
        {
            SeedTlevelTestData();
            CreateService();
        }

        public async override Task When()
        {
            _result = await Service.GetAllTlevelsByUkprnAsync(_tlAwardingOrganisation.UkPrn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeInAscendingOrder(x => x.TlevelTitle);

            var expectedResult = _result.FirstOrDefault();
            expectedResult.Should().NotBeNull();
            expectedResult.TlevelTitle.Should().Be(_pathway.TlevelTitle);
            expectedResult.StatusId.Should().Be(_tqAwardingOrganisation.ReviewStatus);
        }
    }
}
