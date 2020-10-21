using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetAllTlevelsByUkprnAsync
{
    public class When_Called_With_Valid_Ukprn : AwardingOrganisaionServiceBaseTest
    {
        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(_resultsAndCertificationConfiguration, Repository, null, _mapper, _logger);
        }

        public async override Task When()
        {
            _result = await _service.GetAllTlevelsByUkprnAsync(_tlAwardingOrganisation.UkPrn);
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
