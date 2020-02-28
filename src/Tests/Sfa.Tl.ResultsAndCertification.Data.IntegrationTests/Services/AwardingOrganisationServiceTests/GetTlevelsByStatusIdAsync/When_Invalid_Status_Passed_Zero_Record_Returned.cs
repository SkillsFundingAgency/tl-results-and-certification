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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetTlevelsByStatusIdAsync
{
    public class When_Invalid_Status_Passed_Zero_Record_Returned : AwardingOrganisaionServiceBaseTest
    {
        private readonly int statusId = (int)TlevelReviewStatus.Queried;
        private IEnumerable<AwardingOrganisationPathwayStatus> result;

        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(Repository, _mapper, _logger);
        }

        public override void When()
        {
            result = _service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, statusId).Result;
        }

        [Fact(Skip = "TODO")]
        public void Then_Expected_Results_Is_Returned()
        {
            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        protected override void SeedTlevelTestData()
        {
            // TODO:
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Pearson);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Pearson);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Pearson);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, EnumAwardingOrganisation.Pearson);
            DbContext.SaveChangesAsync();
        }

    }
}
