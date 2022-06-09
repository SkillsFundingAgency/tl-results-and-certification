using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerDetails
{
    public class When_SearchLearnerDetails_IsNull : TestSetup
    {
        private SearchLearnerFiltersViewModel _searchFilters;
        private SearchLearnerDetailsListViewModel _searchLearnersList;

        public override void Given()
        {
            _searchFilters = new SearchLearnerFiltersViewModel
            {
                AcademicYears = new List<FilterLookupData>
                {
                    new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                    new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }
                }
            };
            TrainingProviderLoader.GetSearchLearnerFiltersAsync(ProviderUkprn).Returns(_searchFilters);

            AcademicYear = 2020;
            TrainingProviderLoader.SearchLearnerDetailsAsync(ProviderUkprn, AcademicYear).Returns(_searchLearnersList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetSearchLearnerFiltersAsync(ProviderUkprn);
            TrainingProviderLoader.Received(1).SearchLearnerDetailsAsync(ProviderUkprn, AcademicYear);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
