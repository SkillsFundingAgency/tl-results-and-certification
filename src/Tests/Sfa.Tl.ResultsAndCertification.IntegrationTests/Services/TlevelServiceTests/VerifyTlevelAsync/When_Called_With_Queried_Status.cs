using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TlevelServiceTests.VerifyTlevelAsync
{
    public class When_Called_With_Queried_Status : TlevelServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation;
        private readonly TlevelReviewStatus _updatedTlevelReviewStatus = TlevelReviewStatus.Queried;
        private VerifyTlevelDetails _verifyTlevelDetailsModel;
        private bool _isSuccess;

        public override void Given()
        {
            SeedTlevelTestData();
            SeedNotificationTestData();

            _verifyTlevelDetailsModel = new VerifyTlevelDetails
            {
                TqAwardingOrganisationId = _tqAwardingOrganisation.Id,
                PathwayStatusId = (int)TlevelReviewStatus.Queried,
                Query = "test",
                QueriedUserEmail = "sender@test.com"
            };

            CreateService();
        }

        public async override Task When()
        {
            _isSuccess = await Service.VerifyTlevelAsync(_verifyTlevelDetailsModel);
        }

        [Fact]
        public void Then_Email_Sent_Successfully()
        {
            _isSuccess.Should().BeTrue();
        }

        [Fact]
        public void Then_Record_Does_Exist()
        {
            var result = Service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, (int)_updatedTlevelReviewStatus).Result;
            result.Should().ContainSingle();
        }

        protected override void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, _awardingOrganisation);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, _awardingOrganisation);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, _awardingOrganisation, _route);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _pathway, _tlAwardingOrganisation, _tlevelReviewStatus);
            DbContext.SaveChangesAsync();
            DetachEntity<TqAwardingOrganisation>();
        }

        private void SeedNotificationTestData()
        {
            NotificationDataProvider.CreateNotificationTemplates(DbContext);
            DbContext.SaveChangesAsync();
        }
    }
}
