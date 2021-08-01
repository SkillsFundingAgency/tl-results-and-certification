using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
    public class When_UpdatePrintReqeustResponsesAsync_IsCalled : PrintingServiceBaseTest
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
                { 1111111112, RegistrationPathwayStatus.Withdrawn }
            };

            // Seed Registrations
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            foreach (var uln in ulns)
                _profiles.Add(SeedRegistrationDataByStatus(uln.Key, uln.Value, TqProvider));

            // Seed PrintCertificate
            _printCertificates = new List<PrintCertificate>();

            foreach (var profile in _profiles)
                _printCertificates.Add(SeedPrintCertificate(profile.TqRegistrationPathways.FirstOrDefault()));

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

        public async Task WhenAsync(PrintRequestResponse printRequestResponse)
        {
            if (_actualResult != null)
                return;

            var printingResponses = printRequestResponse != null ? new List<PrintRequestResponse> { printRequestResponse } : null;

            _actualResult = await PrintingService.UpdatePrintRequestResponsesAsync(printingResponses);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(PrintRequestResponse printRequestResponse, CertificatePrintingResponse certificatePrintingResponse)
        {
            await WhenAsync(printRequestResponse);

            _actualResult.Should().NotBeNull();            

            _actualResult.IsSuccess.Should().Be(certificatePrintingResponse.IsSuccess);
            _actualResult.PrintingProcessedCount.Should().Be(certificatePrintingResponse.PrintingProcessedCount);
            _actualResult.ModifiedCount.Should().Be(certificatePrintingResponse.ModifiedCount);
            _actualResult.SavedCount.Should().Be(certificatePrintingResponse.SavedCount);

            if(printRequestResponse != null)
            {
                var actualBatch = DbContext.Batch.FirstOrDefault(b => b.Id == (printRequestResponse.BatchNumber < 1 ? printRequestResponse.BatchId : printRequestResponse.BatchNumber));

                var expectedBatchStatus = printRequestResponse.Status == ResponseStatus.Error.ToString() ? BatchStatus.Error : BatchStatus.Accepted;
                actualBatch.Status.Should().Be(expectedBatchStatus);

                if (expectedBatchStatus == BatchStatus.Error)
                {
                    actualBatch.ResponseStatus.Should().Be(ResponseStatus.Error);
                    actualBatch.ResponseMessage.Should().Be(JsonConvert.SerializeObject(printRequestResponse.Errors));
                }
                else
                {
                    actualBatch.ResponseStatus.Should().Be(ResponseStatus.Success);
                    actualBatch.ResponseMessage.Should().BeNullOrWhiteSpace();
                }
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new PrintRequestResponse { BatchNumber = -1, BatchId = 1, Status = ResponseStatus.Error.ToString(), Errors = new List<Error> { new Error { CertificateNumber = "000000001", Name = "Test Name", ErrorMessage = "Uln is required" }  } },
                    new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 1, ModifiedCount = 1, SavedCount = 1,  }
                    },
                    new object[] { new PrintRequestResponse { BatchNumber = 2, Status = ResponseStatus.Success.ToString(), Errors = new List<Error>() },
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