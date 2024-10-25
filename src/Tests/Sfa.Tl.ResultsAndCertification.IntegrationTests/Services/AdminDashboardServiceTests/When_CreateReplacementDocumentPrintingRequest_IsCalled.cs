using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests
{
    public class When_CreateReplacementDocumentPrintingRequest_IsCalled : AdminDashboardServiceBaseTest
    {
        private readonly List<PrintCertificate> _printCertificates = new();
        private bool _actualResult;

        public override void Given()
        {
            CreateAdminDasboardService();

            var _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Withdrawn }
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            List<TqRegistrationProfile> profiles = SeedRegistrationsData(_ulns, TqProvider);

            foreach (var profile in profiles)
            {
                _printCertificates.Add(SeedPrintCertificate(profile.TqRegistrationPathways.FirstOrDefault(), TlProviderAddress));
            }

            DbContext.SaveChanges();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(ReplacementPrintRequest request)
        {
            _actualResult = await AdminDashboardService.CreateReplacementDocumentPrintingRequestAsync(request);
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
                    new object[] { new ReplacementPrintRequest { ProviderUkprn = 10000536, Uln = 1111111113, ProviderAddressId = 1, PrintCertificateId = 3, PerformedBy = "System" }, true } // Withdrawn valid
                };
            }
        }
    }
}