using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CertificateServiceTests.GetLearnerResultsForPrinting
{
    public class When_Provider_With_MultiAddresses : CertificateServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<LearnerResultsPrintingData> _actualResult;
        private OverallResult _expectedOverallResult;
        private TlProviderAddress _latestAddress;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active }    // Valid
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            // Valid
            _expectedOverallResult = new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111111))
                .WithPrintAvailableFrom(DateTime.Now.AddDays(-1))
                .WithCalculationStatus(CalculationStatus.Completed)
                .WithCertificateStatus(CertificateStatus.AwaitingProcessing)
                .Save(DbContext);

            _latestAddress = AddAdditionalProviderAddress();
            DbContext.SaveChanges();

            // Create CertificateService
            CreateService();
        }

        public override async Task When()
        {
            _actualResult = await CertificateService.GetLearnerResultsForPrintingAsync();
        }

        [Fact(Skip = "TODO")]
        public void Then_ExpectedResults_Are_Returned()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(1);

            // Assert TlProvider
            var actualTlProvider = _actualResult.First().TlProvider;
            actualTlProvider.Should().NotBeNull();
            actualTlProvider.Name.Should().Be(TlProvider.Name);
            actualTlProvider.DisplayName.Should().Be(TlProvider.DisplayName);
            actualTlProvider.IsActive.Should().Be(TlProvider.IsActive);
            actualTlProvider.CreatedBy.Should().Be(TlProvider.CreatedBy);
            actualTlProvider.CreatedOn.Should().Be(TlProvider.CreatedOn);
            
            // Assert TlProviderAddress
            var actualTlProviderAdddress = actualTlProvider.TlProviderAddresses.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            actualTlProviderAdddress.Should().NotBeNull();
            actualTlProviderAdddress.Should().BeEquivalentTo(_latestAddress);

            // Assert OverallResults
            var actualOverallResults = _actualResult.First().OverallResults;
            actualOverallResults.Should().HaveCount(1);
            actualOverallResults.First().Should().BeEquivalentTo(_expectedOverallResult);
        }

        private int GetPathwayId(long uln)
        {
            return _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
        }

        private TlProviderAddress AddAdditionalProviderAddress()
        {
            var latestAddress = new TlProviderAddressBuilder().Build(TlProvider);
            latestAddress.DepartmentName = "Latest Office";
            latestAddress.OrganisationName = "Latest Org";
            latestAddress.AddressLine1 = "Latest Address1";

            return TlProviderAddress = TlProviderAddressDataProvider.CreateTlProviderAddress(DbContext, latestAddress);
        }
    }
}
