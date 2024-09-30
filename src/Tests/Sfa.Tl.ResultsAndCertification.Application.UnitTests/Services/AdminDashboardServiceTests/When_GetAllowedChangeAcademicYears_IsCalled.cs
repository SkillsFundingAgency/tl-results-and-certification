using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public class When_GetAllowedChangeAcademicYears_IsCalled : AdminDashboardServiceBaseTest
    {
        private const int LearnerAcademicYear = 2022, PathwayStartYear = 2020;
        private readonly IList<int> _expectedResult = new List<int> { 2020, 2021, 2023 };

        private IList<int> _actualResult;

        public override void Given()
        {
            AdminDashboardRepository.GetAllowedChangeAcademicYearsAsync(Arg.Any<Func<DateTime>>(), LearnerAcademicYear, PathwayStartYear).Returns(_expectedResult);
        }

        public override async Task When()
        {
            _actualResult = await AdminDashboardService.GetAllowedChangeAcademicYearsAsync(LearnerAcademicYear, PathwayStartYear);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}