using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CertificateServiceTests.GetLearnerResultsForPrinting
{
    public class When_GetLearnerResultsForPrintingAsync_IsCalled : CertificateServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<LearnerResultsPrintingData> _actualResult;
        private List<OverallResult> _expectedResults = new List<OverallResult>();

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },    // Valid (Barsley)
                { 1111111112, RegistrationPathwayStatus.Withdrawn }, // Invalid - Withdrawn
                { 1111111113, RegistrationPathwayStatus.Active },    // Invalid - PrintAvailableFrom
                { 1111111114, RegistrationPathwayStatus.Active },    // Invalid - CalculationStatus
                { 1111111115, RegistrationPathwayStatus.Active },    // Invalid - CertificateStatus
                { 1111111116, RegistrationPathwayStatus.Active }     // Valid (Walsall)
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            SeedTqProvider(Provider.WalsallCollege);
            SetRegistrationProviders(_registrations, new List<long> { 1111111116 }, Provider.WalsallCollege);

            // Valid
            _expectedResults.Add(new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111111))
                .WithPrintAvailableFrom(DateTime.Now.AddDays(-1))
                .WithCalculationStatus(CalculationStatus.Completed)
                .WithCertificateStatus(CertificateStatus.AwaitingProcessing)
                .Save(DbContext));

            _expectedResults.Add(new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111116))
                .WithPrintAvailableFrom(DateTime.Now.AddDays(-1))
                .WithCalculationStatus(CalculationStatus.Completed)
                .WithCertificateStatus(CertificateStatus.AwaitingProcessing)
                .Save(DbContext));

            // Invalid - PrintAvailableFrom not reached
            _ = new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111113))
                .WithPrintAvailableFrom(DateTime.Now.AddDays(1))
                .Save(DbContext);

            // Invalid - CalculationStatus InAppeal
            _ = new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111114))
                .WithCalculationStatus(CalculationStatus.CompletedAppealRaised)
                .Save(DbContext);

            // Invalid - CertificateStatus Processed
            _ = new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111115))
                .WithCertificateStatus(CertificateStatus.Processed)
                .Save(DbContext);

            // Create CertificateService
            CreateService();
        }

        public override async Task When()
        {
            _actualResult = await CertificateService.GetLearnerResultsForPrintingAsync();
        }

        [Fact]
        public void Then_ExpectedResults_Are_Returned()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(2);

            AssertLearnerDetailsOf(Provider.BarsleyCollege);
            AssertLearnerDetailsOf(Provider.WalsallCollege);
        }

        private void AssertLearnerDetailsOf(Provider provider)
        {
            var actualResult = _actualResult.Where(x => x.TlProvider.UkPrn == (long)provider);
            actualResult.Should().HaveCount(1);

            // Assert TlProvider
            var actualTlProvider = actualResult.First().TlProvider;
            actualTlProvider.Should().NotBeNull();

            var expectedTlProvider = TlProviders.FirstOrDefault(x => x.UkPrn == (long)provider);
            AssertTlProvider(actualTlProvider, expectedTlProvider);

            // Assert TlProviderAddress
            var actualTlProviderAdddress = actualTlProvider.TlProviderAddresses;
            actualTlProviderAdddress.Should().HaveCount(1);
            actualTlProviderAdddress.First().Should().BeEquivalentTo(actualTlProvider.TlProviderAddresses.FirstOrDefault());

            // Assert OverallResults
            var actualOverallResults = actualResult.First().OverallResults;
            actualOverallResults.Should().HaveCount(1);

            var expectedOverallResult = _expectedResults.FirstOrDefault(x => x.TqRegistrationPathway.TqProvider.TlProvider.UkPrn == (long)provider);
            AssertOverallResult(actualOverallResults[0], expectedOverallResult);

            // Assert Profile
            var actualProfile = actualResult.First().OverallResults.First().TqRegistrationPathway.TqRegistrationProfile;
            var expectedProfile = expectedOverallResult.TqRegistrationPathway.TqRegistrationProfile;
            AssertProfile(actualProfile, expectedProfile);
        }

        private int GetPathwayId(long uln)
        {
            return _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
        }
    }
}
