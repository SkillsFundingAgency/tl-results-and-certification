using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetAllAwardingOrganisations
{
    public class When_Called_Returns_Expected : AwardingOrganisationServiceBaseTest
    {
        private IEnumerable<AwardingOrganisationMetadata> _result;

        public async override Task When()
        {
            _result = await Service.GetAllAwardingOrganisationsAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().HaveCount(2);

            AwardingOrganisationMetadata first = _result.ElementAt(0);
            first.Id.Should().Be(Ncfe.Id);
            first.Ukprn.Should().Be(Ncfe.UkPrn);
            first.DisplayName.Should().Be(Ncfe.DisplayName);

            AwardingOrganisationMetadata second = _result.ElementAt(1);
            second.Id.Should().Be(Pearson.Id);
            second.Ukprn.Should().Be(Pearson.UkPrn);
            second.DisplayName.Should().Be(Pearson.DisplayName);
        }
    }
}