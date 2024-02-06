using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public class When_GetFiltersAsync_IsCalled : AdminDashboardServiceBaseTest
    {
        private AdminSearchLearnerFilters _expectedResult;
        private AdminSearchLearnerFilters _actualResult;

        public override void Given()
        {
            var mockAwardingOrganisationFilters = new List<FilterLookupData>
            {
                new FilterLookupData { Id = 1, Name = "Ncfe", IsSelected = false },
                new FilterLookupData { Id = 2, Name = "Pearson", IsSelected = false }
            };

            var mockAcademicYearFilters = new List<FilterLookupData>
            {
                new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false },
                new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false }
            };

            _expectedResult = new AdminSearchLearnerFilters
            {
                AwardingOrganisations = mockAwardingOrganisationFilters,
                AcademicYears = mockAcademicYearFilters
            };

            var today = new DateTime(2023, 1, 1);
            SystemProvider.UtcToday.Returns(today);

            AdminDashboardRepository.GetAwardingOrganisationFiltersAsync().Returns(mockAwardingOrganisationFilters);
            AdminDashboardRepository.GetAcademicYearFiltersAsync(today).Returns(mockAcademicYearFilters);
        }

        public override async Task When()
        {
            _actualResult = await AdminDashboardService.GetAdminSearchLearnerFiltersAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}