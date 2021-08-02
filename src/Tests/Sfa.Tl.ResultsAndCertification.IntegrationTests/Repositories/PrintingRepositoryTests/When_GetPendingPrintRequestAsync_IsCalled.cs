using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.PrintingRepositoryTests
{
    public class When_GetPendingPrintRequestAsync_IsCalled : PrintingRepositoryBaseTest
    {
        private IList<Batch> _actualResult;

        // Seed data variables
        private List<TqRegistrationProfile> _profiles;
        private List<PrintCertificate> _printCertificates;

        public override void Given()
        {
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

            foreach(var profile in _profiles)
            _printCertificates.Add(SeedPrintCertificate(profile.TqRegistrationPathways.FirstOrDefault()));

            PrintingRepository = new PrintingRepository(DbContext, PrintingRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            if (_actualResult != null)
                return;

            _actualResult = await PrintingRepository.GetPendingPrintRequestAsync();
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();

            var expectedBatchesCount = _printCertificates.Select(p => p.PrintBatchItem.Batch).Distinct().Count();

            _actualResult.Should().NotBeNullOrEmpty();

            _actualResult.Count().Should().Be(expectedBatchesCount);

            foreach (var expectedPrintCertificate in _printCertificates)
            {
                var actualBatch = _actualResult.FirstOrDefault(b => b.Id == expectedPrintCertificate.PrintBatchItem.Batch.Id);

                actualBatch.Should().NotBeNull();
                actualBatch.Should().BeEquivalentTo(expectedPrintCertificate.PrintBatchItem.Batch);

                var actualPrintBatchItem = actualBatch.PrintBatchItems.FirstOrDefault(pb => pb.Id == expectedPrintCertificate.PrintBatchItem.Id);

                actualPrintBatchItem.Should().BeEquivalentTo(expectedPrintCertificate.PrintBatchItem);

                var actualPrintCertificate = actualPrintBatchItem.PrintCertificates.FirstOrDefault(pc => pc.Id == expectedPrintCertificate.Id);

                actualPrintCertificate.Should().BeEquivalentTo(expectedPrintCertificate);
            }
        }        
    }
}
