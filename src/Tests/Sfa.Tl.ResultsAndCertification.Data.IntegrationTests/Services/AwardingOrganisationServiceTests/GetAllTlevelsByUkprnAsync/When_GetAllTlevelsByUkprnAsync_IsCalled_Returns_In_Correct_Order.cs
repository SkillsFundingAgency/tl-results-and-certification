using System.Linq;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetAllTlevelsByUkprnAsync
{
    public class When_GetAllTlevelsByUkprnAsync_IsCalled_Returns_In_Correct_Order : AwardingOrganisaionServiceBaseTest
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

        public override void When()
        {
            _result = _service.GetAllTlevelsByUkprnAsync(_tlAwardingOrganisation.UkPrn).Result;
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned_In_Ascending_Order()
        {
            _result.Should().NotBeNull();
            _result.Should().BeInAscendingOrder(x => x.TlevelTitle);
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
