using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_CreateReplacementDocumentPrintingRequest_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner)> _testCriteriaData;
        private List<TqRegistrationProfile> _profiles;
        private List<PrintCertificate> _printCertificates;
        private bool _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Withdrawn }
            };

            CreateMapper();

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            // Seed PrintCertificate
            _printCertificates = new List<PrintCertificate>();

            foreach (var profile in _profiles)
                _printCertificates.Add(SeedPrintCertificate(profile.TqRegistrationPathways.FirstOrDefault(), TlProviderAddresses.FirstOrDefault(a => a.TlProviderId == profile.TqRegistrationPathways.FirstOrDefault().TqProvider.TlProviderId)));

            DbContext.SaveChanges();

            // Create Service
            RegistrationProfileRepositoryLogger = new Logger<GenericRepository<TqRegistrationProfile>>(new NullLoggerFactory());
            RegistrationProfileRepository = new GenericRepository<TqRegistrationProfile>(RegistrationProfileRepositoryLogger, DbContext);

            TrainingProviderRepositoryLogger = new Logger<TrainingProviderRepository>(new NullLoggerFactory());
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TrainingProviderRepositoryLogger);

            TrainingProviderServiceLogger = new Logger<TrainingProviderService>(new NullLoggerFactory());

            BatchRepositoryLogger = new Logger<GenericRepository<Batch>>(new NullLoggerFactory());
            BatchRepository = new GenericRepository<Batch>(BatchRepositoryLogger, DbContext);

            PrintCertificateRepositoryLogger = new Logger<GenericRepository<PrintCertificate>>(new NullLoggerFactory());
            PrintCertificateRepository = new GenericRepository<PrintCertificate>(PrintCertificateRepositoryLogger, DbContext);

            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, TrainingProviderRepository, BatchRepository, PrintCertificateRepository, TrainingProviderMapper, TrainingProviderServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(ReplacementPrintRequest request)
        {
            _actualResult = await TrainingProviderService.CreateReplacementDocumentPrintingRequestAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(ReplacementPrintRequest request, bool expectedResult)
        {
            await WhenAsync(request);

            _actualResult.Should().Be(expectedResult);

            if (expectedResult)
            {

                var expectedCertificate = _printCertificates.FirstOrDefault(pc => pc.Id == request.PrintCertificateId);

                var actualCertificate = DbContext.PrintCertificate.Where(pc => pc.Uln == request.Uln && pc.Id != request.PrintCertificateId).Include(pc => pc.PrintBatchItem).ThenInclude(pc => pc.Batch).OrderByDescending(p => p.CreatedOn).FirstOrDefault();

                actualCertificate.TqRegistrationPathwayId.Should().Be(expectedCertificate.TqRegistrationPathwayId);
                actualCertificate.CertificateNumber.Should().Be(expectedCertificate.CertificateNumber);
                actualCertificate.Type.Should().Be(expectedCertificate.Type);
                actualCertificate.Uln.Should().Be(expectedCertificate.Uln);
                actualCertificate.LearnerName.Should().Be(expectedCertificate.LearnerName);
                actualCertificate.LearningDetails.Should().Be(expectedCertificate.LearningDetails);
                actualCertificate.DisplaySnapshot.Should().Be(expectedCertificate.DisplaySnapshot);
                actualCertificate.IsReprint.Should().BeTrue();

                var actualPrintBatchItem = actualCertificate.PrintBatchItem;
                actualPrintBatchItem.Should().NotBeNull();
                actualPrintBatchItem.TlProviderAddressId.Should().Be(expectedCertificate.PrintBatchItem.TlProviderAddressId);
                actualPrintBatchItem.Status.Should().BeNull();
                actualPrintBatchItem.Reason.Should().BeNull();
                actualPrintBatchItem.TrackingId.Should().BeNull();
                actualPrintBatchItem.SignedForBy.Should().BeNull();
                actualPrintBatchItem.StatusChangedOn.Should().BeNull();

                var actualBatch = actualPrintBatchItem.Batch;
                actualBatch.Type.Should().Be(expectedCertificate.PrintBatchItem.Batch.Type);
                actualBatch.Status.Should().Be(expectedCertificate.PrintBatchItem.Batch.Status);
                actualBatch.Errors.Should().BeNull();
                actualBatch.PrintingStatus.Should().BeNull();
                actualBatch.RunOn.Should().BeNull();
                actualBatch.ResponseStatus.Should().BeNull();
                actualBatch.ResponseMessage.Should().BeNull();
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new ReplacementPrintRequest { ProviderUkprn = 12345678, Uln = 1111111111, ProviderAddressId = 1, PrintCertificateId = 1, PerformedBy = "System" }, false }, // Invalid ProviderUkprn
                    new object[] { new ReplacementPrintRequest { ProviderUkprn = 10000536, Uln = 1111111111, ProviderAddressId = 1, PrintCertificateId = 1, PerformedBy = "System" }, true }, // Valid
                    new object[] { new ReplacementPrintRequest { ProviderUkprn = 10000536, Uln = 1111111112, ProviderAddressId = 1, PrintCertificateId = 1, PerformedBy = "System" }, false }, // Invalid PrintCertificateId 
                    new object[] { new ReplacementPrintRequest { ProviderUkprn = 10000536, Uln = 1111111112, ProviderAddressId = 1, PrintCertificateId = 2, PerformedBy = "System" }, true }, // Valid
                    new object[] { new ReplacementPrintRequest { ProviderUkprn = 10000536, Uln = 1111111113, ProviderAddressId = 1, PrintCertificateId = 3, PerformedBy = "System" }, false } // Withdrawn Invalid
                };
            }
        }
    }
}
