using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PrintingServiceTests
{
    public class When_GetPendingItemsForTrackBatchAsync_IsCalled : PrintingServiceBaseTest
    {
        private IList<int> _actualResult;
        private IList<int> _expectedBatchIds;

        // Seed data variables
        private List<TqRegistrationProfile> _profiles;
        private List<PrintCertificate> _printCertificates;

        public override void Given()
        {
            CreateMapper();

            _profiles = new List<TqRegistrationProfile>();
            var ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Withdrawn },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Withdrawn },
                { 1111111114, RegistrationPathwayStatus.Withdrawn },
                { 1111111115, RegistrationPathwayStatus.Withdrawn },
            };

            // Seed Registrations
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            foreach (var uln in ulns)
                _profiles.Add(SeedRegistrationDataByStatus(uln.Key, uln.Value, TqProvider));

            // Seed PrintCertificate
            _printCertificates = new List<PrintCertificate>();

            foreach (var profile in _profiles)
                _printCertificates.Add(SeedPrintCertificate(profile.TqRegistrationPathways.FirstOrDefault()));

            var batchStatusItemsToSeed = new List<(int, BatchStatus, PrintingStatus?, PrintingBatchItemStatus?)>
            {
                ( 1, BatchStatus.Error, null, null),
                ( 2, BatchStatus.Accepted, PrintingStatus.AwaitingCollectionByCourier, null),
                ( 3, BatchStatus.Accepted, PrintingStatus.CollectedByCourier, PrintingBatchItemStatus.Delivered),
                ( 4, BatchStatus.Accepted, PrintingStatus.CollectedByCourier, PrintingBatchItemStatus.NotDelivered),
                ( 5, BatchStatus.Accepted, PrintingStatus.CollectedByCourier, null),
            };

            foreach (var batchStatusItem in batchStatusItemsToSeed)
            {
                var batch = _printCertificates.FirstOrDefault(p => p.PrintBatchItem.Batch.Id == batchStatusItem.Item1)?.PrintBatchItem.Batch;
                batch.Status = batchStatusItem.Item2;
                batch.PrintingStatus = batchStatusItem.Item3;
                batch.PrintBatchItems.ToList().ForEach(p => p.Status = batchStatusItem.Item4);
                DbContext.SaveChanges();
            }

            _expectedBatchIds = batchStatusItemsToSeed.Where(b => b.Item2 == BatchStatus.Accepted && b.Item3 == PrintingStatus.CollectedByCourier && b.Item4 != PrintingBatchItemStatus.Delivered && b.Item4 != PrintingBatchItemStatus.NotDelivered).Select(x => x.Item1).ToList();

            BatchRepositoryLogger = new Logger<GenericRepository<Batch>>(new NullLoggerFactory());
            BatchRepository = new GenericRepository<Batch>(BatchRepositoryLogger, DbContext);

            PrintBatchItemRepositoryLogger = new Logger<GenericRepository<PrintBatchItem>>(new NullLoggerFactory());
            PrintBatchItemRepository = new GenericRepository<PrintBatchItem>(PrintBatchItemRepositoryLogger, DbContext);

            PrintingRepositoryLogger = new Logger<IPrintingRepository>(new NullLoggerFactory());
            PrintingRepository = new PrintingRepository(DbContext, PrintingRepositoryLogger);

            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            NotificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            NotificationTemplateRepository = new GenericRepository<NotificationTemplate>(NotificationTemplateRepositoryLogger, DbContext);
            NotificationService = new NotificationService(NotificationTemplateRepository, NotificationsClient, NotificationLogger);

            Configuration = new ResultsAndCertificationConfiguration
            {
                TlevelQueriedSupportEmailAddress = "test@test.com"
            };

            PrintingServiceLogger = new Logger<PrintingService>(new NullLoggerFactory());
            PrintingService = new PrintingService(PrintingServiceMapper, PrintingServiceLogger, BatchRepository, PrintBatchItemRepository, PrintingRepository, NotificationService, Configuration);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            if (_actualResult != null)
                return;

            _actualResult = await PrintingService.GetPendingItemsForTrackBatchAsync();
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();

            _actualResult.Should().NotBeNullOrEmpty();
            _actualResult.Count.Should().Be(_expectedBatchIds.Count());
            _actualResult.Should().BeEquivalentTo(_expectedBatchIds);
        }
    }
}
