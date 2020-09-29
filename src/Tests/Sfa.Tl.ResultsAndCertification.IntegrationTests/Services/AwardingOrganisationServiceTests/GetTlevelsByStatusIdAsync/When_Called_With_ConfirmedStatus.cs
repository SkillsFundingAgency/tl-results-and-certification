using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetTlevelsByStatusIdAsync
{
    public class When_Called_With_ConfirmedStatus : AwardingOrganisaionServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.Confirmed;
        private IEnumerable<AwardingOrganisationPathwayStatus> result;

        protected IList<TlRoute> _routes;
        protected IList<TlPathway> _pathways;
        protected IList<TqAwardingOrganisation> _tqAwardingOrganisations;

        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(_resultsAndCertificationConfiguration, Repository, null, _mapper, _logger);
        }

        public async override Task When()
        {
            result = await _service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, (int)_tlevelReviewStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            result.Should().NotBeNull();
            result.Count().Should().Be(2);

            _tqAwardingOrganisations.ToList().ForEach(tq =>
            {
                var expectedResult = result.FirstOrDefault(x => x.PathwayId == tq.TlPathwayId);
                expectedResult.Should().NotBeNull();
                expectedResult.TlevelTitle.Should().Be(tq.TlPathway.TlevelTitle);
                expectedResult.StatusId.Should().Be(tq.ReviewStatus);
            });            
        }

        protected override void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, _awardingOrganisation);
            _routes = TlevelDataProvider.CreateTlRoutes(DbContext, _awardingOrganisation);
            _pathways = TlevelDataProvider.CreateTlPathways(DbContext, _awardingOrganisation, _routes);
            _tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, _awardingOrganisation, _tlAwardingOrganisation, _pathways, _tlevelReviewStatus);
            DbContext.SaveChangesAsync();
        }
    }
}
