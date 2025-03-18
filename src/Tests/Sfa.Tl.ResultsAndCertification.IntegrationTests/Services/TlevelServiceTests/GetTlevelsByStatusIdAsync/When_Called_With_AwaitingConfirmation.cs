using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TlevelServiceTests.GetTlevelsByStatusIdAsync
{
    public class When_Called_With_AwaitingConfirmation : TlevelServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation;

        public override void Given()
        {
            SeedTlevelTestData();
            CreateService();
        }

        public async override Task When()
        {
            _result = await Service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, (int)_tlevelReviewStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().ContainSingle();

            var expectedResult = _result.Single();
            expectedResult.Should().NotBeNull();
            expectedResult.TlevelTitle.Should().Be(_pathway.TlevelTitle);
            expectedResult.StatusId.Should().Be(_tqAwardingOrganisation.ReviewStatus);
        }

        protected override void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, _awardingOrganisation);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, _awardingOrganisation);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, _awardingOrganisation, _route);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _pathway, _tlAwardingOrganisation, _tlevelReviewStatus);
            DbContext.SaveChangesAsync();
        }
    }
}