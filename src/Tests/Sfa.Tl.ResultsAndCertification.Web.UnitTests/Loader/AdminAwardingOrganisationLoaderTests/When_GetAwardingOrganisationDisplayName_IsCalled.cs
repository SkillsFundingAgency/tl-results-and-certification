using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminAwardingOrganisationLoaderTests
{
    public class When_GetAwardingOrganisationDisplayName_IsCalled : AdminAwardingOrganisationLoaderBaseTest
    {
        private const long Ukprn = 10009931;

        private readonly AwardingOrganisationMetadata _apiResult = new()
        {
            Id = 3,
            Ukprn = Ukprn,
            DisplayName = "City & Guilds"
        };

        private string _result;

        public override void Given()
        {
            ApiClient.GetAwardingOrganisationByUkprnAsync(Ukprn).Returns(_apiResult);
        }

        public override async Task When()
        {
            _result = await Loader.GetAwardingOrganisationDisplayName(Ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().Be(_apiResult.DisplayName);
        }
    }
}