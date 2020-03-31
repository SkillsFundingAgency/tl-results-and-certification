using System.Linq;
using System.Collections.Generic;
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
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetTlevelsByStatusIdAsync
{
    public class When_AwaitingConfirmation_Passed_One_Record_Returned : AwardingOrganisaionServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation;
        private IEnumerable<AwardingOrganisationPathwayStatus> result;

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
            result.Count().Should().Be(1);

            var expectedResult = result.FirstOrDefault();
            expectedResult.Should().NotBeNull();
            expectedResult.PathwayName.Should().Be(_pathway.Name);
            expectedResult.RouteName.Should().Be(_route.Name);
            expectedResult.StatusId.Should().Be(_tqAwardingOrganisation.ReviewStatus);
        }

        protected override void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, _awardingOrganisation);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, _awardingOrganisation);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, _awardingOrganisation, _route);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _route, _pathway, _tlAwardingOrganisation, _tlevelReviewStatus);
            DbContext.SaveChangesAsync();
        }
    }
}
