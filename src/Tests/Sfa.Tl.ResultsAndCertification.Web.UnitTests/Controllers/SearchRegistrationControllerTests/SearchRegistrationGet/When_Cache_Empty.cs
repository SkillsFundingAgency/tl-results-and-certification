using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationGet
{
    public class When_Cache_Empty : SearchRegistrationControllerTestBase
    {
        private SearchRegistrationViewModel _viewModel;

        private readonly SearchRegistrationType _searchRegitrationType = SearchRegistrationType.Registration;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = new SearchRegistrationViewModel
            {
                SearchType = _searchRegitrationType,
                Criteria = new SearchRegistrationCriteriaViewModel
                {
                    SearchKey = string.Empty,
                    Filters = new SearchRegistrationFiltersViewModel
                    {
                        SelectedProviderId = null,
                        AcademicYears = new List<FilterLookupData>
                        {
                            CreateFilter(2020, "2020 to 2021"),
                            CreateFilter(2021, "2021 to 2022"),
                            CreateFilter(2022, "2022 to 2023"),
                            CreateFilter(2023, "2023 to 2024")
                        }
                    }
                }
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(null as SearchRegistrationViewModel);
            SearchRegistrationLoader.CreateSearchRegistration(_searchRegitrationType).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationAsync(_searchRegitrationType);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            SearchRegistrationLoader.Received(1).CreateSearchRegistration(_searchRegitrationType);

            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<SearchRegistrationViewModel>(
                p => p.SearchType == _searchRegitrationType
                && p.Criteria.SearchKey == string.Empty
                && !p.Criteria.PageNumber.HasValue
                && p.Criteria.Filters.Search == string.Empty
                && !p.Criteria.Filters.SelectedProviderId.HasValue
                && p.Criteria.Filters.AcademicYears.All(p => !p.IsSelected)));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = _result.ShouldBeViewResult<SearchRegistrationViewModel>();
            model.Should().BeEquivalentTo(_viewModel);
        }
    }
}