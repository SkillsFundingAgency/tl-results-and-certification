using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationClearFiltersPost
{
    public class When_Called_And_Cache_Contains_Valid_Data : SearchRegistrationControllerTestBase
    {
        private const string PageNumberKey = "pageNumber";
        private const int PageNumber = 1;

        private SearchRegistrationViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = new SearchRegistrationViewModel
            {
                Criteria = new SearchRegistrationCriteriaViewModel
                {
                    SearchKey = "johnson",
                    PageNumber = PageNumber,
                    Filters = new SearchRegistrationFiltersViewModel
                    {
                        Search = "Barnsley College",
                        SelectedProviderId = 125,
                        AcademicYears = new List<FilterLookupData>
                        {
                            CreateFilter(2020, "2020 to 2021", isSelected : true),
                            CreateFilter(2021, "2021 to 2022", isSelected : true),
                            CreateFilter(2022, "2022 to 2023", isSelected : true),
                            CreateFilter(2023, "2023 to 2024", isSelected : true)
                        }
                    }
                }
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationClearFiltersAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchRegistrationViewModel>(
                p => p.Criteria.SearchKey == _viewModel.Criteria.SearchKey
                && p.Criteria.PageNumber == PageNumber
                && p.Criteria.Filters.Search == string.Empty
                && !p.Criteria.Filters.SelectedProviderId.HasValue
                && p.Criteria.Filters.AcademicYears.All(p => !p.IsSelected)));
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            SearchRegistrationCriteriaViewModel criteria = _viewModel.Criteria;
            SearchRegistrationFiltersViewModel filters = criteria.Filters;

            criteria.Should().NotBeNull();
            criteria.SearchKey.Should().Be(_viewModel.Criteria.SearchKey);
            criteria.PageNumber.Should().Be(PageNumber);

            filters.Search.Should().BeEmpty();
            filters.SelectedProviderId.Should().NotHaveValue();
            filters.AcademicYears.Should().AllSatisfy(p => p.IsSelected.Should().BeFalse());
        }

        [Fact]
        public void Then_Redirected_To_SearchRegistrationsRecords()
        {
            _result.ShouldBeRedirectToRouteResult(
                RouteConstants.SearchRegistration,
                (Constants.Type, _viewModel.SearchType),
                (PageNumberKey, _viewModel.Criteria.PageNumber));
        }
    }
}