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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PrintingServiceTests
{
    public class When_UpdateBatchSummaryResponsesAsync_IsCalled : PrintingServiceBaseTest
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

            var batchStatusItemsToSeed = new List<(int, BatchStatus, PrintingStatus?)>
            {
                ( 1, BatchStatus.Error, null),
                ( 2, BatchStatus.Created, null),
                ( 3, BatchStatus.Accepted, null),
                ( 4, BatchStatus.Accepted, null),
                ( 5, BatchStatus.Accepted, null)
            };

            foreach (var batchStatusItem in batchStatusItemsToSeed)
            {
                var batch = _printCertificates.FirstOrDefault(p => p.PrintBatchItem.Batch.Id == batchStatusItem.Item1)?.PrintBatchItem.Batch;
                batch.Status = batchStatusItem.Item2;
                batch.PrintingStatus = batchStatusItem.Item3;
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

        public async Task WhenAsync(BatchSummaryResponse batchSummaryResponse)
        {
            if (_actualResult != null)
                return;

            var batchSummaryResponses = batchSummaryResponse != null ? new List<BatchSummaryResponse> { batchSummaryResponse } : null;

            _actualResult = await PrintingService.UpdateBatchSummaryResponsesAsync(batchSummaryResponses);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(BatchSummaryResponse batchSummaryResponse, CertificatePrintingResponse certificatePrintingResponse)
        {
            await WhenAsync(batchSummaryResponse);

            _actualResult.Should().NotBeNull();

            _actualResult.IsSuccess.Should().Be(certificatePrintingResponse.IsSuccess);
            _actualResult.PrintingProcessedCount.Should().Be(certificatePrintingResponse.PrintingProcessedCount);
            _actualResult.ModifiedCount.Should().Be(certificatePrintingResponse.ModifiedCount);
            _actualResult.SavedCount.Should().Be(certificatePrintingResponse.SavedCount);

            if (batchSummaryResponse != null)
            {
                foreach (var batchSummary in batchSummaryResponse.BatchSummary)
                {
                    var actualBatch = DbContext.Batch.FirstOrDefault(b => b.Id == batchSummary.BatchNumber);

                    actualBatch.Should().NotBeNull();

                    if (batchSummary.Status.Equals("Error", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        actualBatch.ResponseStatus.Should().Be(ResponseStatus.Error);
                        actualBatch.ResponseMessage.Should().Be(batchSummary.ErrorMessage);
                    }
                    else
                    {
                        var expectedBatchStatus = EnumExtensions.GetEnumByDisplayName<PrintingStatus>(batchSummary.Status);
                        actualBatch.PrintingStatus.Should().Be(expectedBatchStatus);
                        actualBatch.ResponseStatus.Should().Be(ResponseStatus.Success);
                        actualBatch.ResponseMessage.Should().BeNullOrWhiteSpace();
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
                    new object[] { new BatchSummaryResponse { BatchSummary = new List<BatchSummary> { new BatchSummary { BatchNumber = 3, Status = EnumExtensions.GetDisplayName(PrintingStatus.AwaitingProcessing), ErrorMessage = string.Empty } } },
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1,  }
                    },
                    new object[] { new BatchSummaryResponse { BatchSummary = new List<BatchSummary> { new BatchSummary { BatchNumber = 4, Status = EnumExtensions.GetDisplayName(PrintingStatus.CollectedByCourier), ErrorMessage = string.Empty } } },
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1,  }
                    },
                    new object[] { new BatchSummaryResponse { BatchSummary = new List<BatchSummary> { new BatchSummary { BatchNumber = 5, Status = EnumExtensions.GetDisplayName(ResponseStatus.Error), ErrorMessage = "Batch does not exist" } } },
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1,  }
                    },
                    new object[] { null,
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0,  }
                    }
                };
            }
        }
    }
}
