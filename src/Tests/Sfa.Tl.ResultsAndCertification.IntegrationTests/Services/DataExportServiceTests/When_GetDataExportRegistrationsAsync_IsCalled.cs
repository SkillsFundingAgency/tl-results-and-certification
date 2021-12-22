using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.DataExportServiceTests
{
    public class When_GetDataExportRegistrationsAsync_IsCalled : DataExportServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private IList<RegistrationsExport> _actualResult;

        public override void Given()
        {
            // Seed data
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Withdrawn }
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // seed registation with couplets
            _registrations.Add(SeedRegistrationData(1111111114, RegistrationPathwayStatus.Active, TqProvider, true));

            // Test class.
            DataExportRepository = new DataExportRepository(DbContext);
            DataExportService = new DataExportService(DataExportRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn)
        {
            if (_actualResult != null)
                return;

            _actualResult = await DataExportService.GetDataExportRegistrationsAsync(aoUkprn);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, bool hasRecords)
        {
            await WhenAsync(aoUkprn);

            if (hasRecords == false)
            {
                _actualResult.Should().BeNullOrEmpty();
                return;
            }

            _actualResult.Should().NotBeNull();
            _actualResult.Count().Should().Be(_registrations.Count);

            foreach (var actualExport in _actualResult)
            {
                var expectedProfile = _registrations.FirstOrDefault(p => p.UniqueLearnerNumber == actualExport.Uln);

                expectedProfile.Should().NotBeNull();

                var expectedPathway = expectedProfile.TqRegistrationPathways.OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                var expectedSpecialisms = expectedPathway.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).Select(s => s.TlSpecialism.LarId).ToList();

                actualExport.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
                actualExport.FirstName.Should().Be(expectedProfile.Firstname);
                actualExport.LastName.Should().Be(expectedProfile.Lastname);
                actualExport.DateOfBirth.Should().Be(expectedProfile.DateofBirth);
                actualExport.DisplayDateOfBirth.Should().Be(expectedProfile.DateofBirth.ToString("ddMMyyyy"));
                actualExport.Ukprn.Should().Be(expectedPathway.TqProvider.TlProvider.UkPrn);
                actualExport.AcademicYear.Should().Be(expectedPathway.AcademicYear);
                actualExport.DisplayAcademicYear.Should().Be("2020/21");
                actualExport.Core.Should().Be(expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId);
                actualExport.SpecialismsList.Should().BeEquivalentTo(expectedSpecialisms);
                actualExport.Status.Should().Be(expectedPathway.Status.ToString());
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { AwardingOrganisation.Ncfe,  false },
                    new object[] { AwardingOrganisation.Pearson,  true }
                };
            }
        }
    }
}
