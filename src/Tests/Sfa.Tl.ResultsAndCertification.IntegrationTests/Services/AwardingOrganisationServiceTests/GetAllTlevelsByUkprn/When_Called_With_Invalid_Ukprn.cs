using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetAllTlevelsByUkprnAsync
{
    public class When_Called_With_Invalid_Ukprn : AwardingOrganisaionServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.Confirmed;

        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(_resultsAndCertificationConfiguration, Repository, null, _mapper, _logger);
        }

        public async override Task When()
        {
            _result = await _service.GetAllTlevelsByUkprnAsync(54546373);
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
