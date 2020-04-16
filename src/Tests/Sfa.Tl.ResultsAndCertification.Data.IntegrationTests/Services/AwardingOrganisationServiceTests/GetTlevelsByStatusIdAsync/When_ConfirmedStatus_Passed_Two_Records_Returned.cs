using System.Collections.Generic;
using System.Linq;
using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetTlevelsByStatusIdAsync
{
    public class When_ConfirmedStatus_Passed_Two_Records_Returned : AwardingOrganisaionServiceBaseTest
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

        public override void When()
        {
            result = _service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, (int)_tlevelReviewStatus).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
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
