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
    public class When_Cache_Not_Empty_Only_Search_Key_Applied : SearchRegistrationControllerTestBase
    {
        private SearchRegistrationViewModel _viewModel;
        private SearchRegistrationCriteriaViewModel _criteriaViewModel;
        private SearchRegistrationFiltersViewModel _filtersViewModel;
        private SearchRegistrationDetailsListViewModel _loadedSearchRegistrationDetailsList;
        private SearchRegistrationDetailsViewModel _loadedRegistrationDetails;

        private readonly SearchRegistrationType _searchRegitrationType = SearchRegistrationType.Registration;
        private IActionResult _result;

        public override void Given()
        {
            _filtersViewModel = new SearchRegistrationFiltersViewModel
            {
                Search = string.Empty,
                SelectedProviderId = null,
                AcademicYears = new List<FilterLookupData>
                {
                    CreateFilter(2020, "2020 to 2021"),
                    CreateFilter(2021, "2021 to 2022"),
                    CreateFilter(2022, "2022 to 2023"),
                    CreateFilter(2023, "2023 to 2024")
                }
            };

            _criteriaViewModel = new SearchRegistrationCriteriaViewModel
            {
                SearchKey = "smith",
                PageNumber = 1,
                Filters = _filtersViewModel
            };

            _viewModel = new SearchRegistrationViewModel
            {
                Criteria = _criteriaViewModel,
                DetailsList = new SearchRegistrationDetailsListViewModel
                {
                    RegistrationDetails = new List<SearchRegistrationDetailsViewModel>()
                }
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_viewModel);

            _loadedRegistrationDetails = new SearchRegistrationDetailsViewModel
            {
                RegistrationProfileId = 2000,
                Uln = 1234567890,
                LearnerName = "Kevin Smith",
                Provider = "Exeter College (10002370)",
                Core = "Agriculture, Land Management and Production",
                StartYear = "2022 to 2023"
            };

            _loadedSearchRegistrationDetailsList = new SearchRegistrationDetailsListViewModel
            {
                RegistrationDetails = new List<SearchRegistrationDetailsViewModel> { _loadedRegistrationDetails },
                TotalRecords = 1,
                PagerInfo = new PagerViewModel
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    RecordFrom = 1,
                    RecordTo = 1,
                    StartPage = 1,
                    TotalItems = 1,
                    TotalPages = 1
                }
            };

            SearchRegistrationLoader.GetSearchRegistrationDetailsListAsync(NcfeUkprn, _searchRegitrationType, _criteriaViewModel)
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
            SearchRegistrationLoader.Received(1).GetSearchRegistrationDetailsListAsync(NcfeUkprn, _searchRegitrationType, _criteriaViewModel);
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
            model.Criteria.Should().BeEquivalentTo(_criteriaViewModel);
            model.Criteria.Filters.Should().BeEquivalentTo(_filtersViewModel);

            model.Pagination.PagerInfo.Should().BeEquivalentTo(_loadedSearchRegistrationDetailsList.PagerInfo);
        }
    }
}