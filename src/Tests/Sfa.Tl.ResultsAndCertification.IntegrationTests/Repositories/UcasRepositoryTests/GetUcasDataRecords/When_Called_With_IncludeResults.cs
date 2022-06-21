using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.UcasRepositoryTests.GetUcasDataRecords
{
    public class When_Called_With_IncludeResults : UcasRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private IList<TqRegistrationPathway> _result;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
                { 1111111116, RegistrationPathwayStatus.Active },
            };
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, null);

            var currentAcademicYear = GetAcademicYear();
            _registrations.ForEach(x =>
            {
                x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1);
            });

            var componentWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113, 1111111114, 1111111115 };
            var componentWithResults = new List<long> { 1111111111, 1111111112, 1111111113 };
            SeedAssessmentsAndResults(_registrations, componentWithAssessments, componentWithResults, $"Summer {currentAcademicYear}");

            componentWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113 };
            componentWithResults = new List<long> { 1111111111, 1111111112, 1111111113 };
            SeedAssessmentsAndResults(_registrations, componentWithAssessments, componentWithResults, $"Autumn {currentAcademicYear}");

            SetAssessmentResult(1111111111, $"Summer {currentAcademicYear}", "B", "Merit");
            SetAssessmentResult(1111111112, $"Autumn {currentAcademicYear}", "B", "Pass");

            CommonRepository = new CommonRepository(DbContext);
            UcasRepository = new UcasRepository(DbContext, CommonRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            await Task.CompletedTask;

            var includeResults = true;
            _result = await UcasRepository.GetUcasDataRecordsAsync(includeResults);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, int expectedAssessmentsCount, int expectedResultsCount, string expectedSeriesName, string expectedGrade, 
            int expectedSplAssessmentCount, int expectedSplResultsCount, string expectedSplGrade)
        {
            await WhenAsync();
            _result.Should().NotBeNull();

            var actualPathwayRegistration = _result.FirstOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == uln);
            actualPathwayRegistration.Should().NotBeNull();

            var actualPathwayAssessments = actualPathwayRegistration.TqPathwayAssessments;
            actualPathwayAssessments.Count().Should().Be(expectedAssessmentsCount);

            if (expectedAssessmentsCount > 0)
            {
                var hasExpectedAssessmentSeries = actualPathwayAssessments.FirstOrDefault(x => x.AssessmentSeries.Name.Equals(expectedSeriesName));
                hasExpectedAssessmentSeries.Should().NotBeNull();
            }

            var actualResults = actualPathwayAssessments.SelectMany(x => x.TqPathwayResults);
            actualResults.Count().Should().Be(expectedResultsCount);

            var actualResult = actualResults.FirstOrDefault(x => x.TqPathwayAssessment.AssessmentSeries.Name.Equals(expectedSeriesName, StringComparison.InvariantCultureIgnoreCase));
            if (expectedGrade != null)
                actualResult.TlLookup.Value.Should().Be(expectedGrade);

            // Assert Specialisms
            var actualSpecialismAssessments = actualPathwayRegistration.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments);
            actualSpecialismAssessments.Count().Should().Be(expectedSplAssessmentCount);

            if (expectedSplAssessmentCount > 0)
            {
                var hasExpectedAssessmentSeries = actualSpecialismAssessments.FirstOrDefault(x => x.AssessmentSeries.Name.Equals(expectedSeriesName));
                hasExpectedAssessmentSeries.Should().NotBeNull();
            }

            var actualSpecialismResults = actualSpecialismAssessments.SelectMany(x => x.TqSpecialismResults);
            actualSpecialismResults.Count().Should().Be(expectedSplResultsCount);

            var actualSpecialismResult = actualSpecialismResults.FirstOrDefault(x => x.TqSpecialismAssessment.AssessmentSeries.Name.Equals(expectedSeriesName, StringComparison.InvariantCultureIgnoreCase));
            if (expectedSplGrade != null)
                actualSpecialismResult.TlLookup.Value.Should().Be(expectedSplGrade);

        }

        private void SetAssessmentResult(long uln, string seriesName, string coreGrade, string specialismGrade)
        {
            var currentResult = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.TqPathwayAssessment.AssessmentSeries.Name.Equals(seriesName, StringComparison.InvariantCultureIgnoreCase));
            currentResult.TlLookup = PathwayComponentGrades.FirstOrDefault(x => x.Value.Equals(coreGrade, StringComparison.InvariantCultureIgnoreCase));

            var currentSpecialismResult = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && 
            x.TqSpecialismAssessment.AssessmentSeries.Name.Equals(seriesName, StringComparison.InvariantCultureIgnoreCase));
            var x = DbContext.TqSpecialismResult.Where(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            var y = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.AssessmentSeries.Name.Equals(seriesName, StringComparison.InvariantCultureIgnoreCase));
            currentSpecialismResult.TlLookup = SpecialismComponentGrades.FirstOrDefault(x => x.Value.Equals(specialismGrade, StringComparison.InvariantCultureIgnoreCase));

            DbContext.SaveChanges();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, 2, 2, "Summer 2021", "B", 2, 2, "Merit" },
                    new object[] { 1111111111, 2, 2, "Autumn 2021", "A*", 2, 2, "Distinction" },

                    new object[] { 1111111112, 2, 2, "Summer 2021", "A*", 2, 2, "Distinction" },
                    new object[] { 1111111112, 2, 2, "Autumn 2021", "B", 2, 2, "Pass" },

                    new object[] { 1111111113, 2, 2, "Summer 2021", "A*", 2, 2, "Distinction" },
                    new object[] { 1111111113, 2, 2, "Autumn 2021", "A*", 2, 2, "Distinction" },

                    new object[] { 1111111114, 1, 0, "Summer 2021", null, 1, 0,  null },
                    new object[] { 1111111115, 1, 0, "Summer 2021", null, 1, 0,  null },
                    new object[] { 1111111116, 0, 0, null, null, null, null, null},
                };
            }
        }
    }
}
