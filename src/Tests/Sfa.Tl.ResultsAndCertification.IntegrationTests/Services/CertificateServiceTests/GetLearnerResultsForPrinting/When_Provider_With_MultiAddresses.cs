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

            _registrations.First().MathsStatus = SubjectStatus.AchievedByLrs;
            _registrations.First().EnglishStatus = SubjectStatus.AchievedByLrs;

            // Valid
            _expectedOverallResult = new OverallResultCustomBuilder()
                .WithTqRegistrationPathwayId(GetPathwayId(1111111111))
                .WithPrintAvailableFrom(DateTime.Now.AddDays(-1))
                .WithCalculationStatus(CalculationStatus.Completed)
                .WithCertificateStatus(CertificateStatus.AwaitingProcessing)
                .Save(DbContext);

            _latestAddress = AddAdditionalProviderAddress();
            DbContext.SaveChanges();
            DetachAll();

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

            actualTlProviderAdddress.TlProviderId.Should().Be(_latestAddress.TlProviderId);
            actualTlProviderAdddress.DepartmentName.Should().Be(_latestAddress.DepartmentName);
            actualTlProviderAdddress.OrganisationName.Should().Be(_latestAddress.OrganisationName);
            actualTlProviderAdddress.AddressLine1.Should().Be(_latestAddress.AddressLine1);
            actualTlProviderAdddress.AddressLine2.Should().Be(_latestAddress.AddressLine2);
            actualTlProviderAdddress.Town.Should().Be(_latestAddress.Town);
            actualTlProviderAdddress.Postcode.Should().Be(_latestAddress.Postcode);
            actualTlProviderAdddress.IsActive.Should().Be(_latestAddress.IsActive);
            actualTlProviderAdddress.CreatedOn.Should().Be(_latestAddress.CreatedOn);
            actualTlProviderAdddress.CreatedBy.Should().Be(_latestAddress.CreatedBy);

            // Assert OverallResults
            var actualOverallResults = _actualResult.First().OverallResults;
            actualOverallResults.Should().HaveCount(1);

            actualOverallResults[0].TqRegistrationPathwayId.Should().Be(_expectedOverallResult.TqRegistrationPathwayId);
            actualOverallResults[0].Details.Should().Be(_expectedOverallResult.Details);
            actualOverallResults[0].ResultAwarded.Should().Be(_expectedOverallResult.ResultAwarded);
            actualOverallResults[0].CalculationStatus.Should().Be(_expectedOverallResult.CalculationStatus);
            actualOverallResults[0].PrintAvailableFrom.Should().Be(_expectedOverallResult.PrintAvailableFrom);
            actualOverallResults[0].PublishDate.Should().Be(_expectedOverallResult.PublishDate);
            if (_expectedOverallResult.EndDate == null)
            {
                actualOverallResults[0].IsOptedin.Should().BeTrue();
                actualOverallResults[0].EndDate.Should().BeNull();
            }
            else
            {
                actualOverallResults[0].IsOptedin.Should().BeFalse();
                actualOverallResults[0].EndDate.Should().NotBeNull();
            }

            actualOverallResults[0].CertificateType.Should().Be(_expectedOverallResult.CertificateType);
            actualOverallResults[0].CertificateStatus.Should().Be(_expectedOverallResult.CertificateStatus);

            // Assert Profile
            var actualProfile = _actualResult.First().OverallResults.First().TqRegistrationPathway.TqRegistrationProfile;
            var expectedProfile = _expectedOverallResult.TqRegistrationPathway.TqRegistrationProfile;
            actualProfile.Firstname.Should().Be(expectedProfile.Firstname);
            actualProfile.Lastname.Should().Be(expectedProfile.Lastname);
            actualProfile.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            actualProfile.Gender.Should().Be(expectedProfile.Gender);
            actualProfile.UniqueLearnerNumber.Should().Be(expectedProfile.UniqueLearnerNumber);
            actualProfile.MathsStatus.Should().Be(expectedProfile.MathsStatus);
            actualProfile.EnglishStatus.Should().Be(expectedProfile.EnglishStatus);
            actualProfile.IsLearnerVerified.Should().Be(expectedProfile.IsLearnerVerified);
            actualProfile.EnglishStatus.Should().Be(expectedProfile.EnglishStatus);
            actualProfile.CreatedOn.Should().Be(expectedProfile.CreatedOn);
            actualProfile.CreatedBy.Should().Be(expectedProfile.CreatedBy);
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
            latestAddress.CreatedOn = DateTime.Now.AddMinutes(3);

            return TlProviderAddress = TlProviderAddressDataProvider.CreateTlProviderAddress(DbContext, latestAddress);
        }
    }
}
