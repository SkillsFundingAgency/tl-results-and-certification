using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.SearchRegistrationServiceTests
{
    public class When_GetFiltersAsync_IsCalled : SearchRegistrationServiceBaseTest
    {
        private SearchRegistrationFilters _expectedResult;
        private SearchRegistrationFilters _actualResult;

        public override void Given()
        {
            var mockAcademicYearFilters = new List<FilterLookupData>
            {
                new() { Id = 2021, Name = "2021 to 2022", IsSelected = false },
                new() { Id = 2022, Name = "2022 to 2023", IsSelected = false }
            };

            _expectedResult = new SearchRegistrationFilters
            {
                AcademicYears = mockAcademicYearFilters
            };

            SearchRegistrationRepository.GetAcademicYearFiltersAsync(Arg.Any<Func<DateTime>>()).Returns(mockAcademicYearFilters);
        }

        public override async Task When()
        {
            _actualResult = await RegistrationService.GetSearchFiltersAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}