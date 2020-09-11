using System.Linq;
using Xunit;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.VerifyTlevelAsync
{
    public class When_Called_With_Queried_Status : AwardingOrganisaionServiceBaseTest
    {        
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private readonly TlevelReviewStatus _tlevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation;
        private readonly TlevelReviewStatus _updatedTlevelReviewStatus = TlevelReviewStatus.Queried;
        private VerifyTlevelDetails _verifyTlevelDetailsModel;
        private bool _isSuccess;
        private NotificationService _notificationService;
        private NotificationTemplate _notificationTemplate;
        private IAsyncNotificationClient _notificationsClient;
        private ILogger<NotificationService> _notificationLogger;

        public override void Given()
        {
            SeedTlevelTestData();
            SeedNotificationTestData();
            CreateMapper();

            _verifyTlevelDetailsModel = new VerifyTlevelDetails
            {
                TqAwardingOrganisationId = _tqAwardingOrganisation.Id,
                PathwayStatusId = (int)TlevelReviewStatus.Queried,
                Query = "test",
                QueriedUserEmail = "sender@test.com"
            };

            _resultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                TlevelQueriedSupportEmailAddress = "test@test.com"
            };
            
            _notificationsClient = Substitute.For<IAsyncNotificationClient>();
            _notificationLogger = Substitute.For<ILogger<NotificationService>>();
            var notificationTemplateRepositoryLogger = Substitute.For<ILogger<GenericRepository<NotificationTemplate>>>();
            IRepository<NotificationTemplate> notificationTemplateRepository = new GenericRepository<NotificationTemplate>(notificationTemplateRepositoryLogger, DbContext);
            _notificationService = new NotificationService(notificationTemplateRepository, _notificationsClient, _notificationLogger);

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(_resultsAndCertificationConfiguration, Repository, _notificationService, _mapper, _logger);
        }

        public override void When()
        {
            _isSuccess = _service.VerifyTlevelAsync(_verifyTlevelDetailsModel).Result;
        }

        [Fact]
        public void Then_Email_Sent_Successfully()
        {
            _isSuccess.Should().BeTrue();
        }

        [Fact]
        public void Then_Record_Does_Exist()
        {
            var result = _service.GetTlevelsByStatusIdAsync(_tlAwardingOrganisation.UkPrn, (int)_updatedTlevelReviewStatus).Result;
            result.Should().NotBeNull();
            result.Count().Should().Be(1);
        }

        protected override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(AwardingOrganisationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("DateTimeResolver") ?
                                new DateTimeResolver<VerifyTlevelDetails, TqAwardingOrganisation>(new DateTimeProvider()) :
                                null);
            });
            _mapper = new Mapper(mapperConfig);
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
            DbContext.SaveChangesAsync();
        }
    }
}
