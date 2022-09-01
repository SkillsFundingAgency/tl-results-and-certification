using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.UcasDataServiceTests
{
    public class When_UcasDataType_Is_Results : UcasDataServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Withdrawn },
            };
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, null);

            SetAcademicYear(_registrations, new List<long> { 1111111111, 1111111112, 1111111113 }, -1);

            var ulnsWithOverallResult = new List<long> { 1111111111, 1111111112, 1111111113 };
            SeedOverallResultData(_registrations, ulnsWithOverallResult);

            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            await Task.CompletedTask;
            ActualResult = await UcasDataService.ProcessUcasDataRecordsAsync(UcasDataType.Results);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            ActualResult.Should().NotBeNull();

            AssertHeaderRecord(UcasDataType.Results);

            ActualResult.UcasDataRecords.Should().HaveCount(2);

            var expectedDataRecords = new List<ExepectedUcasDataRecord>
            {
                new ExepectedUcasDataRecord { Uln = 1111111111, Name = "Last 1:First 1", Sex = "M", DateOfBirth = "10101980", ComponentRecord = "_|10123456|A*||_|10123456|D||_|TLEVEL|D||_|" },
                new ExepectedUcasDataRecord { Uln = 1111111112, Name = "Last 2:First 2", Sex = "F", DateOfBirth = "07051981", ComponentRecord = "_|10123456|A*||_|10123456|D||_|TLEVEL|D||_|" },
            };

            foreach (var expectedRecord in expectedDataRecords)
            {
                var actualRecord = ActualResult.UcasDataRecords.FirstOrDefault(x => x.CandidateNumber == expectedRecord.Uln.ToString());
                actualRecord.Should().NotBeNull();

                actualRecord.UcasRecordType.Should().Be((char)UcasRecordType.Subject);
                actualRecord.SendingOrganisation.Should().Be(ResultsAndCertificationConfiguration.UcasDataSettings.SendingOrganisation);
                actualRecord.ReceivingOrganisation.Should().Be(ResultsAndCertificationConfiguration.UcasDataSettings.ReceivingOrganisation);
                actualRecord.CentreNumber.Should().Be(ResultsAndCertificationConfiguration.UcasDataSettings.CentreNumber);

                actualRecord.CandidateNumber.Should().Be(expectedRecord.Uln.ToString());
                actualRecord.CandidateName.Should().Be(expectedRecord.Name);
                actualRecord.CandidateDateofBirth.Should().Be(expectedRecord.DateOfBirth);
                actualRecord.Sex.Should().Be(expectedRecord.Sex);
                actualRecord.UcaDataComponentRecord.Should().Be(expectedRecord.ComponentRecord);
            }

            AssertTrailerRecord();
        }
    }
}