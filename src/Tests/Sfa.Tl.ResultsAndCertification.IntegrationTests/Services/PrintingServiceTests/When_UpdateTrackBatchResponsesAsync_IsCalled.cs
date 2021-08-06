using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PrintingServiceTests
{
    public class When_UpdateTrackBatchResponsesAsync_IsCalled : PrintingServiceBaseTest
    {
        private CertificatePrintingResponse _actualResult;

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
                { 1111111115, RegistrationPathwayStatus.Withdrawn }
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
                ( 3, BatchStatus.Accepted, PrintingStatus.CollectedByCourier, null),
                ( 4, BatchStatus.Accepted, PrintingStatus.CollectedByCourier, null),
                ( 5, BatchStatus.Accepted, PrintingStatus.CollectedByCourier, null),
            };

            foreach (var batchStatusItem in batchStatusItemsToSeed)
            {
                var batch = _printCertificates.FirstOrDefault(p => p.PrintBatchItem.Batch.Id == batchStatusItem.Item1)?.PrintBatchItem.Batch;
                batch.Status = batchStatusItem.Item2;
                batch.PrintingStatus = batchStatusItem.Item3;

                if (batchStatusItem.Item2 == BatchStatus.Error)
                {
                    batch.ResponseStatus = ResponseStatus.Error;
                    batch.ResponseMessage = "Something went wrong";
                }
                else
                {
                    batch.ResponseStatus = ResponseStatus.Success;
                }

                batch.PrintBatchItems.ToList().ForEach(p => p.Status = batchStatusItem.Item4);
                DbContext.SaveChanges();
            }

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

        public async Task WhenAsync(TrackBatchResponse trackBatchResponse)
        {
            if (_actualResult != null)
                return;

            var trackBatchResponses = trackBatchResponse != null ? new List<TrackBatchResponse> { trackBatchResponse } : null;

            _actualResult = await PrintingService.UpdateTrackBatchResponsesAsync(trackBatchResponses);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(TrackBatchResponse trackBatchResponse, CertificatePrintingResponse certificatePrintingResponse)
        {
            await WhenAsync(trackBatchResponse);

            _actualResult.Should().NotBeNull();

            _actualResult.IsSuccess.Should().Be(certificatePrintingResponse.IsSuccess);
            _actualResult.PrintingProcessedCount.Should().Be(certificatePrintingResponse.PrintingProcessedCount);
            _actualResult.ModifiedCount.Should().Be(certificatePrintingResponse.ModifiedCount);
            _actualResult.SavedCount.Should().Be(certificatePrintingResponse.SavedCount);

            if (trackBatchResponse != null)
            {
                foreach (var deliveryNotification in trackBatchResponse.DeliveryNotifications)
                {
                    var actualBatch = DbContext.Batch.FirstOrDefault(b => b.Id == deliveryNotification.BatchNumber);

                    actualBatch.Should().NotBeNull();

                    if (deliveryNotification.Status.Equals("Error", StringComparison.InvariantCultureIgnoreCase))
                    {
                        actualBatch.ResponseStatus.Should().Be(ResponseStatus.Error);
                        actualBatch.ResponseMessage.Should().Be(deliveryNotification.ErrorMessage);
                    }
                    else
                    {
                        foreach (var trackingDetail in deliveryNotification.TrackingDetails)
                        {
                            var actualBatchItem = actualBatch.PrintBatchItems.FirstOrDefault(x => x.TlProviderAddress.TlProvider.UkPrn == trackingDetail.UKPRN.ToLong());

                            actualBatchItem.Should().NotBeNull();

                            var printBatchItemStatus = EnumExtensions.GetEnumByDisplayName<PrintingBatchItemStatus>(trackingDetail.Status);
                            actualBatchItem.Status.Should().Be(printBatchItemStatus);
                            actualBatchItem.StatusChangedOn.Should().Be(trackingDetail.StatusChangeDate);
                            actualBatchItem.Reason.Should().Be(trackingDetail.Reason);
                            actualBatchItem.TrackingId.Should().Be(trackingDetail.TrackingId);
                            actualBatchItem.SignedForBy.Should().Be(trackingDetail.SignedForBy);

                            actualBatch.ResponseStatus.Should().Be(ResponseStatus.Success);
                            actualBatch.ResponseMessage.Should().BeNullOrWhiteSpace();
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new TrackBatchResponse { DeliveryNotifications = new List<DeliveryNotification> { new DeliveryNotification { BatchNumber = 3, Status = EnumExtensions.GetDisplayName(ResponseStatus.Success), ErrorMessage = string.Empty, TrackingDetails = new List<TrackingDetail> { new TrackingDetail { Name = "Test", UKPRN = "10000536", Status = EnumExtensions.GetDisplayName(PrintingBatchItemStatus.Delivered), TrackingId = "5878AB44478", StatusChangeDate = DateTime.UtcNow, SignedForBy = "User" } } } } },
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1,  }
                    },
                    new object[] { new TrackBatchResponse { DeliveryNotifications = new List<DeliveryNotification> { new DeliveryNotification { BatchNumber = 4, Status = EnumExtensions.GetDisplayName(ResponseStatus.Success), ErrorMessage = string.Empty, TrackingDetails = new List<TrackingDetail> { new TrackingDetail { Name = "Test", UKPRN = "10000536", Status = EnumExtensions.GetDisplayName(PrintingBatchItemStatus.NotDelivered), TrackingId = null, StatusChangeDate = DateTime.UtcNow, SignedForBy = null } } } } },
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1,  }
                    },
                    new object[] { new TrackBatchResponse { DeliveryNotifications = new List<DeliveryNotification> { new DeliveryNotification { BatchNumber = 5, Status = EnumExtensions.GetDisplayName(ResponseStatus.Error), ErrorMessage = "Invalid token", TrackingDetails = new List<TrackingDetail>() } } },
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 2,  }
                    },
                    new object[] { null,
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0,  }
                    }
                };
            }
        }
    }
}
