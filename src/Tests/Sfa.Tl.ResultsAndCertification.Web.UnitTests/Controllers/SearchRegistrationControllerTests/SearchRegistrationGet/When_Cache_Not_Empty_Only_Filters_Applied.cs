using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationGet
{
    public class When_Cache_Not_Empty_Only_Filters_Applied : SearchRegistrationControllerTestBase
    {
        private readonly SearchRegistrationType _searchRegitrationType = SearchRegistrationType.Registration;

        private SearchRegistrationViewModel _viewModel;
        private SearchRegistrationDetailsListViewModel _loadedSearchRegistrationDetailsList;

        private IActionResult _result;

        public override void Given()
        {
            var filtersViewModel = new SearchRegistrationFiltersViewModel()
            {
                Search = "Exeter College",
                SelectedProviderId = 16,
                AcademicYears = new List<FilterLookupData>
                {
                    CreateFilter(2020, "2020 to 2021", isSelected: true),
                    CreateFilter(2021, "2021 to 2022"),
                    CreateFilter(2022, "2022 to 2023"),
                    CreateFilter(2023, "2023 to 2024")
                }
            };

            var criteriaViewModel = new SearchRegistrationCriteriaViewModel
            {
                SearchKey = string.Empty,
                PageNumber = 1,
                Filters = filtersViewModel
            };

            _viewModel = new SearchRegistrationViewModel
            {
                Criteria = criteriaViewModel,
                DetailsList = new SearchRegistrationDetailsListViewModel
                {
                    RegistrationDetails = new List<SearchRegistrationDetailsViewModel>()
                }
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_viewModel);

            var firstLoadedRegistrationDetails = new SearchRegistrationDetailsViewModel
            {
                RegistrationProfileId = 2000,
                Uln = 1234567890,
                LearnerName = "Kevin Smith",
                Provider = "Exeter College (10002370)",
                Core = "Agriculture, Land Management and Production",
                StartYear = "2020 to 2021"
            };

            var secondLoadedRegistrationDetails = new SearchRegistrationDetailsViewModel
            {
                RegistrationProfileId = 3537,
                Uln = 9876543210,
                LearnerName = "Emily Taylor",
                Provider = "Exeter College (10002370)",
                Core = "Design and Development for Engineering and Manufacturing",
                StartYear = "2020 to 2021"
            };

            _loadedSearchRegistrationDetailsList = new SearchRegistrationDetailsListViewModel
            {
                RegistrationDetails = new List<SearchRegistrationDetailsViewModel> { firstLoadedRegistrationDetails, secondLoadedRegistrationDetails },
                TotalRecords = 1,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    RecordFrom = 1,
                    RecordTo = 2,
                    StartPage = 1,
                    TotalItems = 2,
                    TotalPages = 1
                }
            };

            SearchRegistrationLoader.GetSearchRegistrationDetailsListAsync(NcfeUkprn, _searchRegitrationType, criteriaViewModel)
                .Returns(_loadedSearchRegistrationDetailsList);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationAsync(_searchRegitrationType);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            SearchRegistrationLoader.Received(1).GetSearchRegistrationDetailsListAsync(NcfeUkprn, _searchRegitrationType, _viewModel.Criteria);
            CacheService.Received(1).SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = _result.ShouldBeViewResult<SearchRegistrationViewModel>();

            model.State.Should().Be(SearchRegistrationState.ResultsFound);
            model.ContainsResults.Should().BeTrue();
            model.ContainsMultipleResultsPages.Should().BeFalse();

            model.DetailsList.Should().BeEquivalentTo(_loadedSearchRegistrationDetailsList);
            model.Criteria.Should().BeEquivalentTo(_viewModel.Criteria);
            model.Criteria.Filters.Should().BeEquivalentTo(_viewModel.Criteria.Filters);

            model.Pagination.PagerInfo.Should().BeEquivalentTo(_loadedSearchRegistrationDetailsList.PagerInfo);
        }
    }
}