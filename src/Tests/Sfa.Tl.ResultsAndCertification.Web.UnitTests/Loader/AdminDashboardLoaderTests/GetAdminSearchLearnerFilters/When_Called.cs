using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetAdminSearchLearnerFilters
{
    public class When_Called : AdminDashboardLoaderTestsBase
    {
        private AdminSearchLearnerFiltersViewModel _expectedResult;
        private AdminSearchLearnerFiltersViewModel _actualResult;

        public override void Given()
        {
            var expectedApiResult = new AdminSearchLearnerFilters
            {
                AwardingOrganisations = new List<FilterLookupData>
                {
                    new() { Id = 1, Name = "Ncfe", IsSelected = false },
                    new() { Id = 2, Name = "Pearson", IsSelected = false }
                },
                AcademicYears = new List<FilterLookupData>
                {
                    new() { Id = 1, Name = "2020 to 2021", IsSelected = false }
                }
            };

            ApiClient.GetAdminSearchLearnerFiltersAsync().Returns(expectedApiResult);

            _expectedResult = new AdminSearchLearnerFiltersViewModel
            {
                AwardingOrganisations = new List<FilterLookupData>
                {
                    new() { Id = 1, Name = "Ncfe", IsSelected = false },
                    new() { Id = 2, Name = "Pearson", IsSelected = false }
                },
                AcademicYears = new List<FilterLookupData>
                {
                    new() { Id = 1, Name = "2020 to 2021", IsSelected = false }
                }
            };
        }

        public override async Task When()
        {
            _actualResult = await Loader.GetAdminSearchLearnerFiltersAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}