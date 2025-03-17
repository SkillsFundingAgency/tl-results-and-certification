using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetAwardingOrganisationByUkprn
{
    public class When_Ukprn_Exists_Returns_Expected : AwardingOrganisationServiceBaseTest
    {
        private AwardingOrganisationMetadata _awardingOrganisation;

        public async override Task When()
        {
            _awardingOrganisation = await Service.GetAwardingOrganisationByUkprnAsync(Pearson.UkPrn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _awardingOrganisation.Should().NotBeNull();

            _awardingOrganisation.Id.Should().Be(Pearson.Id);
            _awardingOrganisation.Ukprn.Should().Be(Pearson.UkPrn);
            _awardingOrganisation.DisplayName.Should().Be(Pearson.DisplayName);
        }
    }
}