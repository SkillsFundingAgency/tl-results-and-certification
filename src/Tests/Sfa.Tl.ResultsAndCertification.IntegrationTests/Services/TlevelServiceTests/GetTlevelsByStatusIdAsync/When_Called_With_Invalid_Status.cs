﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TlevelServiceTests.GetTlevelsByStatusIdAsync
{
    public class When_Called_With_Invalid_Status : TlevelServiceBaseTest
    {
        private readonly int statusId = (int)TlevelReviewStatus.Queried;
        private IEnumerable<AwardingOrganisationPathwayStatus> result;

        public override void Given()
        {
            SeedTlevelTestData();
            CreateService();
        }

        public async override Task When()
        {
            result = await Service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, statusId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        protected override void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Pearson);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Pearson);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Pearson, _route);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _pathway, _tlAwardingOrganisation, TlevelReviewStatus.AwaitingConfirmation);
            DbContext.SaveChangesAsync();
        }
    }
}