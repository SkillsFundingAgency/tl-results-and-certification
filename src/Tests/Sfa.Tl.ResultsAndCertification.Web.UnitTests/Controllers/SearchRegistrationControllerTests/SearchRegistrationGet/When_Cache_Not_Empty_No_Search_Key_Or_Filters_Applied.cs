using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests.SearchRegistrationGet
{
    public class When_Cache_Not_Empty_No_Search_Key_Or_Filters_Applied : SearchRegistrationControllerTestBase
    {
        private readonly SearchRegistrationType _searchRegitrationType = SearchRegistrationType.Registration;

        private SearchRegistrationViewModel _viewModel;
        private IActionResult _result;

        public override void Given()
        {
            _viewModel = new SearchRegistrationViewModel
            {
                SearchType = _searchRegitrationType,
                State = SearchRegistrationState.ResultsFound,
                Criteria = new SearchRegistrationCriteriaViewModel
                {
                    SearchKey = string.Empty,
                    PageNumber = null,
                    Filters = new SearchRegistrationFiltersViewModel
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
                    }
                },
                DetailsList = new SearchRegistrationDetailsListViewModel
                {
                    RegistrationDetails = new List<SearchRegistrationDetailsViewModel>
                    {
                        new()
                        {
                            RegistrationProfileId = 1500,
                            Uln = 1111111111,
                            LearnerName = "John Smith",
                            Provider = "Barnsley College (10000536)",
                            Core = "Agriculture, Land Management and Production",
                            StartYear = "2020 to 2021"
                        }
                    }
                }
            };

            CacheService.GetAsync<SearchRegistrationViewModel>(CacheKey).Returns(_viewModel);
        }

        public override async Task When()
        {
            _result = await Controller.SearchRegistrationAsync(_searchRegitrationType);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchRegistrationViewModel>(CacheKey);
            SearchRegistrationLoader.DidNotReceive().CreateSearchRegistration(Arg.Any<SearchRegistrationType>());
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<SearchRegistrationViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = _result.ShouldBeViewResult<SearchRegistrationViewModel>();

            model.SearchType.Should().Be(SearchRegistrationType.Registration);
            model.State.Should().Be(SearchRegistrationState.NoSearch);

            model.DetailsList.Should().NotBeNull();
            model.DetailsList.RegistrationDetails.Should().BeEmpty();
            model.DetailsList.TotalRecords.Should().Be(0);
            model.DetailsList.PagerInfo.Should().BeNull();
        }
    }
}