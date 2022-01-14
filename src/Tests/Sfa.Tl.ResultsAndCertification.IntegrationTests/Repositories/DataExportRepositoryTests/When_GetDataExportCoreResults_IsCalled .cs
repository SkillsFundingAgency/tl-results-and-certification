using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.DataExportRepositoryTests
{
    public class When_GetDataExportCoreResults_IsCalled : DataExportRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private IList<CoreResultsExport> _actualResult;

        public override void Given()
        {
            // Seed data
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Withdrawn}
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            var currentAcademicYear = GetAcademicYear();

            _registrations.ForEach(x =>
            {
                x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1);
            });


            var pathwaysWithAssessments = new List<long> { 1111111111, 1111111113, 1111111114 };
            var pathwaysWithResults = new List<long> { 1111111111, 1111111113, 1111111114 };
            SeedAssessmentsAndResults(_registrations, pathwaysWithAssessments, pathwaysWithResults, $"Summer {currentAcademicYear}");

            pathwaysWithAssessments = new List<long> { 1111111112 };
            SeedPathwayAssessments(_registrations, pathwaysWithAssessments, $"Autumn {currentAcademicYear}");


            // Test class.
            DataExportRepository = new DataExportRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn)
        {
            if (_actualResult != null)
                return;

            _actualResult = await DataExportRepository.GetDataExportCoreResultsAsync(aoUkprn);
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

            var expectedPathwayResults = await DbContext.TqPathwayResult
                        .Where(x => x.IsOptedin && x.EndDate == null && // Active result
                                    x.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&  // Active Pathway
                                    x.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn) // Given Ao
                        .Select(x => new CoreResultsExport
                        {
                            Uln = x.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                            CoreCode = x.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                            CoreAssessmentEntry = x.TqPathwayAssessment.AssessmentSeries.Name,
                            CoreGrade = x.TlLookup.Value
                        }).ToListAsync();

            _actualResult.Count().Should().Be(expectedPathwayResults.Count);
            _actualResult.Should().BeEquivalentTo(expectedPathwayResults);
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
