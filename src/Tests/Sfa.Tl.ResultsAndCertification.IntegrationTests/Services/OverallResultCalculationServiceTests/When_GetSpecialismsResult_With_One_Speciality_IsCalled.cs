using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetSpecialismsResult_With_One_Speciality_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private TlLookup _actualResult;

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

            _registrations = SeedRegistrationsData(_ulns, null);

            var currentAcademicYear = GetAcademicYear();

            _registrations.ForEach(x =>
            {
                x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1);
            });

            var componentWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113, 1111111114, 1111111115, 1111111116 };
            var componentWithResults = new List<long> { 1111111111, 1111111112, 1111111113, 1111111116 };
            SeedAssessmentsAndResults(_registrations, componentWithAssessments, componentWithResults, $"Summer {currentAcademicYear}");

            componentWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113, 1111111116 };
            componentWithResults = new List<long> { 1111111111, 1111111112, 1111111113, 1111111116 };
            SeedAssessmentsAndResults(_registrations, componentWithAssessments, componentWithResults, $"Autumn {currentAcademicYear}");

            SetAssessmentResult(1111111111, $"Summer {currentAcademicYear}", "B", "Merit");
            SetAssessmentResult(1111111112, $"Autumn {currentAcademicYear}", "B", "Pass");
            SetAssessmentResult(1111111116, $"Autumn {currentAcademicYear}", "Q - pending result", "Q - pending result");

            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(ICollection<TqRegistrationSpecialism> specialisms)
        {
            await Task.CompletedTask;
            _actualResult = await OverallResultCalculationService.GetSpecialismsResult(TlLookup, specialisms);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, bool hasHighestResult, string expectedHighestGrade)
        {
            var pathway = _registrations.SelectMany(r => r.TqRegistrationPathways).FirstOrDefault(r => r.TqRegistrationProfile.UniqueLearnerNumber == uln);

            var specialisms = pathway.TqRegistrationSpecialisms;
            await WhenAsync(specialisms);

            if (hasHighestResult == false)
            {
                _actualResult.Should().BeNull();
                return;
            }

            _actualResult.Value.Should().Be(expectedHighestGrade);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, true, "Distinction" },
                    new object[] { 1111111112, true, "Distinction" },
                    new object[] { 1111111113, true, "Distinction" },
                    new object[] { 1111111114, false, null },
                    new object[] { 1111111115, false, null },
                    new object[] { 1111111116, true, "Q - pending result" }
                };
            }
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
    }
}
