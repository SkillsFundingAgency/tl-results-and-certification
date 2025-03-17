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
    public class When_Called_With_Queried_Status_Email_Failed : TlevelServiceBaseTest
    {
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation;
        private VerifyTlevelDetails _verifyTlevelDetailsModel;
        private bool _isSuccess;
        private NotificationTemplate _notificationTemplate;

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
        public void Then_Email_Not_Sent_Due_To_Wrong_Template_Name()
        {
            _isSuccess.Should().BeFalse();
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
            _notificationTemplate = NotificationDataProvider.CreateNotificationTemplate(DbContext);
            _notificationTemplate.TemplateName = "WrongTemplate";
            DbContext.SaveChangesAsync();
        }
    }
}