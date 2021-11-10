using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class When_CompareAndProcessAssessmentsAsync_Called_With_Invalid_Assessments : AssessmentServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private AssessmentProcessResponse _result;
        
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqSpecialismAssessment> _specialismAssessments;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active }
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            var currentAcademicYear = GetAcademicYear();
            _registrations.ForEach(x =>
            {
                x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1);
            });

            // Summer Assessment/Results
            var pathwaysWithAssessments = new List<long> { 1111111111 };
            var pathwaysWithResults = new List<long>();
            var summerAssessments = SeedAssessmentsAndResults(_registrations, pathwaysWithAssessments, pathwaysWithResults, $"Summer {currentAcademicYear}");

            // Autumn Assessment/Results
            pathwaysWithAssessments = new List<long> { 1111111112 };
            pathwaysWithResults = new List<long> { 1111111112 };
            var autumnAssessments = SeedAssessmentsAndResults(_registrations, pathwaysWithAssessments, pathwaysWithResults, $"Autumn {currentAcademicYear}");

            // Create a test class instance.
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);

            _pathwayAssessments = summerAssessments.Concat(autumnAssessments).ToList();
            _specialismAssessments = new List<TqSpecialismAssessment>();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(List<TqPathwayAssessment> pathwayAssessments, List<TqSpecialismAssessment> specialismAssessments)
        {
            _result = await AssessmentService.CompareAndProcessAssessmentsAsync(pathwayAssessments, specialismAssessments);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, string seriesName, string expectedValidationMessage)
        {
            var pathwayAssessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            var series = DbContext.AssessmentSeries.FirstOrDefault(x => x.Name.Equals(seriesName, System.StringComparison.InvariantCultureIgnoreCase));

            var pathwayAssessments = new List<TqPathwayAssessment>
            {
                new TqPathwayAssessment
                {
                    TqRegistrationPathwayId = pathwayAssessment.Id,
                    AssessmentSeriesId = !string.IsNullOrEmpty(seriesName) ? series.Id : 0
                }
            };
            
            await WhenAsync(pathwayAssessments, _specialismAssessments);

            _result.Should().NotBeNull();
            _result.ValidationErrors.Count.Should().Be(1);
            _result.ValidationErrors.FirstOrDefault().ErrorMessage.Should().Be(expectedValidationMessage);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, "Autumn 2021", ValidationMessages.AssessmentEntryCannotBeAddedUntilResultRecordedForExistingEntry },
                    new object[] { 1111111112, null, ValidationMessages.AssessmentEntryCannotBeRemovedHasResult }
                };
            }
        }
    }
}
