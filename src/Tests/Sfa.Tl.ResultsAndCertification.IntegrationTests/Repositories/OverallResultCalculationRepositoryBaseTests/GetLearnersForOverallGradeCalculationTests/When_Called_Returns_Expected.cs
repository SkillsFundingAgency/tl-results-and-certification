using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.OverallResultCalculationRepositoryBaseTests.GetLearnersForOverallGradeCalculationTests
{
    public class When_Called_Returns_Expected : GetLearnersForOverallGradeCalculationBaseTest
    {
        private const string AssessmentSeriesName = "Autumn 2024";

        private IOverallResultCalculationRepository _repository;
        private IList<TqRegistrationPathway> _results;

        public override void Given()
        {
            SeedTestData();
            _repository = CreateOverallResultCalculationRepository();
        }

        public override async Task When()
        {
            AssessmentSeries assessmentSeries = CoreAssessmentSeries[AssessmentSeriesName];
            _results = await _repository.GetLearnersForOverallGradeCalculation(assessmentSeries);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            _results.Should().BeEquivalentTo(ExpectedResults);
        }
    }
}