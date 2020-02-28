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
    public class When_AwaitingConfirmation_Passed_One_Record_Returned : AwardingOrganisaionServiceBaseTest
    {

        private readonly int statusId = (int)TlevelReviewStatus.AwaitingConfirmation;
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
            result.Count().Should().Be(1);

            var expectedResult = result.FirstOrDefault();
            expectedResult.Should().NotBeNull();
            expectedResult.PathwayName.Should().Be(_pathway.Name);
            expectedResult.RouteName.Should().Be(_route.Name);
            expectedResult.StatusId.Should().Be(_tqAwardingOrganisation.ReviewStatus);
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
