using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetAwardingOrganisationByUkprn
{
    public class When_Ukprn_Doesnt_Exists_Returns_Null : AwardingOrganisationServiceBaseTest
    {
        private AwardingOrganisationMetadata _awardingOrganisation;

        public async override Task When()
        {
            _awardingOrganisation = await Service.GetAwardingOrganisationByUkprnAsync(99999);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _awardingOrganisation.Should().BeNull();
        }
    }
}