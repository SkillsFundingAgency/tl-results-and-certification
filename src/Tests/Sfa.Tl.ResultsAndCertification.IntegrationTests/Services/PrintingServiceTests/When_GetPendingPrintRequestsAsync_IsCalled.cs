using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
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
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PrintingServiceTests
{
    public class When_GetPendingPrintRequestsAsync_IsCalled : PrintingServiceBaseTest
    {
        private IList<PrintRequest> _actualResult;

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

        public async Task WhenAsync()
        {
            if (_actualResult != null)
                return;

            _actualResult = await PrintingService.GetPendingPrintRequestsAsync();
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();

            var expectedBatchesCount = _printCertificates.Select(p => p.PrintBatchItem.Batch).Distinct().Count();

            _actualResult.Should().NotBeNullOrEmpty();

            _actualResult.Count().Should().Be(expectedBatchesCount);

            var expectedBatches = _printCertificates.Select(pc => pc.PrintBatchItem.Batch).Distinct();


            foreach (var expectedBatch in expectedBatches)
            {
                var actualPrintRequest = _actualResult.FirstOrDefault(b => b.Batch.BatchNumber == expectedBatch.Id);

                actualPrintRequest.Should().NotBeNull();
                actualPrintRequest.Batch.BatchNumber.Should().Be(expectedBatch.Id);
                actualPrintRequest.Batch.BatchDate.Should().Be(expectedBatch.CreatedOn.ToPrintBatchDateFormat());
                actualPrintRequest.Batch.PostalContactCount.Should().Be(expectedBatch.PrintBatchItems.Count);
                actualPrintRequest.Batch.PostalContactCount.Should().Be(expectedBatch.PrintBatchItems.Sum(x => x.PrintCertificates.Count));

                foreach(var actualPrintData in actualPrintRequest.PrintData)
                {
                    var expectedPrintBatchItem = expectedBatch.PrintBatchItems.FirstOrDefault(a => a.TlProviderAddress.TlProvider.UkPrn == actualPrintData.PostalContact.UKPRN.ToLong());

                    expectedPrintBatchItem.Should().NotBeNull();
                    actualPrintData.PostalContact.DepartmentName.Should().Be(expectedPrintBatchItem.TlProviderAddress.DepartmentName);
                    actualPrintData.PostalContact.Name.Should().Be(!string.IsNullOrWhiteSpace(expectedPrintBatchItem.TlProviderAddress.OrganisationName) ? expectedPrintBatchItem.TlProviderAddress.OrganisationName : expectedPrintBatchItem.TlProviderAddress.TlProvider.Name);
                    actualPrintData.PostalContact.ProviderName.Should().Be(expectedPrintBatchItem.TlProviderAddress.TlProvider.Name);
                    actualPrintData.PostalContact.UKPRN.Should().Be(expectedPrintBatchItem.TlProviderAddress.TlProvider.UkPrn.ToString());
                    actualPrintData.PostalContact.AddressLine1.Should().Be(expectedPrintBatchItem.TlProviderAddress.AddressLine1);
                    actualPrintData.PostalContact.AddressLine2.Should().Be(expectedPrintBatchItem.TlProviderAddress.AddressLine2);
                    actualPrintData.PostalContact.Town.Should().Be(expectedPrintBatchItem.TlProviderAddress.Town);
                    actualPrintData.PostalContact.Postcode.Should().Be(expectedPrintBatchItem.TlProviderAddress.Postcode);
                    actualPrintData.PostalContact.CertificateCount.Should().Be(expectedPrintBatchItem.PrintCertificates.Count);

                    foreach(var expectedCertificate in expectedPrintBatchItem.PrintCertificates)
                    {
                        var actualCertificate = actualPrintData.Certificates.FirstOrDefault(c => c.Uln == expectedCertificate.Uln && c.LearnerName == expectedCertificate.LearnerName);

                        actualCertificate.Should().NotBeNull();

                        actualCertificate.CertificateNumber.Should().Be(expectedCertificate.CertificateNumber);
                        actualCertificate.Type.Should().Be(expectedCertificate.Type.ToString());
                        actualCertificate.Uln.Should().Be(expectedCertificate.Uln);
                        actualCertificate.LearnerName.Should().Be(expectedCertificate.LearnerName);
                        var expectedLearnerDetails = JsonConvert.DeserializeObject<LearningDetails>(expectedCertificate.LearningDetails);
                        actualCertificate.LearningDetails.Should().BeEquivalentTo(expectedLearnerDetails);
                    }
                }                
            }
        }
    }
}