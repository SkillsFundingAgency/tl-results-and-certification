using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TlevelServiceTests.GetAllTlevelsByUkprnAsync
{
    public class When_Called_With_Invalid_Ukprn : TlevelServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.Confirmed;

        public override void Given()
        {
            SeedTlevelTestData();
            CreateService();
        }

        public async override Task When()
        {
            _result = await Service.GetAllTlevelsByUkprnAsync(54546373);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeNullOrEmpty();
        }

        protected override void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, _awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, _awardingOrganisation);
            var pathways = TlevelDataProvider.CreateTlPathways(DbContext, _awardingOrganisation, routes);
            TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, _awardingOrganisation, _tlAwardingOrganisation, pathways, _tlevelReviewStatus);
            DbContext.SaveChangesAsync();
        }
    }
}
