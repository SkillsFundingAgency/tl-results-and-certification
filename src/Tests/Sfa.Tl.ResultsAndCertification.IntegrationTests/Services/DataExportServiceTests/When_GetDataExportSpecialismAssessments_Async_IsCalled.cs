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
    public class When_GetDataExportSpecialismAssessments_Async_IsCalled : DataExportServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private IList<SpecialismAssessmentsExport> _actualResult;

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

            SeedSpecialismAssessments(_registrations.SelectMany(r => r.TqRegistrationPathways).ToList());

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

            _actualResult = await DataExportService.GetDataExportSpecialismAssessmentsAsync(aoUkprn);
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

            var expectedSpecialismAssessments = _registrations.SelectMany(x => x.TqRegistrationPathways.Where(p => p.Status == RegistrationPathwayStatus.Active && p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn)
                                                           .SelectMany(p => p.TqRegistrationSpecialisms.Where(pa => pa.IsOptedin && pa.EndDate == null)
                                                           .SelectMany(p => p.TqSpecialismAssessments.Where(pa => pa.IsOptedin && pa.EndDate == null)
                                                           .Select(x => new SpecialismAssessmentsExport
                                                           {
                                                               Uln = x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                                                               SpecialismCode = x.TqRegistrationSpecialism.TlSpecialism.LarId,
                                                               SpecialismAssessmentEntry = x.AssessmentSeries.Name
                                                           })))).ToList();

            _actualResult.Count().Should().Be(expectedSpecialismAssessments.Count);
            _actualResult.Should().BeEquivalentTo(expectedSpecialismAssessments);
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
