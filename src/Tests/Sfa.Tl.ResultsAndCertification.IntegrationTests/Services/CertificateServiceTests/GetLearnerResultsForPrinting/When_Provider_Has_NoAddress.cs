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
    public class When_Provider_Has_NoAddress : CertificateServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<LearnerResultsPrintingData> _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            _ = new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111111))
                .WithPrintAvailableFrom(DateTime.Now.AddDays(-1))
                .WithCalculationStatus(CalculationStatus.Completed)
                .WithCertificateStatus(CertificateStatus.AwaitingProcessing)
                .Save(DbContext);

            EnsureProviderHasNoAddress();

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
            _actualResult.Should().BeEmpty();
        }

        private int GetPathwayId(long uln)
        {
            return _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
        }

        private void EnsureProviderHasNoAddress()
        {
            DbContext.TlProviderAddress.Remove(TlProviderAddress);
            DbContext.SaveChanges();
        }
    }
}
