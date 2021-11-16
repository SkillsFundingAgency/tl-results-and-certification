using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AssessmentRepositoryTests
{
    public class When_GetBulkPathwayAssessments_IsCalled : AssessmentRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;

        private List<TqPathwayAssessment> _pathwayAssessments;
        private IList<TqPathwayAssessment> _result;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
                { 1111111116, RegistrationPathwayStatus.Active }
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, null);

            var currentAcademicYear = GetAcademicYear();
            _registrations.ForEach(x =>
            {
                x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1);
            });

            var pathwaysWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113, 1111111114, 1111111115 };
            var pathwaysWithResults = new List<long> { 1111111111, 1111111112, 1111111114 };
            var summerAssessments = SeedAssessmentsAndResults(_registrations, pathwaysWithAssessments, pathwaysWithResults, $"Summer {currentAcademicYear}");

            pathwaysWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113 };
            pathwaysWithResults = new List<long> { 1111111111 };
            var autumnAssessments = SeedAssessmentsAndResults(_registrations, pathwaysWithAssessments, pathwaysWithResults, $"Autumn {currentAcademicYear}");

            SetAssessmentResult(1111111111, $"Summer {currentAcademicYear}", "B");

            _pathwayAssessments = summerAssessments.Concat(autumnAssessments).ToList();
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await AssessmentRepository.GetBulkPathwayAssessmentsAsync(_pathwayAssessments);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, string expectedSeriesName, string expectedGrade)
        {
            await WhenAsync();
            _result.Should().NotBeNull();

            var actualAssessment = _result.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);

            if (!string.IsNullOrEmpty(expectedSeriesName))
            {
                actualAssessment.Should().NotBeNull();
                actualAssessment.AssessmentSeries.Name.Should().Be(expectedSeriesName);
            }
            else
                actualAssessment.Should().BeNull();

            if (!string.IsNullOrEmpty(expectedGrade))
            {
                actualAssessment.TqPathwayResults.Count.Should().Be(1);
                var actualResult = actualAssessment.TqPathwayResults.FirstOrDefault();
                actualResult.TlLookup.Value.Should().Be(expectedGrade);
            }
        }

        private void SetAssessmentResult(long uln, string seriesName, string grade)
        {
            var currentResult = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.TqPathwayAssessment.AssessmentSeries.Name.Equals(seriesName, StringComparison.InvariantCultureIgnoreCase));
            currentResult.TlLookup = PathwayComponentGrades.FirstOrDefault(x => x.Value.Equals(grade, StringComparison.InvariantCultureIgnoreCase));
            DbContext.SaveChanges();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, "Autumn 2021", "A*" },
                    new object[] { 1111111112, "Autumn 2021", null },
                    new object[] { 1111111113, "Autumn 2021", null },
                    new object[] { 1111111114, "Summer 2021", "A*" },
                    new object[] { 1111111115, "Summer 2021", null },
                    new object[] { 1111111116, null, null },
                };
            }
        }
    }
}
